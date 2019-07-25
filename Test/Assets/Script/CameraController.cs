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
    //視窗移動速度
    public float xSpeed;
    public float ySpeed;
    //與鎖定目標距離
    public float distance;
    //滾輪速度
    public float disSpeed;
    //視窗最近距離
    public float minDistance;
    //視窗最遠距離
    public float maxDistance;
    [Header("Observation")]
    //角度
    public float x;
    public float y;
    //攝影機最終旋轉位置
    private Quaternion rotationEuler;
    //攝影機位置
    private Vector3 cameraPosition;

    //最後一個執行的Update
    void LateUpdate()
    {
        //位置=角色位置
        transform.position = Player.transform.position;
        //如果按下L
        if (Input.GetKeyDown(KeyCode.L))
        {
            //找到target
            Lock(GameObject.Find("target"));
        }
        //如果沒有目標
        if (SubTarget == null)
        {
            //攝影機移動跟隨滑鼠的X位移
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        }
        else
        {
            //等於目標跟玩家的方向 
            Vector3 EnemyVector = SubTarget.transform.position - Player.transform.position;
            //用兩個方向求得其中間角度
            //Vector3.up=(0,1,0)
            x = Vector3.SignedAngle(Vector3.forward, EnemyVector, Vector3.up);
        }
        //攝影機移動跟隨滑鼠的X位移
        y -= Input.GetAxis("Mouse Y") * xSpeed * Time.deltaTime;
        //如果左右的位移大於一定值拉回360度內
        if (x > 360)
            x -= 360;
        else if (x < 0)
            x += 360;
        //攝影機跟玩家的距離依造滾輪得輸入做距離增減
        distance -= Input.GetAxis("Mouse ScrollWheel") * disSpeed * Time.deltaTime;
        //攝影機跟玩家的距離限定在minDistance, maxDistance之間
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        //旋轉UP
        rotationEuler = Quaternion.Euler(y, x, 0);
        //決定攝影機的位置
        //cameraPosition = rotationEuler * new Vector3(0, 0, -distance) + Player.transform.position;
        transform.rotation = rotationEuler;
        //transform.position = cameraPosition;
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