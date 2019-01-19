using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class runWithArms : MonoBehaviour {

    public class MovementInfo {
        public Vector3 currentPoint;
        public Vector3 previousPoint;
        public Vector3 direction;
        public float magnitude = 0;

        public MovementInfo(Vector3 current = new Vector3(), Vector3 previous = new Vector3(), Vector3 direction = new Vector3(), float magnitude = 0) {
            this.currentPoint = current;
            this.previousPoint = previous;
            this.direction = direction;
            this.magnitude = 0;
        }
        public override string ToString() {
            return ("currentPoint: " + this.currentPoint +
                    ", previousPoint: " + this.previousPoint +
                    ", direction: " + this.direction +
                    ", magnitude: " + this.magnitude);
        }
    };

    public GameObject controllerLeft;
    public GameObject controllerRight;
    private List<MovementInfo> movementLeft = new List<MovementInfo>(); //info regarding the movement of the left controller
    private List<MovementInfo> movementRight = new List<MovementInfo>(); //info regarding the movement of the right controller
    private int sizeOfMovementLists = 30; //the number of elements in the movement lists

    void Start () {
        for (int i = 0; i < sizeOfMovementLists; i++) {
            movementLeft.Add(new MovementInfo());
            movementRight.Add(new MovementInfo());
        }
        Vector3 test = new Vector3(0.5f, 0.0f, 0.9f);
        Debug.Log(test.magnitude);
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(movementLeft[0] + "\n" + movementLeft[0].magnitude);
	}

    private void FixedUpdate() {
        Vector3 current = controllerLeft.transform.position;
        Vector3 previous = movementLeft[0].currentPoint;
        Vector3 direction = current - previous.normalized;
        float magnitude = direction.magnitude; //figure out why magniture is zero
        direction = direction.normalized;    
        movementLeft.RemoveAt(movementLeft.Count - 1);
        movementLeft.Insert(0, new MovementInfo(current, previous, direction, magnitude));

        current = controllerRight.transform.position;
        previous = movementRight[0].currentPoint;
        direction = current - previous;
        magnitude = direction.magnitude; //figure out why magniture is zero
        direction = direction.normalized;
        movementRight.RemoveAt(movementRight.Count - 1);
        movementRight.Insert(0, new MovementInfo(current, previous, direction, magnitude));
    }
}
