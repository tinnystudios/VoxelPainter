using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleButton : BaseButton {

    public Transform parent;
    public Transform groupTransform;

    public Axis hasAxis;
    
    void Update() {
        SetOrientation();
        if (isPressed)
        {
            SetPosition();
            MainController.singletonInstance.marqueeManager.enabled = false;
        }
    }

    [System.Serializable]
    public class Axis
    {
        public bool x = true;
        public bool y = true;
        public bool z = true;
    }

    #region ### Overriding Button Fuctions ###

    public override void OnButton(ButtonHitInfo hitInfo)
    {
        base.OnButton(hitInfo);
        SetPosition();
    }

    public void SetPosition()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        Vector2 axis = new Vector2(x, y);
        Vector3 newPos = Vector3.zero;

        if (hasAxis.x)
            newPos.x -= x;
        if (hasAxis.y)
            newPos.y += y;
        if (hasAxis.z)
            newPos.z -= y;

        groupTransform.localPosition += newPos * Time.deltaTime * 100;
        MainController.singletonInstance.selectionController.selectionGroup.position = groupTransform.position;
        MainController.singletonInstance.selectionController.selectionPivot.position = groupTransform.position;
    }

    public override void OnButtonDown(ButtonHitInfo hitInfo)
    {

 


        base.OnButtonDown(hitInfo);

        SetPosition();
    }

    public override void OnButtonUp()
    {
        base.OnButtonUp();
        parent.transform.position = groupTransform.position;
        groupTransform.localPosition = Vector3.zero;
        MainController.singletonInstance.marqueeManager.enabled = true;

    }

    #endregion

    public void SetOrientation() {
        parent.forward = -Camera.main.transform.forward;
        Vector3 ang = parent.eulerAngles;

        if (ang.y > 225 && ang.y < 315)
            ang.y = 270;
        if (ang.y > 135 && ang.y < 225)
            ang.y = 180;
        if (ang.y > 45 && ang.y < 135)
            ang.y = 90;
        if (ang.y > 315)
            ang.y = 0;
        if (ang.y < 45)
            ang.y = 0;

        parent.eulerAngles = ang;
        parent.eulerAngles = new Vector3(0, parent.eulerAngles.y, parent.eulerAngles.z);
    }

}
