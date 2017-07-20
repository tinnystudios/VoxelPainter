using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelect : MonoBehaviour {
    public bool isSelected;
    public MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnSelected()
    {
        isSelected = true;
        meshRenderer.material.color = Color.red;
    }

    private void OnUnselected()
    {
        isSelected = false;
        meshRenderer.material.color = Color.white;
    }
}
