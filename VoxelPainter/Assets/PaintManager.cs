using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintManager : MonoBehaviour {

    public void Paint(FaceButton fb, Color c)
    {
        fb.SetColor(c);
    }

}
