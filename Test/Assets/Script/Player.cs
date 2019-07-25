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
    [Header("可不可以回復SP")]
    public bool CountSp;
    [Header("SPUI")]
    public SpUI spui;
    void Update()
    {
        if (!LookTarget)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                DistanceOffset = Vector3.Distance(Target.transform.position, transform.position);
                LookTarget = true;
            }
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                animator.SetBool("isWalking",true);
                Dodge();
                if (!OnAction)
                {
                    transform.rotation = Quaternion.Euler(0, CamScript.x, 0);
                    transform.Translate(Input.GetAxis("Horizontal") * Speed * Time.deltaTime, 0, Input.GetAxis("Vertical") * Speed * Time.deltaTime);
                }
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
        else if (LookTarget)
        {
            dis = DistanceOffset / Vector3.Distance(Target.transform.position, transform.position);
            if (Input.GetKeyDown(KeyCode.L))
            {
                animator.SetBool("IsWalkingTarget", false);
                LookTarget = false;
            }
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 )
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("IsWalkingTarget", true);
                animator.SetFloat("Walkx", Input.GetAxis("Horizontal"));
                Dodge();
                if (!OnAction)
                {
                    transform.LookAt(Target.transform.position);
                    transform.RotateAround(Target.transform.position, Vector3.down, Input.GetAxis("Horizontal") * Speed * 10 * Time.deltaTime * dis);
                    transform.Translate(0, 0, Input.GetAxis("Vertical") * Speed * Time.deltaTime / 2);
                }
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("IsWalkingTarget", false);

            }
        }
       
       
        if (Input.GetMouseButtonDown(0) && OnAction == false && PlayerSP >= 40)
        {
            CheckCombo = false;
            PlayerSP -= 40;
            OnAction = true;
            Sword.isTrigger = true;
            animator.SetBool("LightAttack",true);
            spui.CoSp();
        }
        if (Input.GetMouseButtonDown(1) && OnAction == false && PlayerSP >= 40)
        {
            CheckCombo = false;
            PlayerSP -= 40;
            OnAction = true;
            Sword.isTrigger = true;
            animator.SetBool("HeavyAttack", true);
            spui.CoSp();
        }
        if (CheckCombo)
        {
            if (Input.GetMouseButtonDown(0) && PlayerSP >= 40)
            {
                PlayerSP -= 40;
                animator.SetBool("NoCombo",false);
                CheckCombo = false;
                spui.CoSp();
            }           
        }
        if (CheckHeavyCombo)
        {
            if (Input.GetMouseButtonDown(1) && PlayerSP >= 40)
            {
                PlayerSP -= 40;
                animator.SetBool("NoCombo", false);
                CheckHeavyCombo = false;
                spui.CoSp();
            }
        }
    }

    private void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !OnAction && PlayerSP >= 40)
        {
            PlayerSP -= 40;
            OnAction = true;
            animator.SetFloat("x", Input.GetAxis("Horizontal"));
            animator.SetFloat("y", Input.GetAxis("Vertical"));
            animator.SetTrigger("Dodge");
            spui.CoSp();
        }
    }
    public void notOnAction()
    {
        OnAction = false;
        animator.SetBool("NoCombo", false);
    }
    public void attackEnd()
    {
        Sword.isTrigger = false;
    }
    public void CheckComboStart()
    {
        animator.SetBool("LightAttack", false);
        CheckCombo = true;
    }
    public void CheckHeavyComboStart()
    {
        animator.SetBool("HeavyAttack", false);
        CheckHeavyCombo = true;
        CountSp = true;
    }
    public void CheckHeavyComboEnd()
    {
        if(CheckHeavyCombo == true)
        {
            CheckHeavyCombo = false;
            animator.SetBool("NoCombo", true);
            OnAction = false;
        }
    }
    public void CheckComboEnd()
    {
        if (CheckCombo == true)
        {
            CheckCombo = false;
            animator.SetBool("NoCombo", true);
            OnAction = false;
        }
    }
    public void AllEnd()
    {
        animator.SetBool("NoCombo", true);
        animator.SetBool("LightAttack", false);
        animator.SetBool("HeavyAttack", false);
    }
    public void Weapon()
    {
        Sword.isTrigger = false;
    }
    public void Dead()
    {
        OnAction = true;
        animator.SetBool("IsDead", true);
    }
    public void GetHit()
    {
        animator.SetBool("IsHurt", true);
    }
    public void Hurt()
    {
        animator.SetBool("IsHurt", false);
    }
}