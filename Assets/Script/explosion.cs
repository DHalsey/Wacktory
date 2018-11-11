using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour {

    public float explosionForce = 500.0f;
    public float radius = 10.0f;
    public float maxRagdollDistance = 4.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.RightShift))
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
            couchPlayerMovement script = hit.GetComponent<couchPlayerMovement>();

            if (rbHit != null)
            {
                if (script != null && Vector3.Distance(rbHit.position, transform.position) < maxRagdollDistance)
                { 
                    script.explosion = true;
                    rbHit.constraints = RigidbodyConstraints.None;
                }

                rbHit.AddExplosionForce(explosionForce, explosionCenter, radius);
            }
        }
    }
}
