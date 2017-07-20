using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
	public SlotInfo slotInfo;
	public Image image;
	public GameObject highlightObject;

	public void Init ()
	{
		image.sprite = slotInfo.sprite;
	}

	public void HighlightObject (bool b)
	{
		highlightObject.SetActive (b);
	}

	public void OnButtonClicked ()
	{
		//PlayerController.singletonInstance.SelectSlot (GetComponent<Slot> ());
	}
}

public enum ItemType
{
	none,
	block,
	axe,
    paintBucket, //Click anywhere to paint the selected object
    eyeDropper,
    select, //Click anywhere to select the object (and then press c to change its color) //Hold down on this select to select the whole object
    scale,//ryiu
    move,
    rotate
}