using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : BaseButton {
    public Transform controlPoint;
    public Transform parent;

    void Update() {
       transform.eulerAngles = Vector3.zero;

        if (MainController.singletonInstance.connectedGroup == transform) {
            if(!Input.GetMouseButton(0))
                SetOrientation();
        }

    }

    public override void OnButtonDown(ButtonHitInfo hitInfo)
    {
        base.OnButtonDown(hitInfo);

    }
    
    public override void OnButtonUp()
    {
        SetOrientation();
        base.OnButtonUp();
    }

    public override void OnButtonHover()
    {
        base.OnButtonHover();
    }

    public void SetOrientation() {
        return;
        MainController.singletonInstance.connectedGroup = transform;

        controlPoint.transform.position = transform.position;
        controlPoint.forward = -Camera.main.transform.forward;
        Vector3 ang = controlPoint.eulerAngles;

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


        controlPoint.eulerAngles = ang;
        controlPoint.eulerAngles = new Vector3(0, parent.eulerAngles.y, parent.eulerAngles.z);

        transform.SetParent(parent);
    }



}
