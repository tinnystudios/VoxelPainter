using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Move : MonoBehaviour {
    public Transform myCam;
    public float moveSpeed = 5;
    public bool hasGravity;
	// Update is called once per frame
	void Update () {
        Vector2 inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 moveDir = myCam.forward * inputDir.y;

        //To ground it
        if (hasGravity)
        {
            moveDir.y = 0;
            GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (inputDir.x != null)
        {
            moveDir += myCam.right * inputDir.x;
        }

        transform.position += moveDir * Time.deltaTime * moveSpeed;

    }
}
