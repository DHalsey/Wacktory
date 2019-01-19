using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talon_hammerDeath : MonoBehaviour {

    public GameObject player; 
    public talon_movement talMovScriptCall;

	// Use this for initialization
	void Start () {
        talMovScriptCall = player.GetComponent<talon_movement>();
	}
	
    // When triggered, player is in hammer head and needs to die..
    private void OnTriggerEnter(Collider other)
    {
        // other is the object that had entered the trigger zone
        Debug.Log(other.name + " entered " + gameObject.name + " trigger");

        // Killing the player because they've been hit by the hammer
        talMovScriptCall.killPlayer();
    }


	// Update is called once per frame
	void Update () {
		
	}
}
