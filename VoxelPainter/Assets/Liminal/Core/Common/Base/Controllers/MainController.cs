using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script should only handle the inputs and then output it to other managers
public class MainController : MonoBehaviour
{
    public static MainController _instance;
    public BaseButtonController baseButtonController;
    public Transform connectedGroup;
    public Transform editHandleGroup;
    public Transform slideGroup;

    public LayerMask buttonMask;
    public Ray ray;

    [Header(("Managers"))]
    public MyCamera myCamera;
    public SelectionController selectionController;
    public BlockController blockController;
    public ScaleManager scaleManager;
    public SlotManager slotManager;
    public ColorPickerController colorController;
    public PaintManager paintManager;
    public DrawSelection marqueeManager;
    #region Base Button Inputs

    public void BaseButtonInputs() {

        if (Input.GetMouseButtonDown(0))
        {
            baseButtonController.OnButtonDown();
        }
        if (Input.GetMouseButton(0))
        {
            baseButtonController.OnButton();
        }

        if (Input.GetMouseButtonUp(0))
        {
            baseButtonController.OnButtonUp();
        }

        if (Input.GetMouseButtonUp(1))
        {
            scaleManager.ToggleGroup();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            slotManager.SelectSlot(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            slotManager.SelectSlot(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            slotManager.SelectSlot(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            slotManager.SelectSlot(3);


        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            slotManager.SelectSlot(4);

        }
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.M))
        {
            slotManager.SelectSlot(5);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            colorController.OnColorToggle();
        }
    }

    #endregion

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
            return;

        BaseButtonInputs();

        if (Input.GetKeyDown(KeyCode.F))
            myCamera.Focus(selectionController.selectionPivot.position);

        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (slotManager.selectedSlot.slotInfo.itemType != ItemType.move || selectionController.selectedList.Count == 0)
            selectionController.controlPoint.gameObject.SetActive(false);
        else
            selectionController.controlPoint.gameObject.SetActive(true);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buttonMask))
        {
            switch (slotManager.selectedSlot.slotInfo.itemType)
            {
                case ItemType.block:
                    if(Input.GetMouseButtonUp(0))
                        blockController.Do(hit);
                    break;

                case ItemType.axe:
                    if (Input.GetMouseButtonUp(0))
                    {
                        //if it is selected, remove from lists
                        if (selectionController.selectedDict.ContainsKey(hit.transform))
                            selectionController.Deselect(hit.transform);

                        Destroy(hit.transform.parent.gameObject);
                    }
                    break;

                case ItemType.paintBucket:
                    if (Input.GetMouseButton(0))
                        paintManager.Paint(hit.transform.GetComponent<FaceButton>(),ColorPickerController.singletonInstance.GetSelectedColor());

                    break;

                case ItemType.select:
                case ItemType.move:

                    if (hit.transform.tag == "EditHandle")
                        break;

                    if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
                        selectionController.OnSelect(hit);
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            selectionController.OnDeselectAll();
                            selectionController.OnSelect(hit);
                        }
                    }
                    break;
            }
        }
        else {
            if (Input.GetMouseButtonDown(0))
            {
                selectionController.OnDeselectAll();
            }
        }


    }

    public static MainController singletonInstance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<MainController>();
            return _instance;
        }
    }


}
