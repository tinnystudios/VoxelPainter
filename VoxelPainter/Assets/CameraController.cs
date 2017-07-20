using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float horizontalSpeed = 100f; // looks like "10" maps to the native speed
    public float verticalSpeed = 100f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");

        float h = horizontalSpeed * Input.GetAxis("Mouse X");
        float v = verticalSpeed * Input.GetAxis("Mouse Y");

        //transform.RotateAround(Vector3.zero, -transform.right, v * Time.deltaTime);
        transform.RotateAround(Vector3.zero, Vector3.up, h * Time.deltaTime);

    }
}
