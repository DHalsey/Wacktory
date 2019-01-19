using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conveyorBreak : MonoBehaviour {
    public GameObject conveyorMover; //collision object that actually moves the object

    private conveyorMove conveyorScript;

    private bool wasConveyorBroken; //holds the last state of the hammer so we can check for a change
    private bool isConveyorBroken;  //the current status of the hammer

    private float repairProgress = 0f;
    private float breakTime = 30.0f;
    private float currentRepairedTime = 0f;

    void Start() {
        conveyorScript = conveyorMover.GetComponent<conveyorMove>();
        isConveyorBroken = conveyorScript.isBroken;
    }

    // Update is called once per frame
    void Update() {
        DrawDebug();
        isConveyorBroken = conveyorScript.isBroken;
        //if we moved from a repaired state to a broken, Break()
        if (isConveyorBroken && !wasConveyorBroken) {
            Break();
        }

        //if we moved from a broken state to a repaired, Repair()
        if (!isConveyorBroken && wasConveyorBroken) {
            Repair();
        }

        if (!isConveyorBroken) currentRepairedTime += Time.deltaTime;
        if (currentRepairedTime >= breakTime) {
            Break();
            currentRepairedTime = 0f;
        }

        wasConveyorBroken = isConveyorBroken;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Wrench") {
            repairProgress += 0.5f;
            if (repairProgress >= 1f) {
                repairProgress = 0f;
                Repair();
            }
        }
    }

    //added a trigger check as well because kinematic objects dont trigger collisions
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Wrench") {
            repairProgress += 0.5f;
            if (repairProgress >= 1f) {
                repairProgress = 0f;
                Repair();
            }
        }
    }

    public bool debugBreak = false;
    public bool debugRepair = false;
    //used to quickly toggle a repair or break
    void DrawDebug() {
        if (debugBreak) {
            debugBreak = false;
            Break();
        }
        if (debugRepair) {
            debugRepair = false;
            Repair();
        }
    }

    void Break() {
        conveyorScript.Break();
    }
    void Repair() {
        conveyorScript.Repair();
    }


}
