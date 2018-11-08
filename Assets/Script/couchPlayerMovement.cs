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

    private void Awake()
    {
        player = transform.parent.gameObject;
        rbPlayer = player.GetComponent<Rigidbody>();
        rbRig = GetComponent<Rigidbody>();
        
    }

    // Use this for initialization
    private void Start()
    {
        verticalAxisName = "Vertical" + playerNumber;
        horizontalAxisName = "Horizontal" + playerNumber;
	}
	
	// Update is called once per frame
	private void Update()
    {
        movementInput = new Vector3(Input.GetAxis(horizontalAxisName), 0, Input.GetAxis(verticalAxisName)) * moveSpeed;
    }

    private void FixedUpdate()
    {
        MoveRig();
        MovePlayer();
        RotatePlayer();
    }

    private void MoveRig()
    {
        rbRig.MovePosition(rbRig.position + Vector3.ClampMagnitude(movementInput, moveSpeed) * Time.deltaTime);
    }

    private void MovePlayer()
    {
        rbPlayer.MovePosition(Vector3.SmoothDamp(rbPlayer.position, rbRig.position, ref playerVelocity, playerMoveTime));
    }

    private void RotatePlayer()
    {
        Vector3 targetDirection = new Vector3(rbRig.position.x - rbPlayer.position.x, 0f, rbRig.position.z - rbPlayer.position.z);

        float step = turnSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(player.transform.forward, targetDirection, step, 0f);

        rbPlayer.rotation = Quaternion.LookRotation(newDirection);
    }
   
}
