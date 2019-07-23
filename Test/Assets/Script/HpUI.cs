using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpUI : MonoBehaviour
{
    public int maxHealth;
    public RectTransform HealthBar, HurtBar;
    public int HpSpeed;
    private Vector2 HpBar;
    private Vector2 SlowBar;
    private Vector2 iniBar;
    private Vector2 zero;
    private bool StartHealth;
    void Start()
    {
        iniBar = new Vector2(maxHealth, HealthBar.sizeDelta.y);
        SlowBar = new Vector2(Time.deltaTime * HpSpeed, 0);
        zero = new Vector2(0, HealthBar.sizeDelta.y);
        HpBar = new Vector2(HpSpeed, 0);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && HealthBar.sizeDelta.x > 0)
        {
            HealthBar.sizeDelta -= HpBar;
            StartHealth = false;
        }
        if (HurtBar.sizeDelta.x > HealthBar.sizeDelta.x)
        {
            HurtBar.sizeDelta -= SlowBar * 2;
        }
        else if(HurtBar.sizeDelta.x < HealthBar.sizeDelta.x)
        {
            HurtBar.sizeDelta = HealthBar.sizeDelta;
            StartHealth = true;
        }
        if (HealthBar.sizeDelta.x > 0)
        {
            if (HealthBar.sizeDelta.x < maxHealth && StartHealth)
            {
                HealthBar.sizeDelta += SlowBar;
            }
            else if (HealthBar.sizeDelta.x > maxHealth)
            {
                HealthBar.sizeDelta = iniBar;
                StartHealth = false;
            }
        }
        else if(HealthBar.sizeDelta.x < 0)
        {
            HealthBar.sizeDelta = zero;
        }
    }
}