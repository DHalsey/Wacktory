using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour {

    public float explosionForce = 10.0f;
    public float radius = 5.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space"))
        {
            Explode();
        }
	}

    private void Explode()
    {
        Debug.Log("EXPLODE");
        Vector3 explosionCenter = transform.position;

        Collider[] colliders = Physics.OverlapSphere(explosionCenter, radius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rbHit = hit.GetComponent<Rigidbody>();

            if (rbHit != null)
            {
                rbHit.AddExplosionForce(explosionForce, explosionCenter, radius);
            }
        }
    }
}
