using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forkTriggerCheck : MonoBehaviour {

    public bool holdingPallet;
    public float forkHeightCheck;

	// Use this for initialization
	void Start () {
        // forkHeightCheck = GameObject.Find("Forklift").GetComponent<forkliftMovement>().currentForkHeight;
	}

    public void OnTriggerEnter(Collider other) {
        Debug.Log("Entering: " + other.tag);
    }

    private void OnTriggerStay(Collider other) {

    }

    private void OnTriggerExit(Collider other) {
        Debug.Log("Exiting: " + other.tag);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
