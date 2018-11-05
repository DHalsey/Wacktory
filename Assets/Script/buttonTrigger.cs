using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonTrigger : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Object Entered Button Trigger");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Object Is Within Button Trigger");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Object Exited Button Trigger");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
