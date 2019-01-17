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
    
    // Left stick input
    public string verticalMoveAxisName;
    public string horizontalMoveAxisName;
    private Vector3 movementInput; // Vector3 that holds the left stick input for this player

    // Right stick input
    public string verticalTurnAxisName;
    public string horizontalTurnAxisName;
    private Vector3 turnInput; // Vector3 that holds the right stick input for this player

    public string jumpButtonName;
    private Rigidbody rb;

    [HideInInspector] public bool isGrounded;  // Checks if the player is on some level surface to jump from
    [HideInInspector] public bool jumped;      // If player already jumped (is in midair)

    private IEnumerator ragdoll; // IEnumerator reference that we can use to check if the ragdoll coroutine is null

    private Quaternion turnAngle = new Quaternion(); // Angle to turn to automatically

    public PhysicMaterial physMat;

    private void Awake()
    {
        if(gameObject.GetComponentInChildren< couchPlayerInteract >() == null) { Debug.LogError("Cannot Access 'Couch Player Interact' script - It should be a script in CouchPlayerPIckupCollider"); } // Makes sure the code is there since it is used for interacting
        //if (gameObject.GetComponent<couchPlayerInteract>() == null) { Debug.LogError("Cannot Access 'Couch Player Interact' script - It should be a script in CouchPlayerPIckupCollider"); } // Makes sure the code is there since it is used for interacting
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
        jumped = false;
        explosion = false;
        ragdolling = false;
    }
   
    private void Start()
    {
        //gameObject.GetComponent<couchPlayerInteract>().interact
        // need to change these to saved strings

        // Add the player's number to get the right input from the Input Manager
        verticalMoveAxisName = "VerticalMove" + playerNumber; 
        horizontalMoveAxisName = "HorizontalMove" + playerNumber;

        verticalTurnAxisName = "VerticalTurn" + playerNumber;
        horizontalTurnAxisName = "HorizontalTurn" + playerNumber;

        jumpButtonName = "Jump" + playerNumber;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        coll = GetComponent<Collider>();
        physMat = coll.material;
	}
	
	private void Update()
    {
        // Store input to movementInput vector3 every frame.
        movementInput = new Vector3(Input.GetAxis(horizontalMoveAxisName), 0f, Input.GetAxis(verticalMoveAxisName));
        turnInput = new Vector3(Input.GetAxis(horizontalTurnAxisName), 0f, Input.GetAxis(verticalTurnAxisName));

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
        if (!ragdolling || gameObject.GetComponent<couchPlayerInteract>().interact);
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

    // Fucntion that finds the turn angle for the player based on their inputs 
    // From user "YoungDeveloper" in:
    // https://answers.unity.com/questions/1032673/how-to-get-0-360-degree-from-two-points.html
    // ----------------------------------------------------------------------------------------
    float calculateAngle(float x, float y)
    {
        float angle = (Mathf.Atan2(x, y) / Mathf.PI) * 180f;
        if (angle < 0)
        {
            angle += 360;
        }
        return angle;
    }
    // ---------------------------------------------------------------------------------------

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
        //Debug.Log("Input X: " + movementInput.x);
        //Debug.Log("Input Z: " + movementInput.z);
        float angleToTurnTo;

        // If movement is being applied from the input, calculate the angle that we need to turn to.
        if (turnInput.magnitude > 0f)
        {
            angleToTurnTo = calculateAngle(turnInput.x, turnInput.z);
            turnAngle = Quaternion.Euler(transform.rotation.x, angleToTurnTo, transform.rotation.z);
        }
        else if (movementInput.magnitude > 0f)
        {
            angleToTurnTo = calculateAngle(movementInput.x, movementInput.z);
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
    
    // Only jump if we are grounded and have not already jumped (not in midair)
    private void Jump()
    {
        if (isGrounded && !jumped && !gameObject.GetComponent<couchPlayerInteract>().interact)
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
