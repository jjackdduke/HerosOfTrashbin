using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    // 발사체 프리팹
    //[SerializeField] GameObject laser;

    // 이동 속도
    [SerializeField] float moveSpeed = 20.0f;


    // 회전 속도
    //[SerializeField] float rotateSpeed = 10.0f;

    Animator anim;
    Rigidbody rigid;

    float h, v;

    GameObject nearObject;
    bool iDown;
    bool fDown;
    bool isFireReady = true;
    bool isSkillReady = true;
    bool skillDown;
    bool isBorder;
    bool isRun;


    Weapon equipWeapon;

    Vector3 moveVec;
    

    float fireDelay;
    float skillDelay = 10f;
    public float skillCoolTime = 7f;

    void Awake()
    {
        
        anim = GetComponent<Animator>();
        equipWeapon = GetComponentInChildren<Weapon>();
        rigid = GetComponent<Rigidbody>();

    }

    void Update()
    {
        
        v = Input.GetAxis("Vertical"); // 앞 뒤 움직임
        h = Input.GetAxis("Horizontal"); // 좌 우 회전
        iDown = Input.GetButtonDown("Interaction");
        fDown = Input.GetButtonDown("Fire1");
        skillDown = Input.GetButtonDown("Fire2");
        isRun = Input.GetKey(KeyCode.LeftShift);

        
        
        
        Attack();
        Move();
        Rotate();

        

    }

    // 이동 관련 함수를 짤 때는 Update보다 FixedUpdate가 더 효율이 좋다고 한다. 그래서 사용했다.
    void FixedUpdate()
    {
        FreezeRotation();
        StopToWallI();
    }

    //void GetInput()
    //{
        
    //    iDown = Input.GetButtonDown("Interaction");
    //    fDown = Input.GetButtonDown("Fire1");
    //    skillDown = Input.GetButtonDown("Fire2");
    //}

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
        if (equipWeapon == null)
            return;

        //fireDelay += Time.deltaTime;
        //isFireReady = equipWeapon.rate < fireDelay;

        skillDelay += Time.deltaTime;
        isSkillReady = skillCoolTime < skillDelay;

        if (fDown)
        {

            equipWeapon.Use();
            anim.SetBool("IsSwing", true);
            anim.SetTrigger("sword_combo");




            //fireDelay = 0;
        }

        if (skillDown && isSkillReady && !fDown)
        {
            equipWeapon.UseSkill();
            anim.SetTrigger("Whirlwind");
            skillDelay = 0;
        }

        anim.SetBool("IsSwing", false);

    }

    void Move()
    {
        if (fDown) return;

        float local_moveSpeed = moveSpeed;

        moveVec = new Vector3(h, 0, v);


        //if (!isFireReady)
        //    moveVec = Vector3.zero;

        if (fDown)
            moveVec = Vector3.zero;

        if (isRun)
            local_moveSpeed = local_moveSpeed * 4.0f;

        if(!isBorder)
            transform.position += moveVec * local_moveSpeed * Time.deltaTime;





        anim.SetBool("IsMove", moveVec != Vector3.zero);




    }

    void Rotate()
    {

        transform.LookAt(transform.position + moveVec);

    }

    


}
