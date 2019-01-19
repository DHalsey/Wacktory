using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talon_xrayMachine : MonoBehaviour {

    public GameObject scanObject;

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        // If the object is not an object the xray is looking for, ignore that object
        /* if(other.name == scanObject.name) */ {
            other.gameObject.GetComponent<talon_boxContents>().showContents();
        }

    }

    // Will run as long as object is in button trigger
    private void OnTriggerStay(Collider other)
    {
        // If the object is not an object the xray is looking for, ignore that object
        /* if (other.name == scanObject.name) */
        {
            other.gameObject.GetComponent<talon_boxContents>().updateContentsPos(other.transform.position);
        }
    }

    // When triggered, button will release; reutrning to original height
    private void OnTriggerExit(Collider other)
    {
        // If the object is not an object the xray is looking for, ignore that object
        /* if (other.name == scanObject.name) */
        {
            other.gameObject.GetComponent<talon_boxContents>().hideContents();
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
