using UnityEngine;
using System.Collections;

public class ShowColorPickerAtStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AColorPicker picker = AColorPicker.Create(null);
		picker.Setup(null);
	}

}
