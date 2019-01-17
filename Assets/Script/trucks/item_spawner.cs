using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_spawner : MonoBehaviour {
    float time1;
    float time2;
    public GameObject item;
	// Use this for initialization
	void Start () {
        time1 = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        time2 = Time.time;
		if(time2 - time1 >= 2.0f){
            time1 = time2;
            Instantiate(item, this.transform.position, Quaternion.identity);
        }
	}
}
