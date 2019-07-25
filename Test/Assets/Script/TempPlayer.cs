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
    public float PlayerHP;
    public float PlayerSP;
    //Origin
    //更動區
    private Transform CamDir;
    private void Start()
    {
        CamDir = GameObject.Find("CameraDir").transform;
    }
    //更動區

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
                //更動區
                Transform BreakY = CamDir;
                BreakY.eulerAngles = new Vector3(0, CamDir.eulerAngles.y, 0);  //利用角度讓Transform的前方和右方維持在XZ平面
                float MoveX = Input.GetAxis("Vertical") * BreakY.forward.x + Input.GetAxis("Horizontal") * BreakY.right.x;
                float MoveZ = Input.GetAxis("Vertical") * BreakY.forward.z + Input.GetAxis("Horizontal") * BreakY.right.z;
                Vector3 Dir = new Vector3(MoveX, 0, MoveZ);
                Debug.Log(Dir);
                transform.LookAt(transform.position - Dir);
                //更動區
                animator.SetBool("isWalking",true);
                Dodge();
                if (!OnAction)
                {
                    //更動區
                    transform.position = transform.position - Dir * Speed * Time.deltaTime;
                    //更動區
                    //transform.Translate( 0, 0, Input.GetAxis("Vertical") * Speed * Time.deltaTime);
                }
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
            /*if (Input.GetKey(KeyCode.Q))
            {
                transform.Rotate(0, -RotateSpeed * Time.deltaTime, 0);
                CamPos.transform.Rotate(0, -RotateSpeed * Time.deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.E))
            {
                transform.Rotate(0, RotateSpeed * Time.deltaTime, 0);
                CamPos.transform.Rotate(0, RotateSpeed * Time.deltaTime, 0);
            }*/
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
              
        if (Input.GetMouseButtonDown(0) && OnAction == false && PlayerSP>=40)
        {
            OnAction = true;
            Sword.isTrigger = true;
            animator.SetBool("LightAttack",true);
        }
        if (Input.GetMouseButtonDown(1) && OnAction == false && PlayerSP >= 40)
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
    }

    private void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !OnAction && PlayerSP >= 40)
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