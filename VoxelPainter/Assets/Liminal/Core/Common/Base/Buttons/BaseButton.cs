using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseButton : MonoBehaviour {

    public BaseButtonStyle buttonStyle;

    //Make get/setters
    [HideInInspector] public ButtonHitInfo m_hitInfo;
    [HideInInspector] public bool isHover;
    [HideInInspector] public bool isPressed;

    //For outsiders to listen
    public delegate void ButtonDelegate();
    public event ButtonDelegate d_OnButtonDown;
    public event ButtonDelegate d_OnButtonUp;

    public virtual void OnButtonDown(ButtonHitInfo hitInfo) {
        isPressed = true;
        m_hitInfo = hitInfo;
        buttonStyle.OnButtonDown(hitInfo);

        if(d_OnButtonDown != null)
            d_OnButtonDown.Invoke();
    }

    public virtual void OnButton(ButtonHitInfo hitInfo)
    {
        m_hitInfo = hitInfo;
        buttonStyle.OnButton();
        
    }

    public virtual void OnButtonUp()
    {
        isPressed = false;
        buttonStyle.OnButtonUp();

        if (d_OnButtonUp != null)
            d_OnButtonUp.Invoke();

    }

    public virtual void OnButtonHoverDown()
    {
        if (isHover)
            return;

        buttonStyle.OnButtonHoverDown();
        isHover = true;
    }

    public virtual void OnButtonHover()
    {
        buttonStyle.OnButtonHover();
    }

    public virtual void OnButtonHoverUp()
    {
        isHover = false;
        buttonStyle.OnButtonHoverUp();
    }

}
