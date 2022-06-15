using System.Collections;
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

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    public Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    private GameObject triggerNpc;
    // private bool triggering;
    public static bool GamePaused = false;

    public GameObject PauseUI;
    public GameObject PauseMain;
    public GameObject PauseOptions;

    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
    }

    private void OnEnable() {
        playerInputActions.Enable();
    }

    private void OnDisable() {
        playerInputActions.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GamePaused = false;
    }

    //Pause menu is van https://www.youtube.com/watch?v=JivuXdrIHK0&t=431s
    void Resume(){
        PauseMain.SetActive(true);
        PauseOptions.SetActive(false);
        GamePaused = false;
        PauseUI.SetActive(GamePaused);
    }
    void Pause(){
        GamePaused = true;
        PauseUI.SetActive(GamePaused);
    }
    // Update is called once per frame
    // Inputsystem actions zijn van https://www.youtube.com/watch?v=m5WsmlEOFiA&t=937s
    void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld) {
            movementInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }
        if(playerInputActions.Player.PauseKey.triggered){
            Debug.Log("Triggered");
            if(GamePaused){
                Resume();
                Debug.Log("Resuming game");
            } else {
                Pause();
                Debug.Log("Pausing game");
            }
        }
    }
    private void FixedUpdate() {
        if(canMove) {
            // If movement input is not 0, try to move
            if(movementInput != Vector2.zero){
                
                bool success = TryMove(movementInput);

                if(!success) {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }

                if(!success) {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
                
                animator.SetBool("isMoving", success);
            } else {
                animator.SetBool("isMoving", false);
            }

        // Set direction of sprite to movement direction
        animator.SetFloat("Horizontal", movementInput.x);
        animator.SetFloat("Vertical", movementInput.y);
        animator.SetFloat("Speed", moveSpeed);
        }
        
    }

    private bool TryMove(Vector2 direction) {
        if(direction != Vector2.zero) {
            // Check for potential collisions
            int count = rb.Cast(
                direction, // X and Y values between -1 and 1 that represent the direction from the body to look for collisions
                movementFilter, // The settings that determine where a collision can occur on such as layers to collide with
                castCollisions, // List of collisions to store the found collisions into after the Cast is finished
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // The amount to cast equal to the movement plus an offset

            if(count == 0){
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            } else {
                return false;
            }
        } else {
            // Can't move if there's no direction to move in
            return false;
        }
    }

    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();
    }
    public void LockMovement() {
        canMove = false;
    }

    public void UnlockMovement() {
        canMove = true;
    }


    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "NPC") {
            // triggering = true;
            triggerNpc = other.gameObject;

            animator.SetBool("isMoving", false);
            triggerNpc.GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "NPC") {
            // triggering = false;
            triggerNpc = null;
        }
    }
}
