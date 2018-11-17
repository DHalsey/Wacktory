using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openBox : MonoBehaviour {
    private talon_boxContents contentsScript;
    private GameObject containedObject;
    private GameObject itemToSpawn;
    public GameObject bombObj;
    public GameObject chickenObj;
    public GameObject walnutObj;
	// Use this for initialization
	void Start () {
        contentsScript = GetComponent<talon_boxContents>();
        if (!contentsScript) { //error to catch changes to the buttonTrigger script
            Debug.LogError("Error!: openBox did not find the contentsScript");
        }
        containedObject = contentsScript.boxContains;
        if (!contentsScript) { //error to catch changes to the buttonTrigger script
            Debug.LogError("Error!: openBox did not find the contained object in contentsScript");
        }

        if (containedObject.name == "BoxItem_bomb") {
            itemToSpawn = bombObj;
        } else if (containedObject.name == "BoxItem_chicken") {
            itemToSpawn = chickenObj;
        } else if (containedObject.name == "BoxItem_walnut") {
            itemToSpawn = walnutObj;
        }
    }
	
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "opensBox") {
            Debug.Log("Opened Box with Trigger");
            if (itemToSpawn) {
                Instantiate(itemToSpawn.transform, transform.position+(Vector3.up*0.2f), transform.rotation);
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "opensBox") {
            Debug.Log("Opened Box with Collision");
            
        }
    }
}
