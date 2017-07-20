using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
	public static PlayerController _instance;
    public ColorPickerController colorPicker;
    public ScaleManager scaleManager;
	public Transform blockGroup;
	public SimpleSmoothMouseLook cameraSmooth;
	public Slot selectedSlot;
	public LineRenderer lineRenderer;
	public GameObject blockPrefab;
	public Transform myCamera;
	public Transform aimer;
	public Transform hitTransform;
	public Color groundColor;
	public float moveSpeed = 2;
	public bool hasGravity;
	public bool isSpawnOnFace;
	public Text textGravity, textMouseLock, textSpawnType;
	public InputField inputField;

    public List<FaceInfo> selectedFaces;
    public Dictionary<FaceInfo, FaceInfo> selectedFaceDictionary = new Dictionary<FaceInfo, FaceInfo>();

    public List<GameObject> selectedList;
    public Dictionary<GameObject, Vector3> selectedDictionary = new Dictionary<GameObject, Vector3>();


    public GameObject moveGroup;
    public Transform objectToMove;
    public Vector3 firstPressedPos;
    public Vector3 firstGroupPos;
    public Transform selectionGroup;
    public Transform selectedObject;
    public bool isInEditMode;
    void Awake ()
	{
		groundColor = Color.clear;

		textGravity.text = "Gravity: " + hasGravity;

	}

	public void SelectSlot (int i)
	{

		if (selectedSlot != null)
			selectedSlot.HighlightObject (false);

		selectedSlot = SlotManager.singletonInstance.slots [i];

		selectedSlot.HighlightObject (true);

	}

	public void SelectSlot (Slot slot)
	{

		if (selectedSlot != null)
			selectedSlot.HighlightObject (false);

		selectedSlot = slot;

		selectedSlot.HighlightObject (true);

	}

    public void QuickSelect() {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Select number 1
            SelectSlot(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSlot(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSlot(4);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            hasGravity = !hasGravity;
            textGravity.text = "Gravity: " + hasGravity;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            SelectSlot(5);
        }

    }

    public void ToggleEditMode() {
        isInEditMode = !isInEditMode;

        if (isInEditMode)
        {
            SetEditMode(true);
        }
        else {
            SetEditMode(false);
        }

    }

    public void SetEditMode(bool b)
    {
        cameraSmooth.lockCursor = !b;
        cameraSmooth.isFreezeLook = b;
    }

    public void ApplyScale() {
        selectionGroup.localScale = Vector3.one * scaleManager.slider.value;
        scaleManager.ToggleGroup();
    }

    public void CancelScale() {
        scaleManager.ToggleGroup();
    }

    public void ToggleMove() {
        moveGroup.SetActive(!moveGroup.activeInHierarchy);

        if (moveGroup.activeInHierarchy)
        {
            AddGroup();
            selectionGroup.SetParent(moveGroup.transform);
            //make sure you look orientation
            isInEditMode = true;
        }
        else {
            isInEditMode = false;
            ResetGroup();
        }
    }

    public void ToggleMoveOff() {
        moveGroup.SetActive(false);
    }

    public void AddGroup()
    {

        selectionGroup.localScale = Vector3.one;

        for (int i = 0; i < selectedList.Count; i++)
        {
            selectedList[i].transform.SetParent(selectionGroup);
        }

    }
    public void ResetGroup() {

        for (int i = 0; i < selectedList.Count; i++)
        {
            selectedList[i].transform.SetParent(blockGroup);
        }

        selectionGroup.SetParent(null);
    }

    // Update is called once per frame
    void Update ()
	{

        QuickSelect();

        Vector2 inputDir = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
		Vector3 moveDir = myCamera.forward * inputDir.y;

		//To ground it
		if (hasGravity) {
			moveDir.y = 0;
			GetComponent<Rigidbody> ().useGravity = true;
		} else {
			GetComponent<Rigidbody> ().useGravity = false;
		}



        if (isInEditMode) {

            moveDir = Vector3.zero;
            moveDir += Vector3.up * inputDir.y;

        }

        if (inputDir.x != null)
        {
            moveDir += myCamera.right * inputDir.x;
        }

        transform.position += moveDir * Time.deltaTime * moveSpeed;


        if (Input.GetKey(KeyCode.LeftAlt))
            return;


        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Physics.Raycast (myCamera.position, myCamera.forward, out hit, Mathf.Infinity)) {

        if (selectedObject != null) {


        }

        if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {

            if (hit.transform.GetComponent<MoveInfo>())
            {

                if (Input.GetMouseButtonDown(0))
                {

                    selectionGroup.SetParent(hit.transform);

                    //place the objerct inside move handle
                    if (selectedList.Count > 0)
                    {

                        for (int i = 0; i < selectedList.Count; i++)
                        {

                        }

                    }

                    firstPressedPos = hit.point;
                    firstGroupPos = hit.transform.parent.parent.position;

                }

                if (Input.GetMouseButton(0))
                {
                    Vector3 movedDist = hit.point - firstPressedPos;

                    if (hit.transform.name == "Y")
                    {
                        movedDist.x = 0;
                        movedDist.z = 0;
                    }


                    if (hit.transform.name == "X")
                    {
                        movedDist.y = 0;
                        movedDist.z = 0;

                    }

                    if (hit.transform.name == "Z")
                    {
                        movedDist.x = 0;
                        movedDist.y = 0;
                    
                    }

                    selectedObject = hit.transform.parent.parent;
                    hit.transform.parent.parent.position = firstGroupPos + movedDist;

                }

                return;
            }
            else {
                //cancel move
                if (Input.GetMouseButtonDown(0))
                {
                    moveGroup.SetActive(false);
                    ResetGroup();
                }
            }

			if (hit.transform == transform)
				return;

            Debug.DrawLine(myCamera.position, hit.point, Color.red);

            //Unhighlight
            if (hitTransform != null) {
                FaceInfo lastFaceInfo = hitTransform.GetComponent<FaceInfo>();

                if (hitTransform != hit.transform && lastFaceInfo != null) {
                    if(!selectedFaceDictionary.ContainsKey(lastFaceInfo))
                       lastFaceInfo.UnHighlight();
                }
			}


            FaceInfo faceInfo = hit.transform.GetComponent<FaceInfo>();

            if (faceInfo) {
                faceInfo.Highlight(Color.blue);
            }

			aimer.position = hit.point;
			hitTransform = hit.transform;
			Transform hitParent = hit.transform.parent;

            #region MouseDown
            if (Input.GetMouseButton(0))
            {

                switch (selectedSlot.slotInfo.itemType) {

                    case ItemType.paintBucket:
                        faceInfo.SetColor(colorPicker.GetSelectedColor());

                        for (int i = 0; i < selectedFaces.Count; i++)
                        {
                            selectedFaces[i].SetColor(colorPicker.GetSelectedColor());
                        }

                        ClearSelection();

                        break;
                    case ItemType.eyeDropper:
                        colorPicker.SetSelectedColor(faceInfo.myColor);
                        //Paint where ever you click.
                        break;

                    case ItemType.select:
                        //dictionary

                        //Draw selection
                        if (Input.GetKey(KeyCode.LeftShift))
                        {


                        }

                        if (Input.GetKey(KeyCode.LeftAlt))
                        {

                        }

                            //Add
                            if (Input.GetKey(KeyCode.LeftShift))
                        {
                            if (!selectedFaceDictionary.ContainsKey(faceInfo))
                            {

                                faceInfo.Highlight(Color.blue);
                                selectedFaces.Add(faceInfo);
                                selectedFaceDictionary.Add(faceInfo, faceInfo);

                            }

                            if (!selectedDictionary.ContainsKey(hitParent.gameObject))
                            {
                                selectedList.Add(hitParent.gameObject);
                                selectedDictionary.Add(hitParent.gameObject, hitParent.position);
 
                            }

                            //Get the center
                            //Find furthest X
                            //Find furthest Z
                            Vector3 lowest = Vector3.zero;
                            Vector3 highest = Vector3.zero;
                            
                            for (int i = 0; i < selectedList.Count; i++) {

                                if (i == 0) {
                                    lowest = selectedList[i].transform.position;
                                    highest = selectedList[i].transform.position;
                                }

                                if (highest.x < selectedList[i].transform.position.x) {
                                    highest.x = selectedList[i].transform.position.x;
                                }

                                if (selectedList[i].transform.position.x < lowest.x)
                                {
                                    lowest.x = selectedList[i].transform.position.x;
                                }


                                if (highest.z < selectedList[i].transform.position.z)
                                {
                                    highest.z = selectedList[i].transform.position.z;
                                }

                                if (selectedList[i].transform.position.z < lowest.z)
                                {
                                    lowest.z = selectedList[i].transform.position.z;
                                }

                            }

                            print("Highest" + highest);
                            print("Lowest" + lowest);

                            Vector3 dir = highest - lowest;
                            dir.Normalize();
                            float dist = Vector3.Distance(highest, lowest);
                            moveGroup.transform.position = highest - (dir * dist/2);
                            selectionGroup.transform.position = highest - (dir * dist / 2);



                        }
                        else
                        {
                            ClearSelection();
                        }

                        break;

                    case ItemType.scale:
                        faceInfo.transform.parent.transform.localScale = Vector3.one * 0.5F;
                        break;

                    case ItemType.move:


                        break;

                }

            }
            #endregion

            #region MouseUp
            if (Input.GetMouseButtonUp (0)) {

				switch (selectedSlot.slotInfo.itemType) {

				case ItemType.block:
                    #region  Block
                    float size = scaleManager.slider.value;

                    GameObject newBlock = Instantiate (blockPrefab);
					newBlock.transform.SetParent (blockGroup);

                    //Set Scale
                    newBlock.transform.localScale = size * Vector3.one;

					//Get editHandleGroup
					if (hitParent == null)
						hitParent = hit.transform;

                    float hitSize = hitParent.transform.localScale.x;

                    //Get the direction from editHandleGroup base to the side of it.
                    Vector3 hitTransformPosition = hit.transform.position;

					if (hit.transform.name != "Top")
						hitTransformPosition.y = hitParent.position.y;

					Vector3 dir = hitTransformPosition - hitParent.position;
                        dir.Normalize();
                    if (hit.transform.name != "Top") {
						dir = dir * 1; //Making 1 basically!

						if (!isSpawnOnFace)
							dir = -dir;
					}


					if (hit.transform.name == "Bottom") {
						dir = new Vector3 (0, -1, 0);
					}

                        float gap = hitSize - size;

                        newBlock.transform.position = hitParent.position;
                        newBlock.transform.position += (dir * (size + gap / 2));

                        float top = gap;
                        float bottom = 0;
                        float middle = gap / 2;
                          
                        newBlock.transform.position += new Vector3(0, middle, 0);

                        //align center?

                        #endregion
                        break;

				case ItemType.axe:
					#region  Axe
					Destroy (hitParent.gameObject);
					#endregion
					break;

                

                }



			}
            #endregion



        }

	}


    public void ClearSelection()
    {

        //Unhighlgiht them
        for (int i = 0; i < selectedFaces.Count; i++)
        {
            selectedFaces[i].UnHighlight();
        }

        selectedFaces.Clear();


        selectedFaceDictionary.Clear();

        //Unhighlgiht them
        for (int i = 0; i < selectedList.Count; i++)
        {
            //Unhighlight all the faces on them
        }

        selectedList.Clear();
        selectedDictionary.Clear();
    }


    public static PlayerController singletonInstance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<PlayerController> ();
			return _instance;
		}
	}
}
