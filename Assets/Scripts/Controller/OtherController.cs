using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;

public class OtherController : MonoBehaviour
{
  private GameObject websocketObject;
  private WebsocketManager websocketScript;

  private Animator otherAnimator;
  private Transform otherTransform;
  private Rigidbody2D otherRigidbody;

  private Vector2 futurePosition;
  private Vector2 currentPosition;

  private void Start()
  {
    websocketObject = GameObject.Find("WebsocketManager");
    websocketScript = websocketObject.GetComponent<WebsocketManager>();

    otherAnimator = GetComponent<Animator>();
    otherTransform = GetComponent<Transform>();
    otherRigidbody = GetComponent<Rigidbody2D>();

    // If the developer skipped the host screen they won't have a websocket
    if (websocketScript.websocket != null) {
      websocketScript.websocket.OnMessage += WebsocketMessage;
    };
  }

  private void FixedUpdate() {
    int speedX = 0;
    int speedY = 0;

    currentPosition = otherTransform.position;

    float diffrenceX = Math.Abs(futurePosition.x - currentPosition.x);
    float diffrenceY = Math.Abs(futurePosition.y - currentPosition.y);

    if (diffrenceX > 0.05) speedX = futurePosition.x > currentPosition.x ? 1 : -1;
    if (diffrenceY > 0.05) speedY = futurePosition.y > currentPosition.y ? 1 : -1;


    // Start the animation based on the movement of the player
    bool isMoving = speedX != 0 || speedY != 0;
    otherAnimator.SetBool("isMoving", isMoving);

    // Set the animation speed to the speed of the character
    int speedMovement = isMoving ? 1 : 0;
    otherAnimator.SetFloat("Speed", speedMovement);

    otherAnimator.SetFloat("Vertical", speedY);
    otherAnimator.SetFloat("Horizontal", speedX);

    otherRigidbody.velocity = new Vector2(speedX, speedY);
  }

  private void WebsocketMessage(byte[] bytes) {
    string message = System.Text.Encoding.UTF8.GetString(bytes);

    Patch patch = JsonConvert.DeserializeObject<Patch>(message);

    futurePosition = new Vector2(patch.x, patch.y);
  }
}
