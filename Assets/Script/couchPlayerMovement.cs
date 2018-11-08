using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class couchPlayerMovement : MonoBehaviour {
    
    public int playerNumber = 1;
    public float moveSpeed = 5.0f;
    public float turnSpeed = 4.0f;
    public float friction = 10.0f;
    public float gravity = 4.0f;
    public float jumpForce = 10.0f;
    
    private string verticalAxisName;
    private string horizontalAxisName;
    private Vector3 movementInput;
    private Rigidbody rb;
    private bool isGrounded;

    private Quaternion turnAngle = new Quaternion();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
    }
   
    private void Start()
    {
        verticalAxisName = "Vertical" + playerNumber;
        horizontalAxisName = "Horizontal" + playerNumber;
	}
	
	private void Update()
    {
        movementInput = new Vector3(Input.GetAxisRaw(horizontalAxisName), 0, Input.GetAxis(verticalAxisName));
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = true;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        TurnPlayer();
        ApplyGravity();

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
    
    // TODO: Handle jump input and call this function
    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
    }
}
