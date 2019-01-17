using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class truck_bed : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    	
	}
    private void OnTriggerEnter(Collider item)
    {
        Debug.Log("calling on trigger");
        if(item.gameObject.layer == 8)
        {
            string itemName = item.gameObject.name;
            GetComponentInParent<truckController>().addItem(itemName);
            Destroy(item.gameObject);
        }
        
    }
}
