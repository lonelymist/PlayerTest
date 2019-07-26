using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// 如果武器碰到其他東西
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //有tag是target
        if (other.gameObject.CompareTag("target"))
        {
            Debug.Log("再打大力一點啊");
        }
    }
}
