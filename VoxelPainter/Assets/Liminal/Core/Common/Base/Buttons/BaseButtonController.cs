using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//When a controller presses a button this scripts receives it and creates a raycast to look for buttons

public class BaseButtonController : MonoBehaviour {

    public BaseButton baseButton;
    [HideInInspector] public ButtonHitInfo m_hitInfo; //For some reason if this is private, the script turns off.
    private RaycastHit hit;
    public LayerMask buttonMask;

    Ray ray;

    void Awake() {
        m_hitInfo.initiator = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Debug();
        HoverRaycast();
    }

    public void HoverRaycast() {

        //If button is pressing (don't look for other buttons)
        if (baseButton != null)
            if (baseButton.isPressed)
                return;

        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buttonMask))
        {
            if (baseButton == null)
                baseButton = hit.transform.GetComponent<BaseButton>();
            else
            {
                //When you hover over a different object
                if (baseButton.transform != hit.transform)
                    OnHoverUp();
                else {

                    if (!baseButton.isHover)
                        baseButton.OnButtonHoverDown();
                    else
                        baseButton.OnButtonHover();

                }
            }
        }
        else
        {
            OnHoverUp();
        }
    }

    public void Debug()
    {
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, transform.position + (transform.forward * 1000));
    }

    public void OnButton() {
        //On button down, do a raycast check.
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buttonMask))
        {
            m_hitInfo.hit = hit;

            if (baseButton != null)
                baseButton.OnButton(m_hitInfo);

        }
    }

    public void OnButtonDown()
    {
        //On button down, do a raycast check.
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buttonMask))
        {
            m_hitInfo.hit = hit;
            if (baseButton != null)
                baseButton.OnButtonDown(m_hitInfo);
        }
    }

    public void OnHoverUp()
    {
        if (baseButton == null)
            return;

        baseButton.OnButtonHoverUp();
        baseButton = null;
    }

    public void OnButtonUp()
    {
        if (baseButton == null)
            return;

        baseButton.OnButtonUp();
        HoverRaycast();
        baseButton = null;

    }

}
