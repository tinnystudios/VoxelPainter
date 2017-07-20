using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ColorPickerController : MonoBehaviour {

    public static ColorPickerController _instance;
    public AColorPicker aColorPicker;
    public GameObject groupColorPicker;
    public bool isShowColorPicker;
    public SimpleSmoothMouseLook mouseLook;
    public Color lastColor;
    
    void Awake() {
        //PlayerInput.singletonInstance.OnColorToggle.AddListener(OnColorToggle);
    }

    public void SetSelectedColor(Color c) {
        aColorPicker.selectedColorDisplay.color = c;
    }

    public void OnColorToggle() {
        isShowColorPicker = !isShowColorPicker;
        MainController.singletonInstance.enabled = !isShowColorPicker;

        groupColorPicker.SetActive(isShowColorPicker);

        if (isShowColorPicker)
            lastColor = aColorPicker.CurrentColor;

    }

    public Color GetSelectedColor() {
        Color c = aColorPicker.selectedColorDisplay.color;
        return c;
    }

    public void Close() {
        //go back to last color!
        aColorPicker.selectedColorDisplay.color = lastColor;
        //PlayerInput.singletonInstance.InvokeColorToggle();
        OnColorToggle();
    }

    public void Ok() {

        //Set Color
        SelectionController sC = MainController.singletonInstance.selectionController;
        for (int i = 0; i < sC.selectedList.Count; i++)
            sC.selectedList[i].GetComponent<FaceButton>().SetColor(GetSelectedColor());

        //Temporarily deselect all to show that the color has changed
        sC.OnDeselectAll();

        OnColorToggle();

    }

    public static ColorPickerController singletonInstance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<ColorPickerController>();
            return _instance;
        }
    }

}
