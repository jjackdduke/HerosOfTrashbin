using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerMover : MonoBehaviour
{
    // 발사체 프리팹
    //[SerializeField] GameObject laser;

    // 이동 속도
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;

    
    // 회전 속도
    //[SerializeField] float rotateSpeed = 10.0f;

    Animator anim;
    Rigidbody rigid;
    SwordPlayer swordPlayerStat;


    float h, v;

    GameObject nearObject;
    bool iDown;
    bool fDown;
    //bool isFireReady = true;
    bool isSkillReady = true;
    bool skillDown;
    bool isBorder;
    bool shiftPressed;
    bool jDown;

    bool isJump;
    bool isRun;

    Weapon equipWeapon;

    Vector3 moveVec;
    

    float fireDelay;
    float skillDelay = 10f;
    public float skillCoolTime = 3f;

    void Start()
    {
        
        anim = GetComponent<Animator>();
        equipWeapon = GetComponentInChildren<Weapon>();
        rigid = GetComponent<Rigidbody>();
        swordPlayerStat = GetComponent<SwordPlayer>();
        jumpPower = 20f;

    }

    void Update()
    {


        GetInput();

        Attack();
        Move();
        Jump();
        Rotate();

        

    }

    // 이동 관련 함수를 짤 때는 Update보다 FixedUpdate가 더 효율이 좋다고 한다. 그래서 사용했다.
    void FixedUpdate()
    {
        FreezeRotation();
        StopToWallI();
    }

    void GetInput()
    {
        v = Input.GetAxis("Vertical"); // 앞 뒤 움직임
        h = Input.GetAxis("Horizontal"); // 좌 우 회전
        shiftPressed = Input.GetKey(KeyCode.LeftShift);
        iDown = Input.GetButtonDown("Interaction");
        fDown = Input.GetButtonDown("Fire1");
        jDown = Input.GetButtonDown("Jump");
        skillDown = Input.GetButtonDown("Fire2");
        
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    void StopToWallI()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("BGobject"));
    }



    void Attack()
    {
        if (equipWeapon == null || isRun)
            return;

        //fireDelay += Time.deltaTime;
        //isFireReady = equipWeapon.rate < fireDelay;

        skillDelay += Time.deltaTime;
        isSkillReady = skillCoolTime < skillDelay;

        if (fDown)
        {
            
            anim.SetTrigger("sword_combo");
            equipWeapon.Use();
            
            //fireDelay = 0;
        }

        if (skillDown && isSkillReady && !fDown)
        {
            anim.SetTrigger("Whirlwind");
            anim.SetBool("IsWhirlwind", true);
            equipWeapon.UseSkill();
            skillDelay = 0;
        }

        

    }

    void Move()
    {
        
        if (fDown) return;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Combo1") 
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Combo2")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Combo3"))
        {

            return;
        }


        moveSpeed = swordPlayerStat.CurrentSpeed;
        float local_moveSpeed = moveSpeed;

        moveVec = new Vector3(h, 0, v);


        //if (!isFireReady)
        //    moveVec = Vector3.zero;

        if (fDown)
            moveVec = Vector3.zero;

        if (shiftPressed)
        {
            local_moveSpeed = local_moveSpeed * 4.0f;
            isRun = true;
        }
        else
        {
            isRun = false;
        }
        

        if (!isBorder)
            transform.position += moveVec * local_moveSpeed * Time.deltaTime;

        
        anim.SetBool("IsMove", moveVec != Vector3.zero);
        anim.SetBool("IsRun", isRun);
    }

    void Rotate()
    {

        transform.LookAt(transform.position + moveVec);

    }

    void Jump()
    {
        if (jDown && !isJump)
        {
            
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            //anim.SetTrigger("IsJump");
            
            
            isJump = true;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            
            //anim.SetTrigger("IsLand");
            anim.SetBool("isJump", false);
            
            
            isJump = false;
        }
    }



}
