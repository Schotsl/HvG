using UnityEngine;
using NativeWebSocket;
using UnityEngine.InputSystem;
using Newtonsoft.Json;

public class PlayerWatcher : MonoBehaviour
{
  WebSocket websocket;

  Joystick joystickObject;
  float joystickVertical;
  float joystickHorizontal;

  Transform playerTransform;

  Animator otherAnimator;
  Transform otherTransform;
  Rigidbody2D otherRigidbody;

  Formatting jsonFormatting;
  JsonSerializerSettings jsonSettings;

  void Start()
  {
    StartWebsocket();

    playerTransform = GetComponent<Transform>();

    joystickObject = FindObjectOfType<Joystick>();
    joystickVertical = joystickObject.Vertical;
    joystickHorizontal = joystickObject.Horizontal;

    GameObject otherObject = GameObject.Find("Player 2");

    otherAnimator = otherObject.GetComponent<Animator>();
    otherTransform = otherObject.GetComponent<Transform>();
    otherRigidbody = otherObject.GetComponent<Rigidbody2D>();

    jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
    jsonFormatting = Formatting.None;

    // Update the server every 100 seconds
    if (SystemInfo.deviceType == DeviceType.Handheld) {
      InvokeRepeating("OnJoystick", 0f, 0.1f);
    }
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

  async void OnJoystick() {
    if (joystickObject.Horizontal != joystickHorizontal || joystickObject.Vertical != joystickVertical) {
      Patch patch = new Patch();

      patch.w = joystickObject.Vertical == 0 ? null : joystickObject.Vertical;
      patch.q = joystickObject.Horizontal == 0 ? null : joystickObject.Horizontal;

      patch.e = playerTransform.position.x == 0 ? null : playerTransform.position.x;
      patch.r = playerTransform.position.y == 0 ? null : playerTransform.position.y;

      SendPatch(patch);

      joystickVertical = joystickObject.Vertical;
      joystickHorizontal = joystickObject.Horizontal;
    }
  }

  async void OnMove(InputValue inputValue)
  {
    Vector2 velocity = inputValue.Get<Vector2>();

    Patch patch = new Patch();

    patch.q = velocity.x == 0 ? null : velocity.x;
    patch.w = velocity.y == 0 ? null : velocity.y;
    patch.e = playerTransform.position.x == 0 ? null : playerTransform.position.x;
    patch.r = playerTransform.position.y == 0 ? null : playerTransform.position.y;

    SendPatch(patch);
  }

  async void SendPatch(Patch patch) {
    // We can abort the update if the WebSocket is closed 
    if (websocket.State != WebSocketState.Open)
    {
      return;
    }

    string message = JsonConvert.SerializeObject(patch, jsonFormatting, jsonSettings);
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

      //Updates the animation for Player 2
      if (velocity != Vector2.zero)
      {
        otherAnimator.SetBool("isMoving", true);
      }
      else
      {
        otherAnimator.SetBool("isMoving", false);
      }

      otherAnimator.SetFloat("Horizontal", velocity.x);
      otherAnimator.SetFloat("Vertical", velocity.y);
      otherAnimator.SetFloat("Speed", 1f);

      // Actually update the position and velocity
      otherRigidbody.velocity = velocity;
      otherTransform.position = position;
    };

    await websocket.Connect();
  }

  private async void OnApplicationQuit()
  {
    await websocket.Close();
  }
}
