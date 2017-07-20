/// <summary>
/// Created by SWAN Dev
/// </summary>

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using System.IO;

public class AColorPicker : MonoBehaviour 
{
	public Transform containerT;
	public bool hasAlpha = false;

	public DLabel labelHexCode;
	private Texture2D colorSpaceTex;
	public RawImage colorSpace;
	private Texture2D alphaGradientTex;
	public RawImage alphaGradient;

	public Transform defaultColorContainer;
	private Image[] defaultColors;

	public Image selectedColorDisplay;
	public Image displayReadPixel;

	public Slider sliderR;
	public Slider sliderG;
	public Slider sliderB;
	public Slider sliderA;

	public DLabel labelR;
	public DLabel labelG;
	public DLabel labelB;
	public DLabel labelA;

	public RectTransform rectPicker;
	private Vector3 originPickerPos;
	public Text pickerBtnMessage;
	private Vector3 rectPos;

	public GameObject pallettePanelGO;

	public Button btn_Close;
	public Button btn_Save;

	public Image appCursor;

	private float canvasScale = 1f;
    public string hexCode = "not selected";
	public Color CurrentColor
	{
		get{
			return selectedColorDisplay.color;
		}
	}

	public Texture2D CurrentPickedColorSample
	{
		get{
			return readTex;
		}
	}


	/// <summary>
	/// Create an instance of AColorPicker, and set editHandleGroup.
	/// </summary>
	/// <param name="parentT">The container/editHandleGroup for this instance.</param>
	public static AColorPicker Create(Transform parentT, string prefabName = "AColorPickerUGUI_Prefab")
	{
		GameObject prefab = Resources.Load(prefabName) as GameObject;
		AColorPicker gifPanel = _InstantiatePrefab<AColorPicker>(prefab);
		gifPanel.transform.SetParent(parentT);
		if(parentT) gifPanel.transform.rotation = parentT.rotation;
		gifPanel.transform.localScale = Vector3.one;
		gifPanel.transform.localPosition = Vector3.zero;
		return gifPanel;
	}

	private static T _InstantiatePrefab<T>(GameObject prefab) where T: MonoBehaviour
	{
		if(prefab != null)
		{
			GameObject go = GameObject.Instantiate(prefab) as GameObject;
			if(go != null)
			{
				go.name = "[Prefab]" + prefab.name;
				go.transform.localScale = Vector3.one;
				return go.GetComponent<T>();
			}
			else
			{	
				Debug.Log("prefab is null!") ;
				return null ;
			}
		}
		else
			return null ;
	}

	bool hasSetup = false;
	void Start()
	{
		Setup();
	}

	// Use this for initialization
	public void Setup(Action onCloseAction = null) 
	{
		if(hasSetup) return;
		hasSetup = true;

		_onCloseAction = onCloseAction;

		transform.localScale = Vector3.one;

		colorSpaceTex = (Texture2D) colorSpace.mainTexture;
		alphaGradientTex = (Texture2D) alphaGradient.mainTexture;

		if(AppManager_ACP.Instance != null)
		{
			StartCoroutine(_SetCanvasScale(AppManager_ACP.Instance.m_MainCanvasScaler));
		}
		else
		{
			CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
			if(Screen.width > Screen.height) //landscape
			{
				canvasScaler.referenceResolution = new Vector2(1920f, 1920f * (float)Screen.height / (float)Screen.width);
			}
			else //portrait or 1:1
			{
				canvasScaler.referenceResolution = new Vector2(1080f, 1080f * (float)Screen.height / (float)Screen.width);
			}

			StartCoroutine(_SetCanvasScale(canvasScaler));
		}

		sliderA.gameObject.SetActive(hasAlpha);
		labelA.transform.parent.gameObject.SetActive(hasAlpha);

		//btn_Save.transform.editHandleGroup.gameObject.SetActive(false);

		defaultColors = defaultColorContainer.GetComponentsInChildren<Image>();

		if(EventSystem.current == null)
		{
			GameObject go = new GameObject("[EventSystem]");
			go.AddComponent<EventSystem>();
			go.AddComponent<StandaloneInputModule>();
			cursor = new PointerEventData(EventSystem.current);
		}

		gameObject.SetActive(true);
	}

	IEnumerator _SetCanvasScale(CanvasScaler canvasScaler)
	{
		yield return new WaitForEndOfFrame();
		canvasScale = canvasScaler.transform.localScale.x;
		pickRectSize = 128 * canvasScale;
	}

	PointerEventData cursor = new PointerEventData(EventSystem.current);
	List<RaycastResult> objectsHit = new List<RaycastResult> ();
	Color color;
	//Update is called once per frame
	void Update () 
	{

        if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButton(0)) {
            return;
        }

		cursor.position = Input.mousePosition;
		if(enableRectPicker)
		{
			rectPos = cursor.position;
			rectPicker.position = new Vector2(Mathf.Clamp(rectPos.x, pickRectSize/2f, Screen.width-pickRectSize/2f), 
				Mathf.Clamp(rectPos.y, pickRectSize/2f, Screen.height-pickRectSize/2f));
		}

		EventSystem.current.RaycastAll(cursor, objectsHit);
		if(objectsHit.Count > 0)
		{
			bool set = false;
			foreach(RaycastResult r in objectsHit)
			{
				//Colors
				if(r.gameObject.Equals(colorSpace.gameObject))
				{
					appCursor.transform.position = (new Vector3(cursor.position.x, cursor.position.y, appCursor.transform.position.z));
					color = colorSpaceTex.GetPixel((int)((cursor.position.x - colorSpace.transform.position.x)/canvasScale), 
						(int)((cursor.position.y - colorSpace.transform.position.y)/canvasScale));
					_SetColor(color);
				}	
				else if(r.gameObject.name.Equals(alphaGradient.name))
				{
					appCursor.transform.position = (new Vector3(cursor.position.x, cursor.position.y, appCursor.transform.position.z));
					if(hasAlpha)
					{
						//Alpha
						color = alphaGradientTex.GetPixel((int)((cursor.position.x - alphaGradient.transform.position.x)/canvasScale), 
							(int)((cursor.position.y - alphaGradient.transform.position.y)/canvasScale));
						_SetAlpha(color.r);
					}
					else
					{//Grey scale
						color = alphaGradientTex.GetPixel((int)((cursor.position.x - alphaGradient.transform.position.x)/canvasScale), 
							(int)((cursor.position.y - alphaGradient.transform.position.y)/canvasScale));
						_SetColor(color);
					}
				}
				else
				{
					foreach(Image img in defaultColors)
					{
						if(r.gameObject.Equals(img.gameObject))
						{
							appCursor.transform.position = (new Vector3(cursor.position.x, cursor.position.y, appCursor.transform.position.z));
							_SetColor(img.color);
						}
					}
				}
			}
		}
	}

	//Call this at the Save button (Btn_Save) is clicked
	public void SaveCurrentColorSample()
	{
		if(CurrentPickedColorSample != null)
		{
			SaveColorSample(CurrentPickedColorSample);

			btn_Save.enabled = false;
			btn_Save.transform.parent.gameObject.SetActive(false);
		}
	}
	private void SaveColorSample(Texture2D tex2D)
	{
		_SaveImageToDataPath(tex2D, "ColorSamples", "ColorSample_" + DateTime.Now.ToString("yyyyMMddHHmmss"), DataPathType.PersistentDataPath);
	}

	private void _ClearPickedSampleTexture()
	{
		//Clear last picked texture before killing AColorPicker to avoid memory leak
		if(readTex) Texture2D.DestroyImmediate(readTex);
		readTex = null;
	}


	bool enableRectPicker = false;
	Texture2D readTex;
	float pickRectSize = 128f;
	float nextUpdateTime_DisplayReadPixel = 0;

	//This function take screenshot at specific rect on screen, and read the pixels for display/preview
	IEnumerator ReadPixelInPickerRect()
	{
		if(Time.time > nextUpdateTime_DisplayReadPixel)
		{
			//Clear last picked texture
			_ClearPickedSampleTexture();

			//Pick image for calculate a color
			readTex = new Texture2D((int)pickRectSize, (int)pickRectSize, TextureFormat.RGB24, false);
			yield return new WaitForEndOfFrame();
			Rect rect = new Rect(rectPicker.position.x-pickRectSize/2f, rectPicker.position.y-pickRectSize/2f, pickRectSize, pickRectSize);
			//Debug.Log("rect: " + rect);
			readTex.ReadPixels(rect, 0, 0);
			readTex.Apply();

			yield return new WaitForEndOfFrame();

			//Set picked result
			_SetPickedSampleResult(readTex);

			yield return new WaitForSeconds(0.2f);

			//Flag
			enableRectPicker = false;

			//Reset picker rect
			pickerBtnMessage.gameObject.SetActive(true);
			rectPicker.position = originPickerPos;

			//Show
			pallettePanelGO.SetActive(true);
			appCursor.gameObject.SetActive(true);
			btn_Close.gameObject.SetActive(true);


			btn_Save.transform.parent.gameObject.SetActive(true);
			btn_Save.enabled = true;
		}
		yield return null;
	}

	private void _SetPickedSampleResult(Texture2D tex)
	{
		if(tex == null) return;
		displayReadPixel.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
		SetColorWithTexture(tex);
	}

	//Call this at CenterPickRect is clicked
	public void ReadColorFromRectPicker()
	{
		if(!enableRectPicker){
			originPickerPos = rectPicker.position;

			//Hide
			pickerBtnMessage.gameObject.SetActive(false);
			pallettePanelGO.SetActive(false);
			appCursor.gameObject.SetActive(false);
			btn_Close.gameObject.SetActive(false);

			//Flag
			enableRectPicker = true;
			nextUpdateTime_DisplayReadPixel = Time.time + 0.5f;
		}

		StartCoroutine(ReadPixelInPickerRect());
	}

	//Pick color from a Texture2D
	private void SetColorWithTexture(Texture2D tex)
	{
		// This function read pixel(s) from a Texture2D, and get/calculate the color for chroma key
		bool onePointColor = false;
		_IsOnSilderValueChange = false;
		if(onePointColor)
		{
			Color getColor = tex.GetPixel((int)Screen.width/2, (int)Screen.height/2);
			_SetColor(getColor);
		}
		else
		{
			Color[] getColors = tex.GetPixels();
			float r = 0f, g = 0f, b = 0f;
			foreach(Color c in getColors)
			{
				r += c.r;
				g += c.g;
				b += c.b;
			}
			Color resultColor = new Color(r/(float)getColors.Length, g/(float)getColors.Length, b/(float)getColors.Length);
			_SetColor(resultColor);
		}
	}

	bool _IsSetColor = false;
	private void _SetColor(Color color)
	{
		if(_IsOnSilderValueChange)
		{
			_IsOnSilderValueChange = false;
			return;
		}

		labelR.SetText(Mathf.FloorToInt(255 * color.r).ToString());
		labelG.SetText(Mathf.FloorToInt(255 * color.g).ToString());
		labelB.SetText(Mathf.FloorToInt(255 * color.b).ToString());
		_IsSetColor = true;
		sliderR.value = color.r;
		_IsSetColor = true;
		sliderG.value = color.g;
		_IsSetColor = true;
		sliderB.value = color.b;
		selectedColorDisplay.color = new Color(color.r, color.g, color.b, selectedColorDisplay.color.a);
		_SetHexCode();
		_CursorVisible(true);
	}

	private void _SetAlpha(float a)
	{
		labelA.SetText((255 * a).ToString());
		sliderA.value = a;
		selectedColorDisplay.color = new Color(selectedColorDisplay.color.r, selectedColorDisplay.color.g, selectedColorDisplay.color.b, a);
		_SetHexCode();
		_CursorVisible(true);
	}

	bool _IsOnSilderValueChange = false;
	public void OnSliderValueChange(Slider slider)
	{
		if(_IsSetColor)
		{
			_IsSetColor = false;
			return;
		}
		_IsOnSilderValueChange = true;

		if(slider == sliderR)
		{
			labelR.SetText(((int)(255 * slider.value)).ToString());
		}
		if(slider == sliderG)
		{
			labelG.SetText(((int)(255 * slider.value)).ToString());
		}
		if(slider == sliderB)
		{
			labelB.SetText(((int)(255 * slider.value)).ToString());
		}
		if(slider == sliderA)
		{
			labelA.SetText(((int)(255 * slider.value)).ToString());
		}

		if(hasAlpha)
		{
			selectedColorDisplay.color = new Color((float)labelR.GetInt()/255f, (float)labelG.GetInt()/255f, (float)labelB.GetInt()/255f, (float)labelA.GetInt()/255f);
		}
		else
		{
			selectedColorDisplay.color = new Color((float)labelR.GetInt()/255f, (float)labelG.GetInt()/255f, (float)labelB.GetInt()/255f, 1f);
		}

		_SetHexCode();
		_CursorVisible(false);
	}

	private void _SetHexCode()
	{
		string hex = (hasAlpha)? _ColorToHex(selectedColorDisplay.color):_ColorToHexWithOutAlpha(selectedColorDisplay.color);
		labelHexCode.SetText(hex);
        hexCode = hex;
        //DPlayerPref.PlayCamLastColorHex = hex;
    }

	private void _CursorVisible(bool isVisible)
	{
		if(appCursor.gameObject.activeSelf != isVisible)
		{
			appCursor.gameObject.SetActive(isVisible);
		}
	}

	private Action _onCloseAction = null;
	public void Close()
	{
		_ClearPickedSampleTexture();
		if(_onCloseAction != null) _onCloseAction();

		Destroy(gameObject, 0.1f);
	}

	#region ----- Common -----
	private float _GetPowerOfTwoFor(float f)
	{
		return Mathf.Log(f, 2);
	}

	private string _ColorToHex(Color32 color)
	{
		string hex = "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
		return hex.ToLower();
	}

	private string _ColorToHexWithOutAlpha(Color32 color)
	{
		string hex = "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex.ToLower();
	}

	private Color _HexToColor(string hex)
	{
		Vector4 v = _HexToVector4(hex);
		return new Color(v.x/255f, v.y/255f, v.z/255f, v.w/255f);
	}

	private Vector4 _HexToVector4(string hex)
	{
		if (hex.StartsWith("#"))
			hex = hex.Substring(1);

		if (hex.Length < 6) throw new Exception("Color not valid");

		int r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		int g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		int b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

		int a = 255;
		if (hex.Length >= 8)
		{
			a = int.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
		}

		return new Vector4(r, g, b, a);
	}

	private void _SaveImageToDataPath(Texture2D texture, string folderName, string fileName, DataPathType dataPathType = DataPathType.PersistentDataPath, bool isThumbnail = false)
	{
		string fullFolderPath = _GetDataPath(dataPathType) + "/" + folderName;
		_SaveImageToPath(texture, fullFolderPath, fileName, isThumbnail);
		Debug.Log("File Saved: " + fullFolderPath);
	}

	private void _SaveImageToPath(Texture2D texture, string fullFolderPath, string fileName, bool isThumbnail = false)
	{
		string fullFilePath = fullFolderPath + "/" + fileName;

		if(!Directory.Exists(fullFolderPath))
		{
			Directory.CreateDirectory(fullFolderPath) ;
		}

		//Encode & Save the image
		if(isThumbnail)
		{
			System.IO.File.WriteAllBytes(fullFilePath, texture.EncodeToJPG()); //no file extension name
		}
		else
		{
			System.IO.File.WriteAllBytes(fullFilePath + ".jpg", texture.EncodeToJPG(90));
		}
	}

	private string _GetDataPath(DataPathType dataPathType)
	{
		string getDataPath = string.Empty;
		switch(dataPathType)
		{
		case DataPathType.DataPath:
			getDataPath = Application.dataPath;
			break;
		case DataPathType.PersistentDataPath:
			getDataPath = Application.persistentDataPath;
			break;
		case DataPathType.StreamingAssetsPath:
			getDataPath = Application.streamingAssetsPath;
			break;
		case DataPathType.TemporaryCachePath:
			getDataPath = Application.temporaryCachePath;
			break;
		}
		return getDataPath;
	}

	public enum DataPathType{
		PersistentDataPath = 0,
		TemporaryCachePath,
		StreamingAssetsPath,
		DataPath,
	}

	#endregion

}
