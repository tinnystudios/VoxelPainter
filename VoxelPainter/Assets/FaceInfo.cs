using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceInfo : MonoBehaviour {

    public Color myColor;

    void Awake() {
        myColor = ColorPickerController.singletonInstance.GetSelectedColor();
        //ApplyColor();
    }

    public void SetColor(Color c) {
        myColor = c;
        ApplyColor();
    }

    public void ApplyColor() {
        GetComponent<MeshRenderer>().material.color = myColor;
    }

    public void Highlight(Color c) {
       GetComponent<MeshRenderer>().material.color = c;
    }

    public void UnHighlight()
    {
        GetComponent<MeshRenderer>().material.color = myColor;
    }

}
