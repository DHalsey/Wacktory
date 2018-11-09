//Primary Author of Script: Dustin Halsey - dhalsey

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conveyorMove : MonoBehaviour {
    // Use this for initialization
    private Collider movementCollider; //the collider that handles the movement of the objects on the conveyor belt
    public Vector3 forceDirection = new Vector3(1,0,0); //the direction of the force from the conveyor belt (WILL BE NORMALIZED)
    private List<Rigidbody> rbList= new List<Rigidbody>();
    public float force = 1;
	void Start () {
        forceDirection = forceDirection.normalized; //normalizes the forceDirection to make sure force is always constant
        movementCollider = GetComponent<Collider>();
        //Time.timeScale = 0.5f;
    }
	
	// Update is called once per frame
	void Update () {
        
	}
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<Rigidbody>()) { //if it has a rigidbody, grab it
            rbList.Add(other.gameObject.GetComponent<Rigidbody>());
        }
    }
    private void OnTriggerStay(Collider other) {
        foreach (Rigidbody rb in rbList) {
            if (rb && rb.gameObject && rb.gameObject.transform) {
                if (!rb.isKinematic) { //move only if the object is not picked up
                    rb.MovePosition(rb.gameObject.transform.position + forceDirection * force * Time.deltaTime);
                }            
            } else { //removes it if the gameobject was deleted elsewhere
                rbList.Remove(rb);
            }
            
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponent<Rigidbody>()) {
            rbList.Remove(other.gameObject.GetComponent<Rigidbody>());
        }
    }
}
