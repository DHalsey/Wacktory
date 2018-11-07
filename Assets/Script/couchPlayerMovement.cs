using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class couchPlayerMovement : MonoBehaviour {

    public int playerNumber = 1;
    public float moveSpeed = 5.0f;
    public float turnSpeed = 4.0f;

    public float playerMoveTime = 0.3f;

    private GameObject player;

    private string verticalAxisName;
    private string horizontalAxisName;
    private Vector3 movementInput;
    private Rigidbody rbRig;
    private Rigidbody rbPlayer;

    private Vector3 playerVelocity;

    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        player = transform.parent.gameObject;
        rbPlayer = player.GetComponent<Rigidbody>();
        rbRig = GetComponent<Rigidbody>();
        
    }

    // Use this for initialization
    void Start () {
        verticalAxisName = "Vertical" + playerNumber;
        horizontalAxisName = "Horizontal" + playerNumber;
	}
	
	// Update is called once per frame
	void Update () {
        movementInput = new Vector3(Input.GetAxis(horizontalAxisName), 0, Input.GetAxis(verticalAxisName)) * moveSpeed;
    }

    private void FixedUpdate()
    {
        MoveRig();
        MovePlayer();
    }

    private void MoveRig()
    {
        rbRig.MovePosition(rbRig.position + Vector3.ClampMagnitude(movementInput, moveSpeed) * Time.deltaTime);
    }

    private void MovePlayer()
    {
        rbPlayer.MovePosition(Vector3.SmoothDamp(rbPlayer.position, rbRig.position, ref playerVelocity, playerMoveTime));
    }

    private void Turn()
    {
        //Vector3 moveDirection = rb.velocity.normalized;
        //float angle = Vector3.Angle(rb.transform.position, moveDirection);
        //Vector3 cross = Vector3.Cross(rb.transform.position, moveDirection);

        //if (cross.z > 0)
        //{
        //    angle = 360 - angle;
        //}

        //Quaternion quat = Quaternion.Euler(0f, 0f, angle);

        //rb.MoveRotation(quat);

        //transform.forward = Vector3.RotateTowards(transform.forward, transform.position + rotateVelocity, turnSpeed * Time.deltaTime, 0f);
        
    }
   
}
