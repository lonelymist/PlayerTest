using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player")]
    public Transform Target;
    [Header("Enemy")]
    public Transform SubTarget;
    [Header("parameter")]
    public float xSpeed;
    public float ySpeed;
    public float distance;
    public float disSpeed;
    public float minDistance;
    public float maxDistance;
    [Header("Observation")]
    public float x;
    public float y;

    private Quaternion rotationEuler;
    private Vector3 cameraPosition;
    private GameObject CameraDir;

    private void Awake()
    {
        CameraDir = new GameObject();
        CameraDir.transform.position = Target.position;
        CameraDir.transform.SetParent(this.transform);
        CameraDir.name = "CameraDir";
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraDir.transform.position = Target.position;
        if (Input.GetKeyDown(KeyCode.L))
        {
            Lock(GameObject.Find("target").transform);
        }
        if (SubTarget == null) //一般模式
        {
            //Getting Axis of mouse
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;

        }
        else //鎖定模式
        {
            Vector3 EnemyVector = SubTarget.position - Target.position;
            x = Vector3.SignedAngle(Vector3.forward, EnemyVector, Vector3.up);
        }

        y -= Input.GetAxis("Mouse Y") * xSpeed * Time.deltaTime;
        //0~360 degrees
        if (x > 360)
            x -= 360;
        else if (x < 0)
            x += 360;

        //transform
        distance -= Input.GetAxis("Mouse ScrollWheel") * disSpeed * Time.deltaTime;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        //distance = GameObject.Find("Player").transform.localScale.y * 15;

        /*this.transform.RotateAround(Target.transform.position, Target.transform.up, x);
        this.transform.RotateAround(Target.transform.position, Target.transform.right, y);*/

        rotationEuler = Quaternion.Euler(y, x, 0);
        cameraPosition = rotationEuler * new Vector3(0, 0, -distance) + Target.position;

        transform.rotation = rotationEuler;
        transform.position = cameraPosition;
    }

    private void Lock(Transform LockedObject)
    {
        if (SubTarget == null)
        {
            SubTarget = LockedObject;
        }
        else
        {
            SubTarget = null;
        }
    }
}
