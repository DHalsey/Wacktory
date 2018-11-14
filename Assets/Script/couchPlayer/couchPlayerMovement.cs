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
    public PhysicMaterial physMat;

    [HideInInspector] public bool explosion;

    [HideInInspector] public bool ragdolling;
    
    private string verticalAxisName;
    private string horizontalAxisName;
    private Vector3 movementInput;
    private string jumpButtonName;
    private Rigidbody rb;
    private bool isGrounded;
    private bool jumped;

    private IEnumerator ragdoll;

    private Quaternion turnAngle = new Quaternion();

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
        verticalAxisName = "Vertical" + playerNumber;
        horizontalAxisName = "Horizontal" + playerNumber;
        jumpButtonName = "Jump" + playerNumber;
        coll = GetComponent<Collider>();
        physMat = coll.material;
	}
	
	private void Update()
    {
        movementInput = new Vector3(Input.GetAxisRaw(horizontalAxisName), 0, Input.GetAxis(verticalAxisName));
        if (Input.GetAxis(jumpButtonName) > 0.0f)
        {
            Jump();
        }

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
        if (!ragdolling)
        {
            MovePlayer();
            TurnPlayer();
        }

        ApplyGravity();

        if (isGrounded)
        {
            ApplyFriction();
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            isGrounded = true;
            jumped = false;
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

    private void MovePlayer()
    {
        Vector3 rbVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if ((rbVelocity + movementInput).magnitude < moveSpeed / 10 || (rbVelocity + movementInput).magnitude < rbVelocity.magnitude)
        {
            rb.AddForce(movementInput * moveSpeed);
        }
    }

    private void TurnPlayer()
    {
        if (movementInput.magnitude > 0f)
        {
            float angleToTurnTo = calculateAngle(movementInput.x, movementInput.z);
            turnAngle = Quaternion.Euler(transform.rotation.x, angleToTurnTo, transform.rotation.z);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, turnAngle, turnSpeed);
    }

    private void ApplyFriction()
    {
        rb.AddForce(-rb.velocity.normalized * friction * Time.deltaTime, ForceMode.Acceleration);
    }

    private void ApplyGravity()
    {
        rb.AddForce(Vector3.down * 9.81f * gravity);
    }
    
    private void Jump()
    {
        if (isGrounded && !jumped)
        {
            jumped = true;
            rb.AddForce(Vector3.up * jumpForce);
        }
    }

    private IEnumerator Ragdoll()
    {
        ragdolling = true;
        coll.material.dynamicFriction = 0.2f;
        coll.material.bounciness = 0.5f;

        yield return new WaitForSeconds(timeToGetUp);

        GetUp();
        coll.material.dynamicFriction = 0.0f;
        coll.material.bounciness = 0.0f;
        ragdolling = false;
    }

    private void GetUp()
    {
        Quaternion resetAngle = Quaternion.identity;
        transform.rotation = Quaternion.Lerp(transform.rotation, resetAngle, turnSpeed);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
