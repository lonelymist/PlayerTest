using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    public float RotateSpeed;
    public bool LookTarget;
    public GameObject Target;
    public float dis;
    public float DistanceOffset;
    public Animator animator;
    public GameObject CamPos;
    public bool OnAction = false;
    public Collider Sword;
    public bool CheckCombo;
    public bool CheckHeavyCombo;
    public bool isDead;
    public float PlayerHP=10;
    void Update()
    {
 
        if (!LookTarget)
        {
            CamPos.transform.position = transform.position;
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
                    transform.Translate(Input.GetAxis("Horizontal") * Speed * Time.deltaTime, 0, Input.GetAxis("Vertical") * Speed * Time.deltaTime);
                }
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(0, -RotateSpeed * Time.deltaTime, 0);
                CamPos.transform.Rotate(0, -RotateSpeed * Time.deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(0, RotateSpeed * Time.deltaTime, 0);
                CamPos.transform.Rotate(0, RotateSpeed * Time.deltaTime, 0);
            }
        }
        else if (LookTarget)
        {
            CamPos.transform.position = transform.position;
            CamPos.transform.LookAt(Target.transform.position);
            dis = DistanceOffset / Vector3.Distance(Target.transform.position, transform.position);
            if (Input.GetKeyDown(KeyCode.L))
            {
                LookTarget = false;
            }
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 )
            {
         
                if (Input.GetAxis("Horizontal") == 0)
                {
                    animator.SetBool("isWalking", true);
                    animator.SetBool("IsWalkingTarget", false);

                }
                if (Input.GetAxis("Horizontal") != 0)
                {
                    animator.SetBool("IsWalkingTarget", true);
                    animator.SetFloat("Walkx", Input.GetAxis("Horizontal"));
                    
                }
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
       
       
        if (Input.GetMouseButtonDown(0) && OnAction == false )
        {
            OnAction = true;
            Sword.isTrigger = true;
            animator.SetBool("LightAttack",true);
        }
        if (Input.GetMouseButtonDown(1) && OnAction == false)
        {
            OnAction = true;
            Sword.isTrigger = true;
            animator.SetBool("HeavyAttack", true);
        }
        if (CheckCombo)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetBool("NoCombo",false);
                CheckCombo = false;
            }           
        }
        if (CheckHeavyCombo)
        {
            if (Input.GetMouseButtonDown(1))
            {
                animator.SetBool("NoCombo", false);
                CheckHeavyCombo = false;
            }
        }

        Hurt();
        Dead();


        if (Input.GetKey(KeyCode.R))
        {
            animator.SetBool("IsHurt", true);
            PlayerHP = PlayerHP - 0.1f;
        }
        if (PlayerHP <= 0)
        {
            PlayerHP = 0;
            isDead = true;
            OnAction = true;

        }
    }

    private void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !OnAction)
        {
            OnAction = true;
            animator.SetFloat("x", Input.GetAxis("Horizontal"));
            animator.SetFloat("y", Input.GetAxis("Vertical"));
            animator.SetTrigger("Dodge");
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
        if (isDead == true)
        {
            animator.SetBool("IsDead", true);
        }
    }
    public void Hurt()
    {
     
            animator.SetBool("IsHurt", false);
        
    }
}