using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talon_buttonTrigger : MonoBehaviour {

    GameObject btn;
    float btnX, btnY, btnZ;
    float btnMovementAmt = 0.17f;

    // Use this for initialization
    void Start ()
    {
        btn = gameObject;

        btn.GetComponent<Renderer>().material.color = new Color(0.95f, 0.5f, 0.5f);


        btnX = btn.transform.position.x;
        btnY = btn.transform.position.y;
        btnZ = btn.transform.position.z;

        Debug.Log("btnX: " + btnX + " btnY: " + btnY + " btnZ: " + btnZ);
    }

    // When triggered, button will press in towards the floor
    private void OnTriggerEnter(Collider other)
    {
        btn.GetComponent<Renderer>().material.color = new Color(0.8f, 0.5f, 0.5f);

        Debug.Log(other.name + " entered " + gameObject.name + " trigger");

        btnY -= btnMovementAmt;
        transform.position = new Vector3(btnX, btnY, btnZ);
    }

    private void OnTriggerStay(Collider other)
    {

    }
    // When triggered, button will release; reutrning to original height
    private void OnTriggerExit(Collider other)
    {
        btn.GetComponent<Renderer>().material.color = new Color(0.95f, 0.5f, 0.5f);

        Debug.Log(other.name + " exited " + gameObject.name + " trigger");

        btnY += btnMovementAmt;
        transform.position = new Vector3(btnX, btnY, btnZ);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
