using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
	public static SlotManager _instance;
	public GameObject slotPrefab;
	public int slotAmount = 6;

	public Transform slotGroup;
	public List<Slot> slots;
	public List<SlotInfo> slotInfos;
    public Slot selectedSlot;

	void Awake ()
	{
		for (int i = 0; i < slotAmount; i++) {
			GameObject slotGO = Instantiate (slotPrefab);
			slotGO.transform.SetParent (slotGroup);

			//Copy the i of slotInfos
			Slot slot = slotGO.GetComponent<Slot> ();
			slots.Add (slot);

			if (slotInfos.Count - 1 < i) {
				slotInfos.Add (slot.slotInfo);
			}

			slot.slotInfo = slotInfos [i];
			slot.Init ();
		}

        //PlayerController.singletonInstance.SelectSlot (0);
        SelectSlot(0);
	}

    public void SelectSlot(int i)
    {

        if (selectedSlot != null)
            selectedSlot.HighlightObject(false);

        selectedSlot = SlotManager.singletonInstance.slots[i];

        selectedSlot.HighlightObject(true);

    }

    public void SelectSlot(Slot slot)
    {

        if (selectedSlot != null)
            selectedSlot.HighlightObject(false);

        selectedSlot = slot;

        selectedSlot.HighlightObject(true);

    }

    public static SlotManager singletonInstance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<SlotManager> ();
			return _instance;
		}
	}

}

[System.Serializable]
public class SlotInfo
{
	public ItemType itemType;
	public Sprite sprite;
}