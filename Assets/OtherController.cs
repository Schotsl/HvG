using System;

using UnityEngine;

using NativeWebSocket;

public class Patch
{
  public float x;
  public float y;
  public string uuid;
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
      Patch patch = JsonUtility.FromJson<Patch>(message);

      // Ignore our own updates
      if (patch.uuid != SystemInfo.deviceUniqueIdentifier)
      {
        Vector2 vector = new Vector2(patch.x, patch.y);

        // Get the transform component of the player
        Transform transform = gameObject.GetComponent<Transform>();

        // Update the actual position of the player
        transform.position = vector;
      }
    };

    // waiting for messages
    await websocket.Connect();
  }

  private async void OnApplicationQuit()
  {
    await websocket.Close();
  }

}