using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject CameraOffset;
    public GameObject CameraZoom;
    private RaycastHit CamColliderDetect;
    [SerializeField]
    public 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(CameraZoom.transform.position, this.transform.position - CameraZoom.transform.position * 20);
        if(Physics.Raycast(CameraZoom.transform.position, this.transform.position - CameraZoom.transform.position, out CamColliderDetect, 20))
        {

            float distanceOffect = CamColliderDetect.distance / Vector3.Distance(CameraZoom.transform.position, CameraOffset.transform.position);
            Debug.Log(distanceOffect);
            if (distanceOffect < 1)
            {

                this.transform.position = Vector3.Lerp(this.transform.position,CameraZoom.transform.position,distanceOffect)/*Vector3.Lerp(OriLocalPosition, CloestPosition, 1 - distanceOffect)*/;
            }
        }
        else
        {
            this.transform.position = CameraOffset.transform.position;
        }
    }
}
