using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour {

    public List<Transform> selectedList;
    public Dictionary<Transform, Transform> selectedDict = new Dictionary<Transform, Transform>();
    public Transform selectionPivot;
    public Transform selectionGroup;
    public Transform controlPoint;
    public SelectionMode selectionMode;

    public Transform[] handles;
    void Update() {
        foreach (Transform t in selectedList)
        {
            t.GetComponent<MeshRenderer>().material.color = t.GetComponent<ButtonStyleExample>().selectedColor;
        }
    }

    public void OnDeselectAll()
    {

        foreach (Transform t in selectedList)
        {
            t.GetComponent<MeshRenderer>().material.color = t.GetComponent<ButtonStyleExample>().normalColor;
            t.SetParent(selectedDict[t]);
        }

        selectedList.Clear();
        selectedDict.Clear();

    }

    public void OnSelect(RaycastHit hit)
    {

        Transform hitTarget = hit.transform;

        //Output a selection script!
        if (!selectedDict.ContainsKey(hitTarget))
        {

            //get editHandleGroup
            Transform hitParent = hit.transform.parent;
            FaceManager faceManager = hitParent.GetComponent<FaceManager>();

            if (faceManager == null)
            {
                Debug.Log("The parent is missing a face manager");
                return;
            }

            switch (selectionMode)
            {
                  case SelectionMode.selectObject:
                    
                    foreach (FaceButton face in faceManager.faceButtons)
                    {
                          selectedList.Add(face.transform);
                          selectedDict.Add(face.transform, face.transform.parent);
                          selectionPivot.position = face.transform.position;
                    }

                    break;

                case SelectionMode.selectFace:
                    selectedList.Add(hitTarget);
                    selectedDict.Add(hitTarget, hitTarget.transform.parent);
                    selectionPivot.position = hitTarget.position;
                    break;
            }

            SetPivotPoint();

        }
        else {

            //Hack
            Deselect(hit.transform);
            SetPivotPoint();
            
        }
    }

    public void Add(Transform t)
    {
        FaceManager faceManager = t.GetComponent<FaceManager>();

        foreach (FaceButton face in faceManager.faceButtons)
        {
            if (!selectedDict.ContainsKey(face.transform))
            {
                selectedList.Add(face.transform);
                selectedDict.Add(face.transform, face.transform.parent);
                selectionPivot.position = face.transform.position;
            }
        }

    }

    public void Deselect(Transform t)
    {

        Transform hitParent = selectedDict[t];
        FaceManager faceManager = hitParent.GetComponent<FaceManager>();

        switch (selectionMode)
        {
            case SelectionMode.selectObject:
                foreach (FaceButton face in faceManager.faceButtons)
                {

                    face.GetComponent<MeshRenderer>().material.color = face.GetComponent<ButtonStyleExample>().normalColor;
                    selectedList.Remove(face.transform);
                    face.transform.SetParent(selectedDict[face.transform]);
                    selectedDict.Remove(face.transform);

                }
                break;

            case SelectionMode.selectFace:

                t.GetComponent<MeshRenderer>().material.color = t.GetComponent<ButtonStyleExample>().normalColor;
                selectedList.Remove(t);
                t.SetParent(selectedDict[t]);
                selectedDict.Remove(t);

                break;
        }

        //t.SetParent(null);
    }

    public void SetPivotPoint() {

        if (selectedList.Count == 0)
            return;

        Vector3 lowest = Vector3.zero;
        Vector3 highest = Vector3.zero;

        for (int i = 0; i < selectedList.Count; i++)
        {

            if (i == 0)
            {
                lowest = selectedList[i].transform.position;
                highest = selectedList[i].transform.position;
            }

            if (highest.x < selectedList[i].transform.position.x)
                highest.x = selectedList[i].transform.position.x;

            if (selectedList[i].transform.position.x < lowest.x)
                lowest.x = selectedList[i].transform.position.x;


            if (highest.z < selectedList[i].transform.position.z)
                highest.z = selectedList[i].transform.position.z;

            if (selectedList[i].transform.position.z < lowest.z)
                lowest.z = selectedList[i].transform.position.z;

            if (highest.y < selectedList[i].transform.position.y)
                highest.y = selectedList[i].transform.position.y;


            if (lowest.y < selectedList[i].transform.position.y)
                lowest.y = selectedList[i].transform.position.y;

        }

        //print("Highest" + highest);
        //print("Lowest" + lowest);
        //highest.y = 0;

        Vector3 dir = highest - lowest;

        dir.Normalize();
        float dist = Vector3.Distance(highest, lowest);

        selectionPivot.transform.position = highest - (dir * dist / 2);
        controlPoint.transform.position = selectionPivot.position;

        for (int i = 0; i < selectedList.Count; i++)
        {
            selectedList[i].parent = null;
        }

        selectionGroup.transform.position = selectionPivot.position;

        for (int i = 0; i < selectedList.Count; i++)
        {
            //Hack
            selectedList[i].GetComponent<MeshRenderer>().material.color = Color.blue;
            selectedList[i].SetParent(selectionGroup);
        }

    }

}

public enum SelectionMode
{
    selectFace,
    selectObject
}