using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("移動速度")]
    [SerializeField]
    private float Speed = 2;
    [Header("目標")]
    public GameObject Target;
    [Header("目前鎖定距離倍率")]
    [SerializeField]
    private float dis;
    [Header("鎖定距離標準")]
    [SerializeField]
    private float DistanceOffset;
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
    [SerializeField]
    private bool CheckCombo;
    [Header("重攻擊COMBO")]
    [SerializeField]
    private bool CheckHeavyCombo;
    [Header("血量")]
    public float PlayerHP;
    [Header("耐力條")]
    public float PlayerSP;
    [Header("SPUI")]
    public SpUI spui;
    void FixedUpdate()
    {
        //如果沒鎖定
        if (Target == null)
        {
            //如果按下L
            if (Input.GetKeyDown(KeyCode.L))
            {
                CamScript.DetectEnemy();
                Target = CamScript.SubTarget;
                //紀錄距離
                if(Target != null)
                {
                    DistanceOffset = Vector3.Distance(Target.transform.position, transform.position);
                }
                //變成鎖定
            }
            //如果有水平垂直輸入
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {

                //有在走路
                animator.SetBool("isWalking",true);
                
                //判斷有沒有閃避
                Dodge();

                //如果沒有其他動作
                if (!OnAction)
                {
                    //鎖定Y軸
                    Cam.transform.eulerAngles = new Vector3(Cam.transform.eulerAngles.x, Cam.transform.eulerAngles.y, 0);
                    //抓取四個象限得方向
                    float MoveX = Input.GetAxis("Vertical") * Cam.transform.forward.x + Input.GetAxis("Horizontal") * Cam.transform.right.x;
                    float MoveZ = Input.GetAxis("Vertical") * Cam.transform.forward.z + Input.GetAxis("Horizontal") * Cam.transform.right.z;
                    Vector3 Dir = new Vector3(MoveX, 0, MoveZ);
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
        else if (Target != null)
        {
            //隨時跟新目標距離，計算跟原本的差距
            dis = DistanceOffset / Vector3.Distance(Target.transform.position, transform.position);
            //如果按下L
            if (Input.GetKeyDown(KeyCode.L))
            {
                CamScript.inViewTarget.Clear();
                CamScript.SpottedEnemies = null;
                CamScript.SubTarget = null;
                Target = null;
                //沒有走路
                animator.SetBool("IsWalkingTarget", false);
                //鎖定無效
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
        if (Input.GetMouseButtonDown(0) && !OnAction && PlayerSP >= 40)
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
        else if (Input.GetMouseButtonDown(1) && !OnAction && PlayerSP >= 40)
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
    /// <summary>
    /// 閃避
    /// </summary>
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
    /// <summary>
    /// 沒有走路之外的動作
    /// </summary>
    public void notOnAction()
    {
        //沒有其他動作
        OnAction = false;
        //沒有連擊無效
        animator.SetBool("NoCombo", false);
    }
    /// <summary>
    /// 攻擊結束
    /// </summary>
    public void attackEnd()
    {
        //把武器變回一般碰撞物件
        Sword.isTrigger = false;
    }
    /// <summary>
    /// 輕攻擊連擊判定開始
    /// </summary>
    public void CheckComboStart()
    {
        //輕攻擊關閉
        animator.SetBool("LightAttack", false);
        //開始檢查是否有下一個連擊
        CheckCombo = true;
       

    }
    /// <summary>
    /// 重攻擊連擊判定開始
    /// </summary>
    public void CheckHeavyComboStart()
    {
        //重攻擊關閉
        animator.SetBool("HeavyAttack", false);
        //開始檢查是否有下一個連擊
        CheckHeavyCombo = true;

    }
    /// <summary>
    /// 重攻擊判定結束
    /// </summary>
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
    /// <summary>
    /// 輕攻擊判定結束
    /// </summary>
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
    /// <summary>
    /// 攻擊都結束了
    /// </summary>
    public void AllEnd()
    {
        //沒錯 沒有連擊
        animator.SetBool("NoCombo", true);
        //將兩種攻擊狀態無效
        animator.SetBool("LightAttack", false);
        animator.SetBool("HeavyAttack", false);
    }
    /// <summary>
    /// 武器變成沒有Trigger
    /// </summary>
    public void Weapon()
    {
        //讓武器回到實體狀態
        Sword.isTrigger = false;
    }
    /// <summary>
    /// 死亡
    /// </summary>
    public void Dead()
    {
        //目前有執行動作
        OnAction = true;
        //玩家死亡
        animator.SetBool("IsDead", true);
    }
    /// <summary>
    /// 受傷
    /// </summary>
    public void GetHit()
    {
        //玩家受傷
        animator.SetBool("IsHurt", true);
    }
    /// <summary>
    /// 受傷結束
    /// </summary>
    public void Hurt()
    {
        //關閉
        animator.SetBool("IsHurt", false);
    }
    public void Revive()
    {
       
        animator.SetTrigger("Revive");
        OnAction = false;
    }


}