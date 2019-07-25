using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player")]
    public GameObject Player;
    [Header("Enemy")]
    public GameObject SubTarget;
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
    
    void LateUpdate()
    {
        transform.position = Player.transform.position;
        if (Input.GetKeyDown(KeyCode.L))
        {
            Lock(GameObject.Find("target"));
        }
        if (SubTarget == null)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        }
        else
        {
            Vector3 EnemyVector = SubTarget.transform.position - Player.transform.position;
            x = Vector3.SignedAngle(Vector3.forward, EnemyVector, Vector3.up);
        }

        y -= Input.GetAxis("Mouse Y") * xSpeed * Time.deltaTime;
        if (x > 360)
            x -= 360;
        else if (x < 0)
            x += 360;
        distance -= Input.GetAxis("Mouse ScrollWheel") * disSpeed * Time.deltaTime;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        rotationEuler = Quaternion.Euler(y, x, 0);
        cameraPosition = rotationEuler * new Vector3(0, 0, -distance) + Player.transform.position;
        transform.rotation = rotationEuler;
        transform.position = cameraPosition;
    }
    private void Lock(GameObject LockedObject)
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