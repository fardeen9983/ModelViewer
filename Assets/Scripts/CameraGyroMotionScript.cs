using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGyroMotionScript : MonoBehaviour
{
    Gyroscope gyro;
    GameObject CameraContainer;
    Quaternion rotationLimit;
    // Start is called before the first frame update
    void Start()
    {
        CameraContainer = new GameObject("Camera Container");
        CameraContainer.transform.position = transform.position;
        transform.SetParent(CameraContainer.transform);
        gyro = EnableGyroScope();
    }

    // Update is called once per frame
    void Update()
    {
        if (gyro != null)
        {
            transform.localRotation = gyro.attitude * rotationLimit;
        }

        if(Input.touchCount == 2)
        {
            transform.position = CameraContainer.transform.position;
        }
    }

    private Gyroscope EnableGyroScope()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Gyroscope temp = Input.gyro;
            temp.enabled = true;
            CameraContainer.transform.rotation = Quaternion.Euler(90f, -90f, 0f);
            rotationLimit = new Quaternion(0, 0, 1, 0);
            return temp;
        }
        return null; 
    }


}
