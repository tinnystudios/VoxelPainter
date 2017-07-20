using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceButton : BaseButton {

    public void SetColor(Color c)
    {
        GetComponent<MeshRenderer>().material.color = c;
        GetComponent<ButtonStyleExample>().normalColor = c;
    }

    public void Highlight()
    {
        GetComponent<MeshRenderer>().material.color = GetComponent<ButtonStyleExample>().selectedColor;
    }

    public void UnHighlight()
    {
        GetComponent<MeshRenderer>().material.color = GetComponent<ButtonStyleExample>().normalColor;
    }
}
