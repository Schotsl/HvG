using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json;

public class PlayerWatcher : MonoBehaviour
{
  private GameObject websocketObject;
  private WebsocketManager websocketScript;

  private Transform playerTransform;

  private Vector2 currentVector;
  private Vector2 previousVector;

  private void Start()
  {
    playerTransform = GetComponent<Transform>();

    websocketObject = GameObject.Find("WebsocketManager");
    websocketScript = websocketObject.GetComponent<WebsocketManager>();

    // Update the server every 100 seconds
    InvokeRepeating("CheckMovement", 0f, 0.1f);
  }

  private void CheckMovement() {
    currentVector = playerTransform.position;

    if (currentVector != previousVector) {
      Patch patch = new Patch();

      patch.x = Mathf.Round(currentVector.x * 100f) / 100f;
      patch.y = Mathf.Round(currentVector.y * 100f) / 100f;

      websocketScript.SendWebsocket(patch);
    }

    previousVector = currentVector;
  }
}
