using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSelectionTest : MonoBehaviour
{
    public Transform selectionBox;
    public Vector3 firstPos;
    public Vector3 secPos;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    Vector3 temp = Input.mousePosition;
	    temp.z = 1f; // Set this to be the distance you want the object to be placed in front of the camera.
                     //selectionBox.position = Camera.main.ScreenToWorldPoint(temp);

	    if (Input.GetMouseButtonDown(0))
	    {
	        firstPos = Camera.main.ScreenToWorldPoint(temp);

	        selectionBox.transform.localScale = Vector3.zero;
	        selectionBox.transform.position = firstPos;
	    }

	    if (Input.GetMouseButton(0))
	    {
	        secPos = Camera.main.ScreenToWorldPoint(temp);
	        Vector3 movedVector = secPos - firstPos;

	        Vector3 displacement = movedVector;
	        displacement.z = 0.01F;
	        displacement.y *= 1.5f;
	        selectionBox.transform.localScale = -displacement;

        }

	    if (Input.GetMouseButtonUp(0))
	    {
	        selectionBox.transform.localScale = Vector3.zero;
        }

    }
}
