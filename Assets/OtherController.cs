using UnityEngine;
using NativeWebSocket;

public class OtherController : MonoBehaviour
{
  WebSocket websocket;

  public float moveSpeed = 1f;

  public Animator animator;

  void Start()
  {
    StartWebsocket();
    animator = GetComponent<Animator>();
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
        Vector2 position = new Vector2(patch.x_pos, patch.y_pos);
        Vector2 velocity = new Vector2(patch.x_speed, patch.y_speed);

        // Get the components
        Transform transform = gameObject.GetComponent<Transform>();
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();

        // Actually update the position and velocity
        rigidbody.velocity = velocity;
        transform.position = position;

        //Updates the animation for player 2
        if(velocity != Vector2.zero){
          
          animator.SetBool("isMoving", true);
        } else {
          animator.SetBool("isMoving", false);
        }
          
          animator.SetFloat("Horizontal", velocity.x);
          animator.SetFloat("Vertical", velocity.y);
          animator.SetFloat("Speed", moveSpeed);

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