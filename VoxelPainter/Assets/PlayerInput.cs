using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
public class PlayerInput : MonoBehaviour {

    public static PlayerInput _instance;
    public UnityEvent OnColorToggle;
    public UnityEvent OnCursorToggle;
    public SimpleSmoothMouseLook myCam;
    // Update is called once per frame
    void Update () {



        return;

        if (Input.GetKeyDown(KeyCode.C)) {
            InvokeColorToggle();
        }


        if(Input.GetMouseButtonDown(1))
        {
            PlayerController.singletonInstance.scaleManager.ToggleGroup();

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayerController.singletonInstance.ToggleMove();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.E))
        {
            PlayerController.singletonInstance.ToggleEditMode();
        }

        if (PlayerController.singletonInstance.isInEditMode)
        {
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0))
            {
                PlayerController.singletonInstance.SetEditMode(false);
            }
            else
            {
                PlayerController.singletonInstance.SetEditMode(true);
            }

        }

    }

    public void InvokeColorToggle() {
        OnColorToggle.Invoke();
        OnCursorToggle.Invoke();
    }

    public static PlayerInput singletonInstance {
        get {

            if(_instance == null)
                _instance = GameObject.FindObjectOfType<PlayerInput>();
            return _instance;

        }
    }
}
