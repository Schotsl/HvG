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

  private void WebsocketMessage(byte[] bytes) {
    string message = System.Text.Encoding.UTF8.GetString(bytes);
    Patch patch = JsonConvert.DeserializeObject<Patch>(message);

    Vector2 position = new Vector2(patch.e ?? 0, patch.r ?? 0);
    Vector2 velocity = new Vector2(patch.q ?? 0, patch.w ?? 0);

    MoveOther(velocity, position);
  }

  private void MoveOther(Vector2 velocity, Vector2 position) {
    bool isMoving = velocity != Vector2.zero;

    otherAnimator.SetFloat("Horizontal", velocity.x);
    otherAnimator.SetFloat("Vertical", velocity.y);
    otherAnimator.SetFloat("Speed", 1f);
    otherAnimator.SetBool("isMoving", isMoving);

    // Actually update the position and velocity
    otherRigidbody.velocity = velocity;
    otherTransform.position = position;
  }
}
