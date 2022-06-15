using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;

public class PlayerWatcher : MonoBehaviour
{
  private GameObject websocketObject;
  private WebsocketWatcher websocketScript;

  private Joystick joystickObject;
  private float joystickVertical;
  private float joystickHorizontal;

  private Transform playerTransform;

  private Animator otherAnimator;
  private Transform otherTransform;
  private Rigidbody2D otherRigidbody;

  private void Start()
  {
    websocketObject = GameObject.Find("WebsocketWatcher");
    websocketScript = websocketObject.GetComponent<WebsocketWatcher>();

    playerTransform = GetComponent<Transform>();

    joystickObject = FindObjectOfType<Joystick>();
    joystickVertical = joystickObject.Vertical;
    joystickHorizontal = joystickObject.Horizontal;

    GameObject otherObject = GameObject.Find("Player 2");

    otherAnimator = otherObject.GetComponent<Animator>();
    otherTransform = otherObject.GetComponent<Transform>();
    otherRigidbody = otherObject.GetComponent<Rigidbody2D>();

    // Update the server every 100 seconds
    if (SystemInfo.deviceType == DeviceType.Handheld) {
      InvokeRepeating("OnJoystick", 0f, 0.1f);
    }

    // If the developer skipped the host screen they won't have a websocket
    if (websocketScript.websocket != null) {
      // Listen for messages from the server
      websocketScript.websocket.OnMessage += (bytes) =>
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
    };
  }

  private void OnJoystick() {
    if (joystickObject.Horizontal != joystickHorizontal || joystickObject.Vertical != joystickVertical) {
      Patch patch = new Patch();

      patch.w = joystickObject.Vertical == 0 ? null : joystickObject.Vertical;
      patch.q = joystickObject.Horizontal == 0 ? null : joystickObject.Horizontal;

      patch.e = playerTransform.position.x == 0 ? null : playerTransform.position.x;
      patch.r = playerTransform.position.y == 0 ? null : playerTransform.position.y;

      websocketScript.SendWebsocket(patch);

      joystickVertical = joystickObject.Vertical;
      joystickHorizontal = joystickObject.Horizontal;
    }
  }

  private void OnMove(InputValue inputValue)
  {
    Vector2 velocity = inputValue.Get<Vector2>();

    Patch patch = new Patch();

    patch.q = velocity.x == 0 ? null : velocity.x;
    patch.w = velocity.y == 0 ? null : velocity.y;
    patch.e = playerTransform.position.x == 0 ? null : playerTransform.position.x;
    patch.r = playerTransform.position.y == 0 ? null : playerTransform.position.y;

    websocketScript.SendWebsocket(patch);
  }
}
