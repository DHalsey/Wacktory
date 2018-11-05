using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talon_movement : MonoBehaviour {

    public int speed = 4;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        // UP, DOWN, LEFT, RIGHT movement controlls
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
	}
}
