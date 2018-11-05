using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class incinerator : MonoBehaviour {

    public bool incinerate = false;

    private void Update()
    {
        if (Input.GetKeyDown("enter"))
        {
            if (!incinerate)
            {
                incinerate = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (incinerate)
        {
            if (other.gameObject.tag == "Destructable")
            {
                Destroy(other);
            }
        }
    }
}
