using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;

public class PlayerWatcher : MonoBehaviour
{
  private GameObject websocketObject;
  private WebsocketManager websocketScript;

  private Joystick joystickObject;
  private float joystickVertical;
  private float joystickHorizontal;
  private bool wasAllowed;

  private Transform playerTransform;

  private void Start()
  {
    websocketObject = GameObject.Find("WebsocketManager");
    websocketScript = websocketObject.GetComponent<WebsocketManager>();

    playerTransform = GetComponent<Transform>();

    joystickObject = FindObjectOfType<Joystick>();
    joystickVertical = joystickObject.Vertical;
    joystickHorizontal = joystickObject.Horizontal;

    // Update the server every 100 seconds
    if (SystemInfo.deviceType == DeviceType.Handheld) {
      InvokeRepeating("OnJoystick", 0f, 0.1f);
    };
  }

  private void OnJoystick() {
    bool isAllowed = !Globals.isDialoguing && !Globals.isPaused;
    bool isChanging = joystickObject.Horizontal != joystickHorizontal || joystickObject.Vertical != joystickVertical;

    if (isAllowed && isChanging) {
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
    bool isAllowed = !Globals.isDialoguing && !Globals.isPaused;
    bool isChanged = isAllowed != wasAllowed;

    if (isAllowed || isChanged) { 
      Patch patch = new Patch();

      Vector2 velocity = !isAllowed && isChanged ? Vector2.zero : inputValue.Get<Vector2>();

      // patch.q = velocity.x == 0 ? null : velocity.x;
      // patch.w = velocity.y == 0 ? null : velocity.y;
      // patch.e = playerTransform.position.x == 0 ? null : playerTransform.position.x;
      // patch.r = playerTransform.position.y == 0 ? null : playerTransform.position.y;

      patch.q = velocity.x;
      patch.w = velocity.y;
      patch.e = playerTransform.position.x;
      patch.r = playerTransform.position.y;

      websocketScript.SendWebsocket(patch);

      wasAllowed = isAllowed;
    }
  }
}
