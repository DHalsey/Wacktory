using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combinerCombine : MonoBehaviour {
    private List<GameObject> objectList = new List<GameObject>();
    public bool combineDEBUG;
    public GameObject itemToSpawnDEBUG;
    private Transform spawnLocation;

    private bool isProcessing = false;
    private float timeToCombine = 3.0f; // in seconds
    private float currentTime = 0.0f;
	// Use this for initialization
	void Start () {
        spawnLocation = transform.Find("combiner_SpawnLocation");
	}
	
	// Update is called once per frame
	void Update () {
        DrawDebug();
        if (isProcessing) Process();
	}

    //handles debugging processes for testing
    //nothing in this function should make it to a live build
    private void DrawDebug() {
        if (combineDEBUG) isProcessing = true;

        //start combining if 2 or more object are in it
        if (objectList.Count >= 2) {
            isProcessing = true;
        }
        combineDEBUG = false;
    }

    //bakes the object over a desired duration
    private void Process() {
        currentTime += Time.deltaTime;

        if(currentTime >= timeToCombine) {
            Combine();
            currentTime = 0;
            isProcessing = false;
        }
    }

    //spits out the combined object when processing is complete
    private void Combine() {
        for (int i = objectList.Count-1; i>=0; i--) {
            GameObject objToRemove = objectList[i];
            objectList.RemoveAt(i);
            Destroy(objToRemove);     
        }
        GameObject testCombineObject = Instantiate(itemToSpawnDEBUG, spawnLocation);
        testCombineObject.transform.parent = null;
        testCombineObject.GetComponent<Rigidbody>().AddForce(transform.rotation*Vector3.forward*5, ForceMode.VelocityChange);

    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Grabbable")) {
            objectList.Add(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other) {
        
    }
    private void OnTriggerExit(Collider other) {
        if (!other.gameObject) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Grabbable")) {
            if (objectList.IndexOf(other.gameObject) >= 0) {
                objectList.Remove(other.gameObject);
            }
            
        }

    }
}
