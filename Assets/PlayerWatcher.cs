using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

public class Update
{
  public float x;
  public float y;
  public string uuid;
}


public class PlayerWatcher : MonoBehaviour
{
 WebSocket websocket;

  void OnMove() {
    Transform transform = GetComponent<Transform>();

    Update update = new Update();

    update.x = transform.position.x;
    update.y = transform.position.y;
    update.uuid = SystemInfo.deviceUniqueIdentifier;

    string message = JsonUtility.ToJson(update);

    SendWebsocket(message);
  }

  async void Start()
  {
    StartWebsocket();
  }

  async void Update()
  {
    try
    {
      #if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
      #endif
    }
    catch
    {
      StartWebsocket();
    }
  }

  async void StartWebsocket()
  {
    websocket = new WebSocket("ws://localhost:8080/v1/socket");

    websocket.OnOpen += () => Debug.Log("Connection open!");
    websocket.OnError += (e) => Debug.Log("Error! " + e);
    websocket.OnClose += (e) => Debug.Log("Connection closed!");

    // waiting for messages
    await websocket.Connect();
  }

  async void SendWebsocket(string message)
  {
    if (websocket.State == WebSocketState.Open)
    {
      await websocket.SendText(message);
    }
  }

  private async void OnApplicationQuit()
  {
    await websocket.Close();
  }
}
