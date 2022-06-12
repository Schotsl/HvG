using UnityEngine;
using NativeWebSocket;
using UnityEngine.InputSystem;

public class PlayerWatcher : MonoBehaviour
{
  WebSocket websocket;

  void Start()
  {
    StartWebsocket();
  }

  async void OnMove(InputValue inputValue)
  {
    // We can abort the update if the WebSocket is closed 
    if (websocket.State != WebSocketState.Open) {
      return;
    }

    Vector2 velocity = inputValue.Get<Vector2>();
    Transform transform = GetComponent<Transform>();
    
    Patch patch = new Patch();

    patch.x_pos = transform.position.x;
    patch.y_pos = transform.position.y;

    patch.x_speed = velocity.x;
    patch.y_speed = velocity.y;
    
    patch.uuid = SystemInfo.deviceUniqueIdentifier;

    string message = JsonUtility.ToJson(patch);
    await websocket.SendText(message);
  }

  async void StartWebsocket()
  {
    websocket = new WebSocket("wss://hvg-server.deno.dev/v1/socket");

    websocket.OnOpen += () => Debug.Log("Connection open!");
    websocket.OnError += (e) => Debug.Log("Error! " + e);
    websocket.OnClose += (e) => Debug.Log("Connection closed!");

    await websocket.Connect();
  }

  private async void OnApplicationQuit()
  {
    await websocket.Close();
  }
}
