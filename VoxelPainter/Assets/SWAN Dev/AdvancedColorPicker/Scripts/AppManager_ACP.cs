﻿/// <summary>
/// Created by SWAN Dev
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AppManager_ACP : MonoBehaviour {

	public CanvasScaler m_MainCanvasScaler;
	public Transform m_ComponentContainer;
	public Button btn_Picker;
	public Button btn_Picker2;
	public TextMesh text;
	public MeshRenderer cubeRenderer1;
	public MeshRenderer cubeRenderer2;

	private static AppManager_ACP _instance = null;
	public static AppManager_ACP Instance
	{
		get{
			return _instance;
		}
	}

	void Awake()
	{
		_instance = this;
		if(Screen.width > Screen.height) //landscape
		{
			m_MainCanvasScaler.referenceResolution = new Vector2(1920f, 1920f * (float)Screen.height / (float)Screen.width);
		}
		else //portrait or 1:1
		{
			m_MainCanvasScaler.referenceResolution = new Vector2(1080f, 1080f * (float)Screen.height / (float)Screen.width);
		}

	}

	Texture2D texture2d;
	void Update()
	{
		if(picker)
		{
			text.color = picker.CurrentColor;
			cubeRenderer1.material.color = picker.CurrentColor;
			cubeRenderer2.material.color = picker.CurrentColor;
			if(picker.CurrentPickedColorSample){
				//Do what you want with the picked texture here:
				texture2d = picker.CurrentPickedColorSample;
				//Debug.Log("TextureColor: " + texture2d.GetPixel(1,1));
			}
		}
	}

	AColorPicker picker;
	public void ShowColorPicker()
	{
		picker = AColorPicker.Create(m_ComponentContainer, "AColorPickerUGUI_Prefab");
		picker.Setup(()=>{
			SetBasicUIVisible(true);
		});
		picker.transform.localPosition = new Vector3(0, -200, 0);
		SetBasicUIVisible(false);
	}

	public void ShowColorPicker2()
	{
		picker = AColorPicker.Create(m_ComponentContainer, "AColorPickerUGUI_2_Prefab");
		picker.Setup(()=>{
			SetBasicUIVisible(true);
		});
		picker.transform.localPosition = new Vector3(0, -200, 0);
		SetBasicUIVisible(false);
	}

	public void SetBasicUIVisible(bool isVisible)
	{
		btn_Picker.gameObject.SetActive(isVisible);
		btn_Picker2.gameObject.SetActive(isVisible);
	}


}
