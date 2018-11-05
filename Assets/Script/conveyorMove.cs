//Primary Author of Script: Dustin Halsey - dhalsey

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conveyorMove : MonoBehaviour {
    // Use this for initialization
    public Collider movementCollider; //the collider that handles the movement of the objects on the conveyor belt
    public Vector3 forceDirection = new Vector3(1,0,0); //the direction of the force from the conveyor belt (WILL BE NORMALIZED)
    private List<Rigidbody> rbList= new List<Rigidbody>();
    public float force = 1;
	void Start () {
        forceDirection = forceDirection.normalized; //normalizes the forceDirection to make sure force is always constant
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<Rigidbody>()) {
            rbList.Add(collision.gameObject.GetComponent<Rigidbody>());
        }
        
    }
    private void OnCollisionStay(Collision collision) {
        foreach (Rigidbody rb in rbList) {
            rb.MovePosition(rb.gameObject.transform.position + forceDirection*force*Time.deltaTime);
        }
    }
    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.GetComponent<Rigidbody>()) {
            rbList.Remove(collision.gameObject.GetComponent<Rigidbody>());
        }       
    }
}
