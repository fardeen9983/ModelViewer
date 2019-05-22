using UnityEngine;

public class ObjectActionScript : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 screenPoint;
    public GameObject selectObj;
    private Ray ray;
    private RaycastHit hit;
    private static int ObjCount;
    public GameObject InstantiateOBJ;
    public Canvas canvas;
  

    private bool allowRotate;
    private bool allowInstantiate;
    private bool allowTranslate;
    private bool allowScale;

    private bool showPanel;
    public void setSelectedObject(GameObject obj)
    {
        selectObj = obj;
        allowRotate = allowInstantiate = allowScale = allowTranslate = showPanel = false;
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && allowInstantiate == true ) { 
                
                ray = Camera.main.ScreenPointToRay(touch.position);


                /*PointerEventData pointer = new PointerEventData(null);
                pointer.position = touch.position;
                List<RaycastResult> result = new List<RaycastResult>();
                hit.Raycast(pointer, result);*/

                //if (result.Count == 0)
                //{
                    if (Physics.Raycast(ray, out hit) && ObjCount < 5)
                    {
                        var temp = Instantiate(InstantiateOBJ, hit.point, Quaternion.identity);
                        //temp.AddComponent<SelectObjectScript>();
                        temp.name = "Element_" + temp.name;
                        ObjCount++;
                    }
               // }
            }
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
            if (touch.phase == TouchPhase.Moved && selectObj!=null && allowTranslate == true) { 
                Vector3 screenPoint = Camera.main.WorldToScreenPoint(selectObj.transform.position);
                if (! (touch.position.y < 0))
                {
                    Vector3 offset = selectObj.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, screenPoint.z));
                    Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, screenPoint.z));
                    selectObj.transform.position = Vector3.Lerp(selectObj.transform.position, touchPos, 10 * Time.deltaTime);
                }
            }
            if(allowRotate == true)
            {

            }
            if(allowScale == true)
            {

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
        }
    }

    public void showHidePanel()
    {
        canvas.GetComponentInChildren<RectTransform>().gameObject.SetActive(showPanel);
        showPanel = !showPanel;
    }
}
