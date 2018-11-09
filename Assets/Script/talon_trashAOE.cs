using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talon_trashAOE : MonoBehaviour {

    GameObject trashArea;

    bool isTriggered;
    float timeInTrigger;
    float triggerEnterStartTime;

    public float timeUntilDeath;
    public Color trashColor;

	// Use this for initialization
	void Start () {
        isTriggered = false;
        timeInTrigger = 0.0f;
        triggerEnterStartTime = 0.0f;

        trashArea = gameObject;
        trashArea.GetComponent<Renderer>().material.color = trashColor;
    }

    // object entered trash trigger
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " entered " + gameObject.name + " trigger");
        isTriggered = true;
        triggerEnterStartTime = Time.time;
    }

    // Will run as long as object is in button trigger
    private void OnTriggerStay(Collider other)
    {
        // If object has been in the trash area of effect for longer than timeUntilDeath, destroy object
        if(Time.time - triggerEnterStartTime > timeUntilDeath)
        {
            Destroy(other);
        }
    }
	
    // When triggered, button will release; reutrning to original height
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.name + " exited " + gameObject.name + " trigger");
        isTriggered = false;
        triggerEnterStartTime = 0.0f;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
