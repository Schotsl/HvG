using System;
using UnityEngine;

public class OtherController : MonoBehaviour
{
    private GameObject websocketObject;
    private WebsocketManager websocketScript;

    private Animator otherAnimator;
    private Rigidbody2D otherRigidbody;

    private Vector2 startPosition;
    private Vector2 targetPosition;

    private void Start()
    {
        websocketObject = GameObject.Find("WebsocketManager");
        websocketScript = websocketObject.GetComponent<WebsocketManager>();

        otherAnimator = GetComponent<Animator>();
        otherRigidbody = GetComponent<Rigidbody2D>();

        // Altough we're attaching this script too Player 2 I've still hardcoded the name just in case
        string name = "Player 2";

        websocketScript.AddPosition(RecievePosition, name);
    }

    private void RecievePosition(float x, float y)
    {
        targetPosition = new Vector2(x, y);
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
    }
}
