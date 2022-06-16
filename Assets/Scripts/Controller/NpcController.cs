using UnityEngine;
using System.Collections;

public class NpcController : MonoBehaviour
{
    private GameObject websocketObject;
    private WebsocketManager websocketScript;

    private Animator otherAnimator;
    private Rigidbody2D otherRigidbody;

    private void Start()
    {
        otherAnimator = GetComponent<Animator>();
        otherRigidbody = GetComponent<Rigidbody2D>();

        StartCoroutine(StartCycle());
    }

    private IEnumerator StartCycle() {
        float roamingTime = Random.Range(1, 3);
        float waitingTime = Random.Range(5, 10);

        StartRoaming();

        yield return new WaitForSeconds(roamingTime);

        StopRoaming();

        yield return new WaitForSeconds(waitingTime);

        StartCoroutine(StartCycle());
    }

    private void StartRoaming() {
        float xVelocity = Random.Range(.2f, .4f);
        float yVelocity = Random.Range(.2f, .4f);
        
        int xInvert = Random.Range(0, 3);
        int yInvert = Random.Range(0, 3);

        if (xInvert == 1) xVelocity = -xVelocity;
        if (yInvert == 1) yVelocity = -yVelocity;

        if (xInvert == 2) xVelocity = 0;
        if (yInvert == 2) yVelocity = 0;
        
        Vector2 velocity = new Vector2(
            xVelocity,
            yVelocity
        );

        UpdateMovement(velocity);
    }

    private void StopRoaming() {
        Vector2 velocity = new Vector2(0, 0);

        UpdateMovement(velocity);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("NPC collided with " + collision.name);

        Vector2 velocity = new Vector2(0, 0);

        UpdateMovement(velocity);
    }

    private void UpdateMovement(Vector2 velocity) {
        bool isMoving = velocity != Vector2.zero;
        
        otherAnimator.SetFloat("Speed", velocity.magnitude);

        otherAnimator.SetFloat("Vertical", velocity.y);
        otherAnimator.SetFloat("Horizontal", velocity.x);

        otherAnimator.SetBool("isMoving", isMoving);

        otherRigidbody.velocity = velocity;
    }
}
