using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceManager : MonoBehaviour
{

    public FaceButton[] faceButtons;

    public void Highlight()
    {
        for (int i = 0; i < faceButtons.Length; i++)
        {
            faceButtons[i].Highlight();
        }
    }

    public void UnHighlight()
    {
        for (int i = 0; i < faceButtons.Length; i++)
        {
            faceButtons[i].UnHighlight();

        }
    }

}
