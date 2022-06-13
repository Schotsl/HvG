using UnityEngine;
using NativeWebSocket;
using UnityEngine.InputSystem;
using Newtonsoft.Json;

public class PlayerWatcher : MonoBehaviour
{
  WebSocket websocket;

  Transform transform;
  Rigidbody2D rigidbody;

  Formatting formatting;
  JsonSerializerSettings settings;

  void Start()
  {
    StartWebsocket();

    GameObject other = GameObject.Find("Player 2");

    transform = other.GetComponent<Transform>();
    rigidbody = other.GetComponent<Rigidbody2D>();

    settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
    formatting = Formatting.None;
  }

  void Update()
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

  async void OnMove(InputValue inputValue)
  {
    // We can abort the update if the WebSocket is closed 
    if (websocket.State != WebSocketState.Open) {
      return;
    }

    Vector2 velocity = inputValue.Get<Vector2>();
    Transform transform = GetComponent<Transform>();
    
    Patch patch = new Patch();

    patch.q = velocity.x == 0 ? null : velocity.x;
    patch.w = velocity.y == 0 ? null : velocity.y;
    patch.e = transform.position.x == 0 ? null : transform.position.x;
    patch.r = transform.position.y == 0 ? null : transform.position.y;
    
    string message = JsonConvert.SerializeObject(patch, formatting, settings);
    await websocket.SendText(message);
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

      Patch patch = JsonConvert.DeserializeObject<Patch>(message);

      Vector2 position = new Vector2(patch.e ?? 0, patch.r ?? 0);
      Vector2 velocity = new Vector2(patch.q ?? 0, patch.w ?? 0);

      // Actually update the position and velocity
      rigidbody.velocity = velocity;
      transform.position = position;
    };

    await websocket.Connect();
  }

  private async void OnApplicationQuit()
  {
    await websocket.Close();
  }
}
