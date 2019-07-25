using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("移動速度")]
    public float Speed;
    [Header("有沒有鎖定")]
    public bool LookTarget;
    [Header("目標")]
    public GameObject Target;
    [Header("目前鎖定距離倍率")]
    public float dis;
    [Header("鎖定距離標準")]
    public float DistanceOffset;
    [Header("開麥拉")]
    public GameObject Cam;
    public CameraController CamScript;
    [Header("動畫")]
    public Animator animator;
    [Header("有沒有做動作")]
    public bool OnAction = false;
    [Header("打擊判定")]
    public Collider Sword;
    [Header("輕攻擊COMBO")]
    public bool CheckCombo;
    [Header("重攻擊COMBO")]
    public bool CheckHeavyCombo;
    [Header("血量")]
    public float PlayerHP;
    [Header("耐力條")]
    public float PlayerSP;
    [Header("SPUI")]
    public SpUI spui;
    void Update()
    {
        //如果沒鎖定
        if (!LookTarget)
        {
            //如果按下L
            if (Input.GetKeyDown(KeyCode.L))
            {
                //紀錄距離
                DistanceOffset = Vector3.Distance(Target.transform.position, transform.position);
                //變成鎖定
                LookTarget = true;
            }
            //如果有水平垂直輸入
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {

                //有在走路
                animator.SetBool("isWalking",true);
                
                Dodge();
                //如果沒有其他動作
                if (!OnAction)
                {
                    //鎖定Y軸
                    Cam.transform.eulerAngles = new Vector3(0, Cam.transform.eulerAngles.y, 0);
                    //抓取四個象限得方向
                    float MoveX;
                    float MoveZ;
                    Vector3 Dir;

                    MoveX = Input.GetAxis("Vertical") * Cam.transform.forward.x + Input.GetAxis("Horizontal") * Cam.transform.forward.z;
                    MoveZ = Input.GetAxis("Vertical") * Cam.transform.forward.z + Input.GetAxis("Horizontal") * Cam.transform.forward.x;
                    Dir = new Vector3(MoveX, 0, MoveZ);
                    transform.LookAt(transform.position + Dir);
                    transform.position = Vector3.Lerp(transform.position, transform.position + Dir, Speed * Time.deltaTime);
                }
            }
            else
            {
                //沒在走
                animator.SetBool("isWalking", false);
            }
        }
        //鎖定
        else if (LookTarget)
        {
            //隨時跟新目標距離，計算跟原本的差距
            dis = DistanceOffset / Vector3.Distance(Target.transform.position, transform.position);
            //如果按下L
            if (Input.GetKeyDown(KeyCode.L))
            {
                //沒有走路
                animator.SetBool("IsWalkingTarget", false);
                //鎖定無效
                LookTarget = false;
            }
            //如果有水平或垂直輸入
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 )
            {

                //有走路
                animator.SetBool("isWalking", true);
                animator.SetBool("IsWalkingTarget", true);
                //抓到他的X輸出方向
                animator.SetFloat("Walkx", Input.GetAxis("Horizontal"));
                Dodge();

                //如果沒其他動作
                if (!OnAction)
                {
                    //面向Target方向
                    transform.LookAt(Target.transform.position);
                    //視角隨水平輸入改變
                    transform.RotateAround(Target.transform.position, Vector3.down, Input.GetAxis("Horizontal") * Speed * 10 * Time.deltaTime * dis);
                    //移動
                    transform.Translate(0, 0, Input.GetAxis("Vertical") * Speed * Time.deltaTime / 2);
                }
            }
            else
            {
                //沒在走路
                animator.SetBool("isWalking", false);
                animator.SetBool("IsWalkingTarget", false);

            }
        }
       
       //按下左鍵且沒其他動作且sp>=40
        if (Input.GetMouseButtonDown(0) && OnAction == false && PlayerSP >= 40)
        {
            //結束確認是否有combo
            CheckCombo = false;
            //sp減少40
            PlayerSP -= 40;
            //正在動作中
            OnAction = true;
            //武器進入trigger
            Sword.isTrigger = true;
            //輕攻擊
            animator.SetBool("LightAttack",true);
            //減少耐力條
            spui.CoSp();
        }
        //按下右鍵且沒其他動作且sp>=40
        else if (Input.GetMouseButtonDown(1) && OnAction == false && PlayerSP >= 40)
        {
            //結束確認是否有combo
            CheckHeavyCombo = false;
            //sp減少40
            PlayerSP -= 40;
            //正在動作中
            OnAction = true;
            //武器進入trigger
            Sword.isTrigger = true;
            //重攻擊
            animator.SetBool("HeavyAttack", true);
            //減少耐力條
            spui.CoSp();
        }
        //如果正在查combo
        if (CheckCombo)
        {
            //如果有新的左鍵輸入且sp>=40

            if (Input.GetMouseButtonDown(0) && PlayerSP >= 40)
            {
                //sp-40
                PlayerSP -= 40;
                //沒有combo無效
                animator.SetBool("NoCombo",false);
                //檢查結束
                CheckCombo = false;
                //耐力條減少
                spui.CoSp();
            }           
        }
        //如果正在查HeavyCombo
        if (CheckHeavyCombo)
        {
            //如果有新的右鍵輸入且sp >= 40
            if (Input.GetMouseButtonDown(1) && PlayerSP >= 40)
            {
                //sp-40
                PlayerSP -= 40;
                //沒有combo無效
                animator.SetBool("NoCombo", false);
                //檢查結束
                CheckHeavyCombo = false;
                //耐力條減少
                spui.CoSp();
            }
        }
    }
    //設定閃避
    private void Dodge()
    {
        //如果有空白鍵輸入且沒有其他動作且SP>=40
        if (Input.GetKeyDown(KeyCode.Space) && !OnAction && PlayerSP >= 40)
        {
            //角色方向轉向攝影機方向
            transform.rotation = Quaternion.Euler(new Vector3(0,Cam.transform.rotation.eulerAngles.y,0));
            //sp-40
            PlayerSP -= 40;
            //有動作正在執行
            OnAction = true;
            //抓取水平及垂直輸入去決定動作的方向
            animator.SetFloat("x", Input.GetAxis("Horizontal"));
            animator.SetFloat("y", Input.GetAxis("Vertical"));
            //執行一次閃避
            animator.SetTrigger("Dodge");
            //減少SP
            spui.CoSp();
        }
    }
    //如果沒有其他動作
    public void notOnAction()
    {
        //沒有其他動作
        OnAction = false;
        //沒有連擊無效
        animator.SetBool("NoCombo", false);
    }
    //攻擊結束
    public void attackEnd()
    {
        //把武器變回一般碰撞物件
        Sword.isTrigger = false;
    }
    //確認輕攻擊連擊開始
    public void CheckComboStart()
    {
        //輕攻擊關閉
        animator.SetBool("LightAttack", false);
        //開始檢查是否有下一個連擊
        CheckCombo = true;
       

    } 
    //確認重攻擊連擊開始
    public void CheckHeavyComboStart()
    {
        //重攻擊關閉
        animator.SetBool("HeavyAttack", false);
        //開始檢查是否有下一個連擊
        CheckHeavyCombo = true;

    }
    //確認重攻擊結束
    public void CheckHeavyComboEnd()
    {
        //如果確認沒被關閉
        if(CheckHeavyCombo == true)
        {
            //關閉他
            CheckHeavyCombo = false;
            //沒錯 沒有連擊
            animator.SetBool("NoCombo", true);
            //現在沒有動作喔
            OnAction = false;
        }
    }
    //確認輕攻擊結束
    public void CheckComboEnd()
    {
        //如果確認沒被關閉
        if (CheckCombo == true)
        {
            //關閉他
            CheckCombo = false;
            //沒錯 沒有連擊
            animator.SetBool("NoCombo", true);
            //現在沒有動作喔
            OnAction = false;
        }
    }
    //攻擊全體結束
    public void AllEnd()
    {
        //沒錯 沒有連擊
        animator.SetBool("NoCombo", true);
        //將兩種攻擊狀態無效
        animator.SetBool("LightAttack", false);
        animator.SetBool("HeavyAttack", false);
    }
    public void Weapon()
    {
        //讓武器回到實體狀態
        Sword.isTrigger = false;
    }
    //如果死了
    public void Dead()
    {
        //目前有執行動作
        OnAction = true;
        //玩家死亡
        animator.SetBool("IsDead", true);
    }
    //如果受傷
    public void GetHit()
    {
        //玩家受傷
        animator.SetBool("IsHurt", true);
    }
    //受傷結束
    public void Hurt()
    {
        //關閉
        animator.SetBool("IsHurt", false);
    }
}