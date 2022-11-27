using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float dashSpeed;
    public float dashSpeedChangeFactor;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    public float fireCooldown;
    public float startfireCooldown = 0.25f;
    private bool tryDash;

    [Header("Keybinds")]
    public string horizontalMovement = "Horizontal";
    public string verticalMovement = "Vertical";
    public string jumpKey = "Jump";
    public string sprintKey = "Sprint";
    public string fireKey = "Fire1";

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    //float horizontalInput;
    //float verticalInput;

    private Vector2 movementInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public float startHealth;
    public float health;
    public float startInvulTimer;
    public float invulTimer;
    public Vector3 respawnPos;
    public GameObject myDeathCanvas;
    public bool dead;

    [Header("Other Objects")]
    public GameObject myCamera;
    public Camera camObject;
    public GameObject bulletPrefab;
    private TempoObjSpawner doStuffChecker;
    private HeartBehaviour theHeart;
    public CameraCorrect camCorrect;

    public enum MovementState
    {
        walking,
        sprinting,
        dashing,
        air
    }

    public bool dashing;

    // Start is called before the first frame update
    void Start()
    {
        //Put the camera in the correct position
        camCorrect = FindObjectOfType<CameraCorrect>();
        camCorrect.PlayerJoined();
        if (camCorrect.publicIndex == 1)
            camObject.rect = new Rect(0, 0.5f, 1, 0.5f);
        else if (camCorrect.publicIndex == 2)
            camObject.rect = new Rect(0, 0, 1, 0.5f);

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        doStuffChecker = FindObjectOfType<TempoObjSpawner>();
        theHeart = FindObjectOfType<HeartBehaviour>();
        health = startHealth;
        myDeathCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        //Handle drag
        if (state == MovementState.walking || state == MovementState.sprinting || state == MovementState.dashing)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (invulTimer > 0)
            invulTimer -= Time.deltaTime;

        if (fireCooldown > 0)
            fireCooldown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EnemyBullet" && invulTimer <= 0)
        {
            health--;
            invulTimer = startInvulTimer;

            if (health == 0)
                StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        transform.position = new Vector3(1000f, 1000f, 1000f);
        myDeathCanvas.SetActive(true);
        dead = true;
        invulTimer = 5.0f;
        yield return new WaitForSeconds(3.0f);
        dead = false;
        myDeathCanvas.SetActive(false);
        health = startHealth;
        transform.position = respawnPos;
    }

    private void MyInput()
    {
        /*horizontalInput = Input.GetAxisRaw(horizontalMovement);
        verticalInput = Input.GetAxisRaw(verticalMovement);*/

        //When to jump
        /*if(Input.GetButtonDown(jumpKey) && readyToJump && grounded && doStuffChecker.doStuff == true)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        else if(Input.GetButtonDown(jumpKey) && readyToJump && grounded && doStuffChecker.doStuff != true && dead == false)
        {
            theHeart.TakeDamage(2);
        }*/

        /*//When to fire
        if(Input.GetButtonDown(fireKey) && fireCooldown <= 0 && doStuffChecker.doStuff == true && dead == false)
        {
            SpawnBullet();
            fireCooldown = startfireCooldown;
        }
        else if(Input.GetButtonDown(fireKey) && doStuffChecker.doStuff == false && dead == false)
        {
            theHeart.TakeDamage(2);
        }*/
    }

    public void FirePressed(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            //When to fire
            if (fireCooldown <= 0 && doStuffChecker.doStuff == true && dead == false)
            {
                SpawnBullet();
                fireCooldown = startfireCooldown;
            }
            else if (doStuffChecker.doStuff == false && dead == false)
            {
                theHeart.TakeDamage(2);
            }
        }
    }

    public void JumpPressed(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (readyToJump && grounded && doStuffChecker.doStuff == true)
            {
                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
            else if (readyToJump && grounded && doStuffChecker.doStuff != true && dead == false)
            {
                theHeart.TakeDamage(2);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    private void SpawnBullet()
    {
        Instantiate(bulletPrefab, myCamera.transform.position, myCamera.transform.rotation);
    }

    public void SetSprint(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            tryDash = true;
        }
        else if(ctx.canceled)
        {
            tryDash = false;
        }
    }

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;

    private void StateHandler()
    {
        //Mode - Dashing
        if(dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }

        //Mode - Sprinting
        else if(grounded && tryDash)
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        //Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        //Mode - Air
        else
        {
            state = MovementState.air;

            if (desiredMoveSpeed < sprintSpeed)
                desiredMoveSpeed = walkSpeed;
            else
                desiredMoveSpeed = sprintSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == MovementState.dashing) keepMomentum = true;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;

    }

    private float speedChangeFactor;
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;
        Debug.Log("Lerping");

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    private void MovePlayer()
    {
        //Calculate movement direction
        //moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        moveDirection = orientation.forward * movementInput.y + orientation.right * movementInput.x;

        //On slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        //On ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //In air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        //Turn 
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        //Limit speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        //Limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //Limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        //Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
