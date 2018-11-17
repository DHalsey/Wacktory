using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxSpawner : MonoBehaviour {
    public GameObject cardboardBox;
    private float spawnDelay = 1.5f; //time in seconds between box spawns
    private float currentTime = 0; //current time from last box spawn in seconds
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        currentTime += Time.deltaTime;
        if (currentTime >= spawnDelay) {
            GameObject boxClone;
            boxClone = Instantiate(cardboardBox, transform.position, transform.rotation);
            Destroy(boxClone, 30); //debug to destroy boxes after 30s to avoid a pileup
            currentTime = 0;
        }
	}
}
