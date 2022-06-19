using System;
using UnityEngine;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    public float carSpeed;
    public Rigidbody2D carRigidbody;

    public Rigidbody2D carAnimator;

    [Header("The points that the car will move to")]
    public List<GameObject> pointsList;

    [Header("Should the car start at the first point")]
    public bool pointsStart;

    // [Header("Should the car teleport back to the first point")]
    // public bool pointsReset;

    private int pointsPosition;

    private GameObject websocketObject;
    private WebsocketManager websocketScript;

    private Vector2 startPosition;
    private Vector2 targetPosition;

    private void Start()
    {
        websocketObject = GameObject.Find("WebsocketManager");
        websocketScript = websocketObject.GetComponent<WebsocketManager>();

        if (pointsStart)
        {
            // We can skip too the second point since we've already teleported there
            pointsPosition = 1;

            // Teleport the car too the first point
            carRigidbody.position = pointsList[0].transform.position;
        }

        startPosition = carRigidbody.position;
        targetPosition = carRigidbody.position;
    }

    private void RecievePosition(float x, float y)
    {
        targetPosition = new Vector2(x, y);

        Debug.Log($"<color=purple>[CarController]</color> {name} has received {x} X, {y} Y");
    }

    private void SelectPosition()
    {
        if (pointsPosition >= pointsList.Count) {
            pointsPosition = 0;

            // if (pointsReset) {
            //     // We can skip too the second point since we've already teleported there
            //     pointsPosition = 1;
            // 
            //     // Teleport the car too the first point
            //     carRigidbody.position = pointsList[0].transform.position;
            // } else {
            //     // I've were not teleporting back too the first point we'll set it as our target
            //     pointsPosition = 0;
            // }
        }

        GameObject pointsTarget = pointsList[pointsPosition];

        targetPosition = pointsTarget.transform.position;
        pointsPosition += 1;

        Debug.Log(
            $"<color=purple>[CarController]</color> {name} has selected {targetPosition.x} X, {targetPosition.y} Y"
        );

        if (Globals.isHosting)
        {
            // Wrap the NPC's name and position in a object
            PositionUpdate update = new PositionUpdate(name, targetPosition.x, targetPosition.y);

            // Send the NPC target to the other player
            websocketScript.SendWebsocket(update);
        }
    }

    private void FixedUpdate()
    {
        float speedX = 0;
        float speedY = 0;

        Vector2 currentPosition = carRigidbody.position;

        float diffrenceX = Math.Abs(targetPosition.x - currentPosition.x);
        float diffrenceY = Math.Abs(targetPosition.y - currentPosition.y);

        if (diffrenceX > 0.05)
            speedX = targetPosition.x > currentPosition.x ? carSpeed : -carSpeed;
        if (diffrenceY > 0.05)
            speedY = targetPosition.y > currentPosition.y ? carSpeed : -carSpeed;

        carRigidbody.velocity = new Vector2(speedX, speedY);

        // If the NPC stopped moving we can select a new target
        if (carRigidbody.velocity == Vector2.zero)
        {
        if (pointsList.Count > 0)
        {
            // Only the host needs to select a new target
            if (Globals.isHosting)
            {
                SelectPosition();
            }
            else
            {
                websocketScript.AddPosition(RecievePosition, name);
            }
        }
        }
    }
}
