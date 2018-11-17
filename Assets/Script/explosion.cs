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
        // For now, bombs explode when RIGHT SHIFT is pressed.  ***************** CHANGE LATER *********************
		if (Input.GetKeyDown(KeyCode.RightShift))
        {
            Explode();
        }
	}

    private void Explode()
    {
        Vector3 explosionCenter = transform.position; // The center of the explosion radius

        Collider[] colliders = Physics.OverlapSphere(explosionCenter, radius); // Array of colliders that are within the explosion radius.

        // For every collider in the explosion radius
        foreach (Collider hit in colliders)
        {
            Rigidbody rbHit = hit.GetComponent<Rigidbody>();
            couchPlayerMovement script = hit.GetComponent<couchPlayerMovement>();

            if (rbHit != null)
            {
                // If the object has a couchPlayerMovement script, set explosion to true in the player's script and disable its constraints before applying the explosive force to it.
                if (script != null && Vector3.Distance(rbHit.position, transform.position) < maxRagdollDistance)
                { 
                    script.explosion = true;
                    rbHit.constraints = RigidbodyConstraints.None;
                }

                // Apply explosion force to object, only if it has a rigidbody component.
                rbHit.AddExplosionForce(explosionForce, explosionCenter, radius);
            }
        }
    }
}
