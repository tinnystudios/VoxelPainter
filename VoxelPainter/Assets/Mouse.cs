using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour {
    // C#
    public float horizontalSpeed = 10f; // looks like "10" maps to the native speed
    public float verticalSpeed = 10f;

    public Transform cursorObj;

    public bool allowCursorMove = true;

    void CheckCursorLock()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Start()
    {
        CheckCursorLock();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckCursorLock();
        }
        float h = horizontalSpeed * Input.GetAxis("Mouse X");
        float v = verticalSpeed * Input.GetAxis("Mouse Y");
        Vector3 delta = new Vector3(h, v, 0);
        if (allowCursorMove)
        {
            cursorObj.position += delta * Time.deltaTime; // moves the virtual cursor
                                         // You need to clamp the position to be inside your wanted area here,
                                         // otherwise the cursor can go way off screen
        }
    }
}
