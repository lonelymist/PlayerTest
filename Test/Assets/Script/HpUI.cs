using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpUI : MonoBehaviour
{
    //最大血量
    [Header("最大血量")]
    [SerializeField]
    private int maxHealth = 400;
    //設置一個有矩形的位置大小等訊息之物件
    public RectTransform HealthBar, HurtBar;
    //血量刷新量
    [Header("血量刷新量")]
    [SerializeField]
    private int HpSpeed = 40;
    //根據hpspeed減少的血量
    private Vector2 HpBar;
    //比較慢的漸近血量
    private Vector2 SlowBar;
    //開始血量
    private Vector2 iniBar;
    //0
    private Vector2 zero;
    //是否可以回血
    private bool StartHealth;
    //玩家
    public Player player;
    void Start()
    {
        iniBar = new Vector2(maxHealth, HealthBar.sizeDelta.y);
        SlowBar = new Vector2(Time.deltaTime * HpSpeed, 0);
        zero = new Vector2(0, HealthBar.sizeDelta.y);
        HpBar = new Vector2(HpSpeed, 0);
        player.PlayerHP = maxHealth;
    }
    void Update()
    {
        //如果按下R並且血量條的size>0
        if (Input.GetKeyDown(KeyCode.R) && HealthBar.sizeDelta.x > 0)
        {
            //受傷
            player.GetHit();
            //扣除一次傷害的size
            HealthBar.sizeDelta -= HpBar;
            //生命值扣除一定傷害
            player.PlayerHP -= HpSpeed;
            //開始回血關閉
            StartHealth = false;
        }
        if (Input.GetKeyDown(KeyCode.T)&& HealthBar.sizeDelta.x == 0)
        {

            //回到原始血量
            HealthBar.sizeDelta = iniBar;
            player.Revive();
            //開始回血關閉
            StartHealth = true;
        }
        //如果紅條>綠條
        if (HurtBar.sizeDelta.x > HealthBar.sizeDelta.x)
        {
            //慢慢地漸進跟上
            HurtBar.sizeDelta -= SlowBar * 2;
        }
        //如果綠條<=紅條
        else if(HurtBar.sizeDelta.x <= HealthBar.sizeDelta.x)
        {
            //兩個相等
            HurtBar.sizeDelta = HealthBar.sizeDelta;
            //玩家血量等於紅條
            player.PlayerHP = HealthBar.sizeDelta.x;
            //開始回血
            StartHealth = true;
        }
        //如果綠血>0
        if (HealthBar.sizeDelta.x > 0)
        {
            //如果當前血量小餘最大血量且可以回血
            if (HealthBar.sizeDelta.x < maxHealth && StartHealth)
            {
                //慢慢地回血
                HealthBar.sizeDelta += SlowBar;
                //角色也慢慢回血
                player.PlayerHP = HealthBar.sizeDelta.x;
            }
            //如果當前血量>最大血量
            else if (HealthBar.sizeDelta.x > maxHealth)
            {
                //回到原始狀態
                HealthBar.sizeDelta = iniBar;
                //不能回血
                StartHealth = false;
            }
        }
        //目前血量<=0
        else if(HealthBar.sizeDelta.x <= 0)
        {
            //歸零
            HealthBar.sizeDelta = zero;
            player.PlayerHP = 0;
            //玩家死亡
            player.Dead();
        }
    }
}