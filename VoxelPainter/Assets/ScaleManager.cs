using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ScaleManager : MonoBehaviour {

    public GameObject scaleGroup;
    public Slider slider;
    public InputField inputField;
    public float size; //size of the brush.

    public void ToggleGroup() {
        scaleGroup.SetActive(!scaleGroup.activeInHierarchy);
        MainController.singletonInstance.enabled = !scaleGroup.activeInHierarchy;
    }

    public void ApplyScale()
    {
        ToggleGroup();

        SelectionController sC = MainController.singletonInstance.selectionController;

        //resize
        sC.selectionGroup.transform.localScale = Vector3.one * size;

        //degroup
        foreach (Transform t in sC.selectedList)
            t.SetParent(null);

        //reset size
        sC.selectionGroup.transform.localScale = Vector3.one;

        //regroup
        foreach (Transform t in sC.selectedList)
            t.SetParent(sC.selectionGroup);
    }

    public void CancelScale()
    {
        ToggleGroup();
    }

    void Update() {

        if (EventSystem.current.currentSelectedGameObject != null) {

            if (EventSystem.current.currentSelectedGameObject.GetComponent<Slider>())
            {
                inputField.text = slider.value.ToString();
            }
            else
            {
                slider.value = float.Parse(inputField.text);
                size = slider.value;
            }

        }


    }

}
