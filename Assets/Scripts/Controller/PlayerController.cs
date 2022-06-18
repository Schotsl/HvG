using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Takes and handles input and movement for a player character
public class PlayerController : MonoBehaviour
{
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
        pauseController = GetComponent<PauseController>();
        joystick = FindObjectOfType<Joystick>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Globals.triggering = false;
    }

    // Update is called once per frame
    // Inputsystem actions zijn van https://www.youtube.com/watch?v=m5WsmlEOFiA&t=937s
    void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            movementInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }

        if (playerInputActions.Player.PauseKey.triggered)
        {
            pauseController.Toggle();
        }
        if (Globals.triggering)
        {
            triggerNpc.transform.GetChild(0).gameObject.SetActive(true);
            previousNpc = triggerNpc;
        }
        else
        {
            if (previousNpc != null)
            {
                previousNpc.transform.GetChild(0).gameObject.SetActive(false);
                previousNpc = null;
            }
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NPC")
        {
            Globals.triggering = true;
            triggerNpc = other.gameObject;

            animator.SetBool("isMoving", false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "NPC")
        {
            Globals.triggering = false;
            triggerNpc = null;
        }
    }
}
