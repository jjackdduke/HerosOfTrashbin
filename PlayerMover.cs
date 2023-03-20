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
    [SerializeField] float rotateSpeed = 10.0f;

    Animator anim;


    float h, v;

    GameObject nearObject;
    bool iDown;
    bool fDown;
    bool isFireReady = true;

    Weapon equipWeapon;

    Vector3 moveVec;
    

    float fireDelay;

    void Awake()
    {
        
        anim = GetComponent<Animator>();
        equipWeapon = GetComponentInChildren<Weapon>();

    }

    void Update()
    {

        // 발사 메소드 반복호출
        //ProcessFiring();
        GetInput();
        Attack();



    }

    // 이동 관련 함수를 짤 때는 Update보다 FixedUpdate가 더 효율이 좋다고 한다. 그래서 사용했다.
    void FixedUpdate()
    {

        v = Input.GetAxis("Vertical"); // 앞 뒤 움직임
        h = Input.GetAxis("Horizontal"); // 좌 우 회전
        Move();
        Rotate();
        
    }

    void GetInput()
    {
        
        iDown = Input.GetButtonDown("Interaction");
        fDown = Input.GetButtonDown("Fire1");

    }

    void Attack()
    {
        //if (equipWeapon == null)
        //    return;

        //fireDelay += Time.deltaTime;
        //isFireReady = equipWeapon.rate < fireDelay;

        //if(fDown && isFireReady)
        //{
        //    equipWeapon.Use();
        //    anim.SetTrigger("sword_combo");
        //    fireDelay = 0;
        //}

        if (equipWeapon == null)
            return;


        if (fDown)
        {
            equipWeapon.Use();
            anim.SetTrigger("sword_combo");
  
        }
    }

    void Move()
    {




        moveVec = new Vector3(h, 0, v);
        

        if (!isFireReady)
            moveVec = Vector3.zero;

        transform.position += moveVec * moveSpeed * Time.deltaTime;
        
        
        anim.SetBool("IsMove", moveVec != Vector3.zero);


    }

    void Rotate()
    {

        transform.LookAt(transform.position + moveVec);

       

    }

    //void ProcessFiring() {
    //    // 스페이스 누르면 발싸
    //    if (Input.GetButton("Jump")) {
    //        // Debug.Log("Firing");
    //        SetLaserActive(true);
    //    } else {
    //        SetLaserActive(false);
    //    }
    //}

    //void SetLaserActive(bool isActive) 
    //{    
    //    // 파티클 On/Off
    //    var emissionModule = laser.GetComponent<ParticleSystem>().emission;
    //    emissionModule.enabled = isActive;
    //    // if (isActive) 
    //    // {
    //    //     Debug.Log(emissionModule);
    //    //     Debug.Log(emissionModule.enabled);
    //    // }

    //}


}
