using UnityEngine;

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

    private void CheckMovement()
    {
        currentVector = playerTransform.position;

        if (currentVector != previousVector)
        {
            string name = "Player 2";
            float x = currentVector.x;
            float y = currentVector.y;

            PositionUpdate update = new PositionUpdate(name, x, y);

            websocketScript.SendWebsocket(update);
        }

        previousVector = currentVector;
    }
}
