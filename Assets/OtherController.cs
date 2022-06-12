using System;

using UnityEngine;

using NativeWebSocket;

public class Location
{
  public float x;
  public float y;
}

public class OtherController : MonoBehaviour
{
  WebSocket websocket;

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
    websocket.OnMessage += (bytes) =>
    {
      string message = System.Text.Encoding.UTF8.GetString(bytes);

      Location location = JsonUtility.FromJson<Location>(message);
      Vector2 vector = new Vector2(location.x, location.y);

      // Get the transform component of the player
      Transform transform = GetComponent<Transform>();

      // Update the actual position of the player
      transform.position = vector;
    };

    // Keep sending messages at every 0.3s
    InvokeRepeating("SendWebsocket", 0.0f, 0.3f);

    // waiting for messages
    await websocket.Connect();
  }

  async void SendWebsocket()
  {
    if (websocket.State == WebSocketState.Open)
    {
      // Sending bytes
      await websocket.Send(new byte[] { 10, 20, 30 });

      // Sending plain text
      await websocket.SendText("plain text message");
    }
  }

  private async void OnApplicationQuit()
  {
    await websocket.Close();
  }

}