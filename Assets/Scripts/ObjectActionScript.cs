using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class ObjectActionScript : MonoBehaviour
{
    private float angle;
    private Vector3 offset;
    private Vector3 screenPoint;
    private GameObject selectObj;
    private Ray ray;
    private RaycastHit hit;
    private static int ObjCount;
    public GameObject InstantiateOBJ;
    public Canvas canvas;

    private GraphicRaycaster raycaster;

    private bool allowRotate;
    private bool allowInstantiate;
    private bool allowTranslate;
    private bool allowScale;
    private bool showPanel;

    private GameObject showPanelButton, panel;
    private GameObject ToggleInstantiate, ToggleRotate, ToggleTranslate, ToggleScale;
    public Joystick joystick;
    public void setSelectedObject(GameObject obj)
    {
        selectObj = obj;
    }
    private void Start()
    {
        allowRotate = allowInstantiate = allowScale = allowTranslate = showPanel = false;
        showPanelButton = canvas.transform.Find("ShowPanelButton").gameObject;
        panel = canvas.transform.Find("Panel").gameObject;

        raycaster = canvas.GetComponent<GraphicRaycaster>();

        ToggleRotate = panel.transform.Find("Rotate").gameObject;
        ToggleInstantiate = panel.transform.Find("Instantiate").gameObject;
        ToggleScale = panel.transform.Find("Scale").gameObject;
        ToggleTranslate = panel.transform.Find("Translate").gameObject;
  
    }
    private void Update()
    {
        Vector3 translate = new Vector3(10 * joystick.Horizontal * Time.deltaTime, 0,10 * joystick.Vertical * Time.deltaTime);
        transform.Translate(translate);
        if (Input.touchCount >= 0)
        {
            Touch touch = Input.GetTouch(0);
            ray = Camera.main.ScreenPointToRay(touch.position);

            PointerEventData pointer = new PointerEventData(null);
            pointer.position = touch.position;
            List<RaycastResult> result = new List<RaycastResult>();
            raycaster.Raycast(pointer, result);
            if (result.Count == 0)
            {
                if (touch.phase == TouchPhase.Stationary)
                {

                    ray = Camera.main.ScreenPointToRay(touch.position);
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.name.ToUpper().Contains("ELEMENT"))
                        {
                            setSelectedObject(hit.collider.gameObject);
                        }
                    }
                }
                if (touch.phase == TouchPhase.Began && allowInstantiate == true)
                {
                    if (Physics.Raycast(ray, out hit) && ObjCount < 5)
                    {
                        var temp = Instantiate(InstantiateOBJ, hit.point, Quaternion.identity);

                        temp.name = "Element_" + temp.name;
                        ObjCount++;
                    }

                }
                
                if (touch.phase == TouchPhase.Moved && selectObj != null && allowTranslate == true)
                {
                    Vector3 screenPoint = Camera.main.WorldToScreenPoint(selectObj.transform.position);
                    if (!(touch.position.y < 0))
                    {
                        Vector3 offset = selectObj.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, screenPoint.z));
                        Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, screenPoint.z));
                        selectObj.transform.position = Vector3.Lerp(selectObj.transform.position, touchPos, 10 * Time.deltaTime);
                    }
                }
                if (allowRotate == true && touch.phase == TouchPhase.Moved)
                {
                    if (touch.position.y > 0)
                    {
                        angle += touch.deltaPosition.y;
                        selectObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    }
                }
                if (allowScale == true && Input.touchCount == 2)
                {
                    Touch one = Input.GetTouch(0), two = Input.GetTouch(1);
                    Vector3 initScale = new Vector3(1,1,1);
                    float initDist = 0, scaleFactor;
                    if (touch.phase == TouchPhase.Began)
                    {
                       
                        initDist = Vector3.Distance(one.position, two.position);
                        initScale = selectObj.transform.localScale;
                    }
                    else
                    {
                        float curDistance = Vector3.Distance(one.position, two.position);
                        scaleFactor = curDistance / initDist;
                        selectObj.transform.localScale = scaleFactor * initScale;
                    }

                }
            }
        }
    }

    public void setAllowRotate(bool val)
    {
        allowRotate = val;
        if (val)
        {
            allowInstantiate = false;
            allowScale = false;
            allowTranslate = false;

            ToggleInstantiate.GetComponent<Toggle>().isOn = false;
            ToggleScale.GetComponent<Toggle>().isOn = false;
            ToggleTranslate.GetComponent<Toggle>().isOn = false;
        }
    }
    public void setAllowInstantiate(bool val)
    {
        allowInstantiate = val;
        if (val)
        {
            allowRotate = false;
            allowScale = false;
            allowTranslate = false;

            ToggleRotate.GetComponent<Toggle>().isOn = false;
            ToggleScale.GetComponent<Toggle>().isOn = false;
            ToggleTranslate.GetComponent<Toggle>().isOn = false;
 
        }
    }
    public void setAllowTranslate(bool val)
    {
        allowTranslate = val;
        if (val)
        {
            allowRotate = false;
            allowScale = false;
            allowInstantiate = false;

            ToggleInstantiate.GetComponent<Toggle>().isOn = false;
            ToggleScale.GetComponent<Toggle>().isOn = false;
            ToggleRotate.GetComponent<Toggle>().isOn = false;     
        }
    }
    public void setAllowScale(bool val)
    {  
        allowScale = val;
        if (val)
        {
            allowRotate = false;
            allowInstantiate = false;
            allowTranslate = false;

            ToggleInstantiate.GetComponent<Toggle>().isOn = false;
            ToggleRotate.GetComponent<Toggle>().isOn = false;
            ToggleTranslate.GetComponent<Toggle>().isOn = false;
        }
    }

    public void showHidePanel()
    {
        panel.SetActive(showPanel);
        showPanel = !showPanel;
    }
}
