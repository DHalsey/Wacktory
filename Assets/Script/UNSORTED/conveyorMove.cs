//Dustin Halsey - dhalsey
//conveyorMove.cs
//Functionality: 
//  moves objects with rigidbodies that come into contact with the conveyor belt's hitbox
//
//How to Attach:
//  Attach this to the object that has the collider that you would like to move the objects
//  For most cases, this object will be a child of the actual conveyor belt model.
//    This is because we want to use a separate hitbox from the hitbox that allows standard rigidbody collisions of the model
//  Public Variables:
//      forceDirection - should be assigned to the direction that you want to push the object
//      force - the speed at which the object will be moved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conveyorMove : MonoBehaviour {
    // Use this for initialization
    private Collider movementCollider; //the collider that handles the movement of the objects on the conveyor belt
    public GameObject conveyorObj; //the object that actually displays the conveyor belt model
    public Vector3 forceDirection = new Vector3(1,0,0); //the direction of the force from the conveyor belt (WILL BE NORMALIZED)
    private List<Rigidbody> rbList= new List<Rigidbody>(); //a list of all rigidbopdies that are in contact with the conveyorbelt
    private ParticleSystem psBreak;
    public Color colorRepaired;
    public Color colorBroken;

    [HideInInspector]
    public bool isBroken = false; //tells us if they conveyor belt is currently broken or not
    public float force = 1; //the power of the force in the forceDirection
	void Start () {
        forceDirection = forceDirection.normalized; //normalizes the forceDirection to make sure force is always constant
        movementCollider = GetComponent<Collider>();
        psBreak = GetComponent<ParticleSystem>();
        conveyorObj.GetComponent<Renderer>().material.SetColor("_Color", colorRepaired);
    }

    //add the collided object to a list to move if it has a rigidbody
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<Rigidbody>()) { 
            rbList.Add(other.gameObject.GetComponent<Rigidbody>());
        }
    }

    //move all objects that are in rbList
    private void OnTriggerStay(Collider other) {
        if(isBroken) return; //stop moving things while it is broken
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

    //remove the rigidbody from the list when it leaves the conveyor's collider
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponent<Rigidbody>()) {
            rbList.Remove(other.gameObject.GetComponent<Rigidbody>());
        }
    }

    public bool Break() {
        psBreak.Play();
        conveyorObj.GetComponent<Renderer>().material.SetColor("_Color", colorBroken);
        isBroken = true;
        return true;
    }

    public bool Repair() {
        psBreak.Stop();
        psBreak.Clear();
        conveyorObj.GetComponent<Renderer>().material.SetColor("_Color", colorRepaired);
        isBroken = false;
        return true;
    }

}
