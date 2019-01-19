using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxerSuction : MonoBehaviour {
    public Vector3 forceDirection;
    public float forcePower;
	// Use this for initialization
	void Start () {
        forceDirection = forceDirection.normalized;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.tag == "grabbable") {
            if (other.gameObject.GetComponent<Rigidbody>()) {
                other.gameObject.GetComponent<Rigidbody>().AddForce(
                    forceDirection * Time.deltaTime * forcePower,
                    ForceMode.Force);
            }
        }
    }
}
