using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSpawnScript : MonoBehaviour
{
    public GameObject obj;
    Ray touchRay;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            touchRay = Camera.main.ScreenPointToRay(touch.position);
            
            if(Physics.Raycast(touchRay,out hit))
            {
                Instantiate(obj, hit.point, Quaternion.identity);
            }
        }
    }
}
