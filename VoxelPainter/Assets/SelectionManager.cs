using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {

    public Transform target;
    public Transform selection;
    public Transform rightPivot;
    void Update() {
        //selection.position = target.position;

        if (Input.GetMouseButtonDown(0)) {
            selection.transform.position = target.position;
        }

        if (Input.GetMouseButton(0))
        {
            //Base the direction of the second touch to the direction of the camera. Aka make sure it's always flat!
        }

    }

}

public enum SelectionType {
    singleFace,
    wholeObject
}
