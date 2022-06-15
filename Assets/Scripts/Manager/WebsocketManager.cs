using UnityEngine;
using NativeWebSocket;
using Newtonsoft.Json;

public class WebsocketManager : MonoBehaviour
{
    Formatting jsonFormatting;
    JsonSerializerSettings jsonSettings;

    public WebSocket websocket
    {
        get => Globals.websocket;
    }

    private void Start()
    {
        jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        jsonFormatting = Formatting.None;
    }

    private void Update()
    {
        if (Globals.websocket != null) {
            #if !UNITY_WEBGL || UNITY_EDITOR
                Globals.websocket.DispatchMessageQueue();
            #endif
        }
    }

    async public void StartWebsocket()
    {   
        Globals.websocket = new WebSocket("wss://hvg-server.deno.dev/v1/socket");

        Globals.websocket.OnOpen += OpenWebsocket;
        Globals.websocket.OnError += ErrorWebsocket;
        Globals.websocket.OnClose += CloseWebsocket;

        await Globals.websocket.Connect();
    }

    private void OpenWebsocket() {
        Debug.Log("Connection open!");
    }

    private void ErrorWebsocket(string error) {
        Debug.Log("Error: " + error);
    }

    private void CloseWebsocket(WebSocketCloseCode error) {
        Debug.Log("Connection closed!");
    }

    async public void SendWebsocket(object data)
    {
        if (Globals.websocket == null) {
            return;
        }

        string json = JsonConvert.SerializeObject(data, jsonFormatting, jsonSettings);

        // We can abort the update if the WebSocket is closed 
        if (Globals.websocket.State != WebSocketState.Open)
        {
            await Globals.websocket.Connect();
        }
        
        await Globals.websocket.SendText(json);
    }

    private async void OnApplicationQuit()
    {
        if (Globals.websocket != null)
        {
            await Globals.websocket.Close();
        }
    }
}