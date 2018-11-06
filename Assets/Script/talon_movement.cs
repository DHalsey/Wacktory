using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talon_movement : MonoBehaviour {

    public float speed = 4.0f;
    public bool isDead = false;
    public Random rnd;

    public Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
    public void killPlayer()
    {
        // Killing player, setting speed to very slow, laying pill on side
        isDead = true;
        speed = 0.5f;

        // Dampen towards the target rotation
        transform.rotation = new Quaternion(0.0f, 0.0f, 90.0f, 0.0f);

        rb.AddForce(new Vector3(Random.Range(-1000.0f, 1000.0f), 0, Random.Range(-1000.0f, 1000.0f)));

    }

    public void setSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // Update is called once per frame
    void Update() {
        
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
