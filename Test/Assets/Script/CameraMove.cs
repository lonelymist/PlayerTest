using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject CameraOffset;
    public GameObject CameraZoom;
    private RaycastHit CamColliderDetect;
    void FixedUpdate()
    {
        Debug.DrawRay(CameraZoom.transform.position, this.transform.position - CameraZoom.transform.position * 1);
        if(Physics.Raycast(CameraZoom.transform.position, this.transform.position - CameraZoom.transform.position, out CamColliderDetect, 1))
        {

            float distanceOffect = CamColliderDetect.distance / Vector3.Distance(CameraZoom.transform.position, CameraOffset.transform.position);
            if (distanceOffect < 1)
            {

                this.transform.position = Vector3.Lerp(this.transform.position, CameraZoom.transform.position, distanceOffect / 2);
            }
        }
        else
        {
            this.transform.position = CameraOffset.transform.position;
        }
    }
}
