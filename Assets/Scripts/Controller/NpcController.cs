using System;
using UnityEngine;

using Random = UnityEngine.Random;

public class NpcController : MonoBehaviour
{
    private GameObject websocketObject;
    private WebsocketManager websocketScript;

    private Animator otherAnimator;
    private Rigidbody2D otherRigidbody;

    private Vector2 startPosition;
    private Vector2 targetPosition;

    [Header("The max square radius a NPC can move")]
    public float movingRadius;

    [Header("Should the NPC idle randomly")]
    public bool idleRandomly;

    private void Start()
    {
        websocketObject = GameObject.Find("WebsocketManager");
        websocketScript = websocketObject.GetComponent<WebsocketManager>();

        otherAnimator = GetComponent<Animator>();
        otherRigidbody = GetComponent<Rigidbody2D>();

        startPosition = otherRigidbody.position;
        targetPosition = otherRigidbody.position;
    }

    private void RecievePosition(float x, float y)
    {
        targetPosition = new Vector2(x, y);

        Debug.Log($"<color=green>[NpcController]</color> {name} has received {x} X, {y} Y");
    }

    private void SelectPosition()
    {
        float postiveX = startPosition.x + movingRadius;
        float negativeX = startPosition.x - movingRadius;

        float postiveY = startPosition.y + movingRadius;
        float negativeY = startPosition.y - movingRadius;

        float randomY = Random.Range(negativeY, postiveY);
        float randomX = Random.Range(negativeX, postiveX);

        targetPosition = new Vector2(randomX, randomY);

        Debug.Log(
            $"<color=green>[NpcController]</color> {name} has selected {randomX} X, {randomY} Y"
        );

        if (Globals.isHosting)
        {
            // Wrap the NPC's name and position in a object
            PositionUpdate update = new PositionUpdate(name, randomX, randomY);

            // Send the NPC target to the other player
            websocketScript.SendWebsocket(update);
        }
    }

    private void FixedUpdate()
    {
        int speedX = 0;
        int speedY = 0;

        Vector2 currentPosition = otherRigidbody.position;

        float diffrenceX = Math.Abs(targetPosition.x - currentPosition.x);
        float diffrenceY = Math.Abs(targetPosition.y - currentPosition.y);

        if (diffrenceX > 0.05)
            speedX = targetPosition.x > currentPosition.x ? 1 : -1;
        if (diffrenceY > 0.05)
            speedY = targetPosition.y > currentPosition.y ? 1 : -1;

        // Start the animation based on the movement of the player
        bool isMoving = speedX != 0 || speedY != 0;
        otherAnimator.SetBool("isMoving", isMoving);

        // Set the animation speed to the speed of the character
        int speedMovement = isMoving ? 1 : 0;
        otherAnimator.SetFloat("Speed", speedMovement);

        otherAnimator.SetFloat("Vertical", speedY);
        otherAnimator.SetFloat("Horizontal", speedX);

        otherRigidbody.velocity = new Vector2(speedX, speedY);

        // If the NPC stopped moving we can select a new target
        if (otherRigidbody.velocity == Vector2.zero)
        {
            // Only the host needs to select a new target
            if (Globals.isHosting)
            {
                SelectPosition();
            }
        }
    }
}
