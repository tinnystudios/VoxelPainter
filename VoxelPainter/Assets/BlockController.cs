using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public GameObject blockPrefab;
    public Transform blockGroup;
    public ScaleManager scaleManager;

    public void Do(RaycastHit hit)
    {
        float size = scaleManager.size;
        Transform hitParent = hit.transform.parent;
        Vector3 hitParentPosition = hitParent.position;

        #region  Block

        GameObject newBlock = Instantiate(blockPrefab);
        newBlock.transform.SetParent(blockGroup);

        newBlock.transform.localScale = Vector3.one * size;

        Vector3 newPos = hit.transform.position;
        Vector3 dir = hit.transform.position - hitParent.position;
        dir = dir * 2;
        dir.Normalize();
        //Dir is either 1 or 0

        float hitSize = hitParent.transform.localScale.x;
        float gap = hitSize - size;

        newBlock.transform.position = hit.transform.parent.position;
        newBlock.transform.position += dir * (size + gap/2);

        #endregion

        FaceButton[] faceManager = newBlock.GetComponent<FaceManager>().faceButtons;

        foreach (FaceButton f in faceManager)
            f.SetColor(ColorPickerController.singletonInstance.GetSelectedColor());



    }

 

}
