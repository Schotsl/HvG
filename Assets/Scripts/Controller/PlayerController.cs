using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Takes and handles input and movement for a player character
public class PlayerController : MonoBehaviour
{
    private GameObject websocketObject;
    private WebsocketManager websocketScript;

    private int damageCount;
    private bool damageTaking;

    public GameObject damageScreen;
    public GameObject endingVehicle;
    public List<GameObject> otherVehicles;

    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    protected Joystick joystick;

    PauseController pauseController;
    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    public Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private GameObject triggerNpc;
    private GameObject previousNpc;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        websocketObject = GameObject.Find("WebsocketManager");
        websocketScript = websocketObject.GetComponent<WebsocketManager>();

        websocketScript.AddHealth(RecieveHealth);

        pauseController = GetComponent<PauseController>();
        joystick = FindObjectOfType<Joystick>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    // Inputsystem actions zijn van https://www.youtube.com/watch?v=m5WsmlEOFiA&t=937s
    void Update()
    {
        Image image = damageScreen.GetComponent<Image>();
        Color color = image.color;

        // Slowly turn down the red overlay
        if (color.a > 0) {
            color.a -= 0.004f;
            image.color = color;
        }

        if (moveSpeed < 1) {
            moveSpeed += 0.002f;
        }

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            float ySpeed = (float)joystick.Vertical;
            float xSpeed = (float)joystick.Horizontal;

            if (ySpeed > 0) ySpeed += .15f; 
            if (ySpeed < 0) ySpeed -= .15f; 

            if (xSpeed > 0) xSpeed += .15f; 
            if (xSpeed < 0) xSpeed -= .15f; 

            xSpeed = Mathf.Round(xSpeed);
            ySpeed = Mathf.Round(ySpeed);

            movementInput = new Vector2(xSpeed, ySpeed);
        }

        if (playerInputActions.Player.PauseKey.triggered)
        {
            pauseController.Toggle();
        }
    }

    private void FixedUpdate()
    {
        bool isMoving = false;

        if (!Globals.isPaused && !Globals.isDialoguing && !Globals.isInMap) {
            
            // If movement input is not 0, try to move
            if (movementInput != Vector2.zero)
            {
                isMoving = TryMove(movementInput);
            }
        }

        // Set direction of sprite to movement direction
        animator.SetFloat("Speed", moveSpeed);

        animator.SetFloat("Vertical", movementInput.y);
        animator.SetFloat("Horizontal", movementInput.x);

        animator.SetBool("isMoving", isMoving);
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Check for potential collisions
            int count = rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                moveSpeed * Time.fixedDeltaTime + collisionOffset
            ); // The amount to cast equal to the movement plus an offset

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            // Can't move if there's no direction to move in
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Ending") {
            // Set the evil car on the run
            endingVehicle.SetActive(true);

            // Disable al other cars to prevent collections
            otherVehicles.ForEach((otherVehicle) => {
                otherVehicle.SetActive(false);
            });

        } else if (target.tag == "Overing") {
            SceneManager.LoadScene(sceneName:"SceneWin");
        } else if (target.tag == "Laser") 
        {
            if (!damageTaking) {
                Image image = damageScreen.GetComponent<Image>();
                Color color = image.color;

                damageCount ++;
                damageTaking = true;

                moveSpeed = 0f;

                // Turn the screen increasingly red
                color.a = damageCount * 0.5f;
                image.color = color;

                HealthUpdate update = new HealthUpdate(damageCount);
                websocketScript.SendWebsocket(update);

                if (damageCount > 2) {
                    SceneManager.LoadScene(sceneName:"SceneOver");
                }
            }
        } 
    }

    void RecieveHealth(int damageHealth) {
        Image image = damageScreen.GetComponent<Image>();
        Color color = image.color;

        // Turn the screen increasingly red
        color.a = damageHealth * 0.5f;
        image.color = color;

        if (damageHealth > 2) {
            SceneManager.LoadScene(sceneName:"SceneOver");
        }
    }

    void OnTriggerExit2D(Collider2D target)
    {
        if (target.tag == "Laser")
        {
            damageTaking = false;
        }
    }
}
