using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class couchPlayerMovement : MonoBehaviour {
    
    public int playerNumber;
    public float moveSpeed = 50.0f;
    public float turnSpeed = 0.1f;
    public float friction = 600.0f;
    public float gravity = 3.0f;
    public float jumpForce = 350.0f;
    public float timeToGetUp = 3.0f;

    private Collider coll;

    [HideInInspector] public bool explosion; // When the player is being affected by an explosion. This is accessed by the explosion.cs script

    [HideInInspector] public bool ragdolling; // When the player is ragdolling.
    
    private string verticalAxisName;   // Name of controller vertical axis for this player
    private string horizontalAxisName; // Name of controller horizontal axis for this player
    private Vector3 movementInput;     // Vector3 that holds the controller input for this player
    private string jumpButtonName;
    private Rigidbody rb;

    [HideInInspector] public bool isGrounded;           // Checks if the player is on some level surface to jump from
    [HideInInspector] public bool jumped;               // If player already jumped (is in midair)

    private IEnumerator ragdoll; // IEnumerator reference that we can use to check if the ragdoll coroutine is null

    private Quaternion turnAngle = new Quaternion(); // Angle to turn to automatically

    public PhysicMaterial physMat;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
        jumped = false;
        explosion = false;
        ragdolling = false;
    }
   
    private void Start()
    {
        // Add the player's number to get the right input from the Input Manager
        verticalAxisName = "Vertical" + playerNumber; 
        horizontalAxisName = "Horizontal" + playerNumber;
        jumpButtonName = "Jump" + playerNumber;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        coll = GetComponent<Collider>();
        physMat = coll.material;
	}
	
	private void Update()
    {
        // Store input to movementInput vector3 every frame.
        movementInput = new Vector3(Input.GetAxisRaw(horizontalAxisName), 0, Input.GetAxis(verticalAxisName));

        // If the jump button is pressed, jump.
        // Could not use GetButtonDown properly sicne for some reason it would always allow player to double jump.
        if (Input.GetAxis(jumpButtonName) > 0.0f)
        {
            Jump();
        }

        // If there was an explosion, start ragdoll coroutine (ragdoll for some amount of time)
        if (explosion)
        {
            explosion = false;
            if (ragdoll != null)
                StopCoroutine(ragdoll);

            ragdoll = Ragdoll();
            StartCoroutine(ragdoll);
        }
        
    }

    private void FixedUpdate()
    {
        // Handles movement and automatic turning.
        if (!ragdolling)
        {
            MovePlayer();
            TurnPlayer();
        }

        ApplyGravity();

        // Only apply friction if the player is grounded
        if (isGrounded)
        {
            ApplyFriction();
        }

    }

    // Thanks for this insane trigonometry, Dustin
    // ---------------------------------------------------------------------------
    float calculateAngle(float x, float y)
    {
        float angle = 0;
        if (x > 0 && y == 0)
        { //prevents divide by zero for perfect 90* angle
            angle = 90.0f;
        }
        else if (x < 0 && y == 0)
        { //prevents divide by zero for perfect -90* angle
            angle = -90.0f;
        }
        else if (x != 0 && y != 0)
        { //if the stick isnt rested, handle normal turnng
            angle = Mathf.Rad2Deg * Mathf.Atan(x / y);
            if (y < 0)
            {
                angle += 180; //handles -y rotations since tan is [-PI/2,PI/2]
            }
        }
        else if (x == 0 && y < 0)
        {
            angle = 180;
        }
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }
    // ----------------------------------------------------------------------------

    // Handle player movement
    private void MovePlayer()
    {
        // New vector3 with 0 y value since we dont care about that direction. 
        Vector3 rbVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        // If our player has not reached their max speed (moveSpeed), add movement to it.
        if ((rbVelocity + movementInput).magnitude < moveSpeed / 10 || (rbVelocity + movementInput).magnitude < rbVelocity.magnitude)
        {
            rb.AddForce(movementInput * moveSpeed);
        }
    }

    // Handle automatic turning
    private void TurnPlayer()
    {
        // If movement is being applied from the input, calculate the angle that we need to turn to.
        if (movementInput.magnitude > 0f)
        {
            float angleToTurnTo = calculateAngle(movementInput.x, movementInput.z);
            turnAngle = Quaternion.Euler(transform.rotation.x, angleToTurnTo, transform.rotation.z);
        }

        // Lerp to the turnAngle (smooth turning)
        transform.rotation = Quaternion.Lerp(transform.rotation, turnAngle, turnSpeed);
    }

    // Simply applies a Vector that is an inverse of the player's velocity * some predefined friction
    private void ApplyFriction()
    {
        rb.AddForce(-rb.velocity.normalized * friction * Time.deltaTime, ForceMode.Acceleration);
    }

    // Applies stronger gravity to make jumping / falling feel better
    private void ApplyGravity()
    {
        rb.AddForce(Vector3.down * 9.81f * gravity);
    }
    
    // Only jump if we are groudned and have not already jumped (not in midair)
    private void Jump()
    {
        if (isGrounded && !jumped)
        {
            jumped = true;
            rb.AddForce(Vector3.up * jumpForce);
        }
    }

    // Coroutine that handles ragdolling
    private IEnumerator Ragdoll()
    {
        ragdolling = true;
        coll.material.dynamicFriction = 0.2f; // Change the value of the player's friction so they don't slide around while ragdolling
        coll.material.bounciness = 0.5f;      // Add some bounciness for fun

        yield return new WaitForSeconds(timeToGetUp); // Wait specified amount of time to get up

        GetUp();
        coll.material.dynamicFriction = 0.0f; // Change friction and bounciness back to normal
        coll.material.bounciness = 0.0f;
        ragdolling = false;
    }

    // Handles resetting the player's position / rotation after ragdolling
    private void GetUp()
    {
        Quaternion resetAngle = Quaternion.identity;
        transform.rotation = Quaternion.Lerp(transform.rotation, resetAngle, turnSpeed);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Handles player death. Right now, it only disables the player's movement.
    public void Kill()
    {
        rb.constraints = RigidbodyConstraints.None;
        ragdolling = true;
        coll.material.dynamicFriction = 0.2f;
        coll.material.bounciness = 0.5f;
    }
}
