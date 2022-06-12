using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using System.Threading;
using System.Threading.Tasks;

public class PlayerWatcher : MonoBehaviour
{
  WebSocket websocket;

  async void OnMove()
  {
    Transform transform = GetComponent<Transform>();

    Patch patch = new Patch();

    patch.x = transform.position.x;
    patch.y = transform.position.y;
    
    patch.uuid = SystemInfo.deviceUniqueIdentifier;

    string message = JsonUtility.ToJson(patch);

    if (websocket.State == WebSocketState.Open)
    {
      websocket.SendText(message);
    }
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
    websocket = new WebSocket("wss://hvg-server.deno.dev/v1/socket");

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
