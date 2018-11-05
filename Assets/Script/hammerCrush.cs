using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hammerCrush : MonoBehaviour {

    public GameObject hammer;

    private bool hammerDown;
    private bool hammerUp;

    public Vector3 downAngle = new Vector3(0.0f, 0.0f, 90f);
    public Vector3 upAngle = new Vector3(0.0f, 0.0f, 0.0f);
    public float hammerSpeed = 3.0f;
    public float raiseSpeed = 1.5f;
    public float coolDownSpeed = 1.0f;

    private float moveTime = 0f;
    private Vector3 currentAngle;

	// Use this for initialization
	void Start () {
        hammerDown = false;
        currentAngle = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("space") && !hammerDown && !hammerUp)
        {
            hammerDown = true;
        }

        // Lower hammer
        if (hammerDown)
        {
            currentAngle = Vector3.Lerp(upAngle, downAngle, moveTime);

            hammer.transform.eulerAngles = currentAngle;
            moveTime += Time.deltaTime * hammerSpeed;

            // Once hammer is fully lowered
            if (hammer.transform.eulerAngles == downAngle)
            {
                hammerUp = true;
                hammerDown = false;
                moveTime = 0f;
            }
        }

        // Raise hammer
        if (hammerUp)
        {
            currentAngle = Vector3.Lerp(downAngle, upAngle, moveTime);

            hammer.transform.eulerAngles = currentAngle;
            moveTime += Time.deltaTime * raiseSpeed;

            // Once hammer is fully raised
            if (hammer.transform.eulerAngles == upAngle)
            {
                hammerUp = false;
                moveTime = 0f;
            }

        }
	}
}
