using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSelection : MonoBehaviour {

    public Texture marqueeGraphics;
    private Vector2 marqueeOrigin;
    private Vector2 marqueeSize;
    public Rect marqueeRect;
    public List<GameObject> SelectableUnits;
    private Rect backupRect;
    public LayerMask buttonMask;
    public Dictionary<GameObject, GameObject> sDict = new Dictionary<GameObject, GameObject>();
    public List<GameObject> sUnits;
    public FaceManager[] units;

    private void OnGUI()
    {
        marqueeRect = new Rect(marqueeOrigin.x, marqueeOrigin.y, marqueeSize.x, marqueeSize.y);
        GUI.color = new Color(0, 0, 0, .3f);
        if(marqueeGraphics != null)
        GUI.DrawTexture(marqueeRect, marqueeGraphics);
    }
    void Update()
    {

        if(Input.GetKey(KeyCode.LeftAlt))
            return;

        if (Input.GetMouseButtonDown(0))
        {
            //Poppulate the selectableUnits array with all the selectable units that exist
            SelectableUnits = new List<GameObject>();
            units = GameObject.FindObjectsOfType<FaceManager>();
            
            /*
            sDict = new Dictionary<GameObject, GameObject>();
            sUnits.Clear();

            for (int i = 0; i < SelectableUnits.Count; i++)
            {
                if (!sDict.ContainsKey(SelectableUnits[i].transform.parent.gameObject))
                {
                    sDict.Add(SelectableUnits[i].transform.parent.gameObject,
                        SelectableUnits[i].transform.parent.gameObject);
                    sUnits.Add(SelectableUnits[i].transform.parent.gameObject);
                }
            }
            */

            float _invertedY = Screen.height - Input.mousePosition.y;
            marqueeOrigin = new Vector2(Input.mousePosition.x, _invertedY);

            //Check if the player just wants to select a single unit opposed to drawing a marquee and selecting a range of units
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                SelectableUnits.Remove(hit.transform.gameObject);
                hit.transform.gameObject.SendMessage("OnSelected", SendMessageOptions.DontRequireReceiver);
            }
        }


        if (Input.GetMouseButton(0))
        {
            float _invertedY = Screen.height - Input.mousePosition.y;
            marqueeSize = new Vector2(Input.mousePosition.x - marqueeOrigin.x, (marqueeOrigin.y - _invertedY) * -1);
            //FIX FOR RECT.CONTAINS NOT ACCEPTING NEGATIVE VALUES
            if (marqueeRect.width < 0)
            {
                backupRect = new Rect(marqueeRect.x - Mathf.Abs(marqueeRect.width), marqueeRect.y, Mathf.Abs(marqueeRect.width), marqueeRect.height);
            }
            else if (marqueeRect.height < 0)
            {
                backupRect = new Rect(marqueeRect.x, marqueeRect.y - Mathf.Abs(marqueeRect.height), marqueeRect.width, Mathf.Abs(marqueeRect.height));
            }
            if (marqueeRect.width < 0 && marqueeRect.height < 0)
            {
                backupRect = new Rect(marqueeRect.x - Mathf.Abs(marqueeRect.width), marqueeRect.y - Mathf.Abs(marqueeRect.height), Mathf.Abs(marqueeRect.width), Mathf.Abs(marqueeRect.height));
            }

        }

        if (Input.GetMouseButtonUp(0))
        {

            for (int i = 0; i < units.Length; i++)
            {

                //Convert the world position of the unit to a screen position and then to a GUI point
                Vector3 _screenPos = Camera.main.WorldToScreenPoint(units[i].transform.position);
                Vector2 _screenPoint = new Vector2(_screenPos.x, Screen.height - _screenPos.y);
                //Ensure that any units not within the marquee are currently unselected
                if (!marqueeRect.Contains(_screenPoint) || !backupRect.Contains(_screenPoint))
                {
                    //unselected
                    units[i].UnHighlight();
                }
                if (marqueeRect.Contains(_screenPoint) || backupRect.Contains(_screenPoint))
                {
                    //selected
                    units[i].Highlight();
                    //Add inside selection
                    MainController.singletonInstance.selectionController.Add(units[i].transform);
                }

            }
            MainController.singletonInstance.selectionController.SetPivotPoint();

        }

        if (Input.GetMouseButtonUp(0))
        {
            //Reset the marquee so it no longer appears on the screen.
            marqueeRect.width = 0;
            marqueeRect.height = 0;
            backupRect.width = 0;
            backupRect.height = 0;
            marqueeSize = Vector2.zero;

        }
    }
}
