using UnityEngine;
using NativeWebSocket;
using Newtonsoft.Json;

public class WebsocketManager : MonoBehaviour
{
    // Used https://github.com/endel/NativeWebSocket

    // Used https://medium.com/unity-nodejs/websocket-client-server-unity-nodejs-e33604c6a006

    Formatting jsonFormatting;
    JsonSerializerSettings jsonSettings;

    public WebSocket websocket
    {
        get => Globals.websocket;
    }

    private void Start()
    {
        Debug.Log("<color=orange>[WebsocketManager]</color> Mounting websocket");

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
        Debug.Log("<color=orange>[WebsocketManager]</color> Starting websocket");

        Globals.websocket = new WebSocket("wss://hvg-server.deno.dev/v1/socket");

        Globals.websocket.OnOpen += OpenWebsocket;
        Globals.websocket.OnError += ErrorWebsocket;
        Globals.websocket.OnClose += CloseWebsocket;

        await Globals.websocket.Connect();
    }

    async public void ConnectWebsocket() {
        Debug.Log("<color=orange>[WebsocketManager]</color> Restoring websocket");

        await Globals.websocket.Connect();
    }

    private void OpenWebsocket() {
        Debug.Log("<color=orange>[WebsocketManager]</color> Connection has been opened");
    }

    private void ErrorWebsocket(string error) {
        Debug.Log("<color=orange>[WebsocketManager]</color> Connection error occurred" + error);
    }

    private void CloseWebsocket(WebSocketCloseCode error) {
        Debug.Log("<color=orange>[WebsocketManager]</color> Connection has been closed");
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

    private void OnApplicationQuit()
    {
        if (Globals.websocket != null)
        {
            Globals.websocket.CancelConnection();
        }
    }
}