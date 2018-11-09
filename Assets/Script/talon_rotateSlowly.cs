using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talon_rotateSlowly : MonoBehaviour {

    GameObject obj;

    public float speed;
    public bool flipRotation;
    public float slowAmt;

	// Use this for initialization
	void Start () {
        obj = gameObject;
        speed = 20.0f;
        slowAmt = 1.0f;
	}

    // When triggered, object will rotate slower than others
    private void OnTriggerEnter(Collider other)
    {
        slowAmt = 0.25f;
    }

    // Will run as long as object is in button trigger
    private void OnTriggerStay(Collider other)
    {
        // Particle effect will activate when object is within collider
    }

    // When exited, object will continue to rotate at normal speed
    private void OnTriggerExit(Collider other)
    {
        slowAmt = 1.0f;
    }

    // Update is called once per frame
    void Update () {
        if(!flipRotation)
        {
            // Space.Self is refering to rotating around it's own center as apposed to the Space.Wrold that would be world axis rotation
            obj.transform.Rotate(0, 0, speed * Time.deltaTime * slowAmt);
        }
        else
        {
            obj.transform.Rotate(0, 0, -speed * Time.deltaTime * slowAmt);
        }
    }
}
