using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpUI : MonoBehaviour
{
    public int maxSP;
    public RectTransform SPHealthBar, SPHurtBar;
    public int SPSpeed;
    private Vector2 SPBar;
    private Vector2 SPSlowBar;
    private Vector2 SPiniBar;
    private Vector2 SPzero;
    private bool StartSP;
    public Player playerSP;
    void Start()
    {
        SPiniBar = new Vector2(maxSP, SPHealthBar.sizeDelta.y);
        SPSlowBar = new Vector2(Time.deltaTime * SPSpeed, 0);
        SPzero = new Vector2(0, SPHealthBar.sizeDelta.y);
        SPBar = new Vector2(SPSpeed, 0);
        playerSP.PlayerSP = maxSP;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && SPHealthBar.sizeDelta.x >= 40 && !playerSP.OnAction || Input.GetMouseButtonDown(0) && SPHealthBar.sizeDelta.x >= 40|| Input.GetMouseButtonDown(1) && SPHealthBar.sizeDelta.x >= 40)
        {
           
            SPHealthBar.sizeDelta -= SPBar;
            playerSP.PlayerSP -= SPSpeed;
            StartSP = false;
        }
        if (SPHurtBar.sizeDelta.x > SPHealthBar.sizeDelta.x)
        {
            SPHurtBar.sizeDelta -= SPSlowBar * 2;
        }
        else if (SPHurtBar.sizeDelta.x <= SPHealthBar.sizeDelta.x)
        {
            SPHurtBar.sizeDelta = SPHealthBar.sizeDelta;
            playerSP.PlayerSP = SPHealthBar.sizeDelta.x;
            StartSP = true;
        }
        if (SPHealthBar.sizeDelta.x >= 0)
        {
            if (SPHealthBar.sizeDelta.x < maxSP && StartSP && playerSP.OnAction == false)
            {

                SPHealthBar.sizeDelta += SPSlowBar;
                playerSP.PlayerSP = SPHealthBar.sizeDelta.x;
            }
            else if (SPHealthBar.sizeDelta.x > maxSP)
            {
                SPHealthBar.sizeDelta = SPiniBar;
                StartSP = false;
            }
        }
        else if (SPHealthBar.sizeDelta.x < 0)
        {
            SPHealthBar.sizeDelta = SPzero;
            playerSP.PlayerSP = 0;
            
        }
    }
}