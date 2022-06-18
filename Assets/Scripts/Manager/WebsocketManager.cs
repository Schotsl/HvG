using UnityEngine;
using NativeWebSocket;
using Newtonsoft.Json;
using System.Collections.Generic;

public class WebsocketManager : MonoBehaviour
{
    // Used https://github.com/endel/NativeWebSocket

    // Used https://medium.com/unity-nodejs/websocket-client-server-unity-nodejs-e33604c6a006

    public List<ClueListener> clueListeners;
    public List<LaserListener> laserListeners;
    public List<HostingListener> hostingListeners;
    public List<PositionListener> positionListeners;
    public List<SubscribeListener> subscribeListeners;
    
    Formatting jsonFormatting;
    JsonSerializerSettings jsonSettings;

    public WebSocket websocket
    {
        get => Globals.websocket;
    }

    WebsocketManager() {
        clueListeners = new List<ClueListener>();
        laserListeners = new List<LaserListener>();
        hostingListeners = new List<HostingListener>();
        positionListeners = new List<PositionListener>();
        subscribeListeners = new List<SubscribeListener>();
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

    private void RestartListener() {
        if (Globals.websocket != null) {
            Globals.websocket.OnMessage -= MessageWebsocket;
            Globals.websocket.OnMessage += MessageWebsocket;
        }  
    }

    async public void StartWebsocket()
    {   
        Debug.Log("<color=orange>[WebsocketManager]</color> Starting websocket");

        Globals.websocket = new WebSocket("wss://hvg-server.deno.dev/v1/socket");

        Globals.websocket.OnOpen += OpenWebsocket;
        Globals.websocket.OnError += ErrorWebsocket;
        Globals.websocket.OnClose += CloseWebsocket;
        Globals.websocket.OnMessage += MessageWebsocket;

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

    private void MessageWebsocket(byte[] bytes) {
        string message = System.Text.Encoding.UTF8.GetString(bytes);
        Update update = JsonConvert.DeserializeObject<Update>(message, jsonSettings);

        switch (update.type) {
            case Type.Clue:
                ClueUpdate clueUpdate = JsonConvert.DeserializeObject<ClueUpdate>(message);
                SendClue(clueUpdate);
                break;
            case Type.Laser:
                LaserUpdate laserUpdate = JsonConvert.DeserializeObject<LaserUpdate>(message);
                SendLaser(laserUpdate);
                break;
            case Type.Hosting:
                HostingUpdate hostingUpdate = JsonConvert.DeserializeObject<HostingUpdate>(message);
                SendHosting(hostingUpdate);
                break;
            case Type.Position:
                PositionUpdate positionUpdate = JsonConvert.DeserializeObject<PositionUpdate>(message);
                SendPosition(positionUpdate);
                break;
            case Type.Subscribe:
                SubscribeUpdate subscribeUpdate = JsonConvert.DeserializeObject<SubscribeUpdate>(message);
                SendSubscribe(subscribeUpdate);
                break;
            default:
                Debug.Log("<color=orange>[WebsocketManager]</color> An unknown message has been received");
                break;

        }
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



    public void AddClue(ClueCallback callback, string target) {
        ClueListener listener = new ClueListener(callback, target);
        
        clueListeners.Add(listener);

        RestartListener();
    }

    public void SendClue(ClueUpdate update) {
        clueListeners.ForEach((listener) => {
            if (listener.target == update.target) {
                listener.callback();
            }
        });
    }

    public void RemoveClue(ClueCallback callback, string target) {
        ClueListener listener = new ClueListener(callback, target);

        clueListeners.Remove(listener);

        RestartListener();
    }

    

    public void AddLaser(LaserCallback callback, string target) {
        LaserListener listener = new LaserListener(callback, target);

        laserListeners.Add(listener);

        RestartListener();
    }

    public void SendLaser(LaserUpdate update) {
        laserListeners.ForEach((listener) => {
            if (listener.target == update.target) {
                listener.callback(update.triggered);
            }
        });
    }

    public void RemoveLaser(LaserCallback callback, string target) {
        LaserListener listener = new LaserListener(callback, target);

        laserListeners.Remove(listener);

        RestartListener();
    }



    public void AddHosting(HostingCallback callback) {
        HostingListener listener = new HostingListener(callback);

        hostingListeners.Add(listener);

        RestartListener();
    }

    public void SendHosting(HostingUpdate update) {
        Debug.Log("bruh");
        hostingListeners.ForEach((listener) => {
            listener.callback(update.success);
        });
    }

    public void RemoveHosting(HostingCallback callback) {
        HostingListener listener = new HostingListener(callback);

        hostingListeners.Remove(listener);

        RestartListener();
    }



    public void AddPosition(PositionCallback callback, string target) {
        PositionListener listener = new PositionListener(callback, target);

        positionListeners.Add(listener);

        RestartListener();
    }

    public void SendPosition(PositionUpdate update) {
        positionListeners.ForEach((listener) => {
            if (listener.target == update.target) {
                listener.callback(update.x, update.y);
            }
        });
    }

    public void RemovePosition(PositionCallback callback, string target) {
        PositionListener listener = new PositionListener(callback, target);

        positionListeners.Remove(listener);

        RestartListener();
    }



    public void AddSubscribe(SubscribeCallback callback) {
        SubscribeListener listener = new SubscribeListener(callback);

        subscribeListeners.Add(listener);

        RestartListener();
    }

    public void SendSubscribe(SubscribeUpdate update) {
        subscribeListeners.ForEach((listener) => {
            listener.callback(update.success);
        });
    }

    public void RemoveSubscribe(SubscribeCallback callback) {
        SubscribeListener listener = new SubscribeListener(callback);

        subscribeListeners.Remove(listener);

        RestartListener();
    }
}