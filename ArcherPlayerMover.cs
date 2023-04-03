using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArcherPlayerMover : MonoBehaviour
{
    // 발사체 프리팹
    //[SerializeField] GameObject laser;

    // 이동 속도
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;


    public Image skillFilter;

    private PlayerStat playerStat;
   

    
    // 회전 속도
    //[SerializeField] float rotateSpeed = 10.0f;

    Animator anim;
    Rigidbody rigid;

    float h, v;


    bool fDown;
    bool isFireReady = true;
    bool isSkillReady = true;
    bool skillDown;
    bool isBorder;
    bool shiftPressed;
    bool jDown;

    bool isJump;
    bool isRun;

    Weapon equipWeapon;

    Vector3 moveVec;


    float fireDelay = 1000f; // 이 값은 큰값으로만 정하면 된다.
                              // 공격속도는 Weapon에서 조절  
                                
    float skillDelay = 10f;
    int shotCnt = 10;
    public float skillCoolTime = 4f; // 스킬 쿨타임 정하는 곳
    public TextMeshProUGUI coolTimeCounter;

    private float currentCoolTime;


    // 오디오
    AudioSource audioSource;
    public AudioClip audioShoot;
    public AudioClip audioArrowSkill;
    public AudioClip audioWalk;
    public AudioClip audioRun;

    void Start()
    {
        
        anim = GetComponent<Animator>();
        equipWeapon = GetComponentInChildren<Weapon>();
        rigid = GetComponent<Rigidbody>();
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
        jumpPower = 20f;
        skillFilter.fillAmount = 0;
        audioSource = GetComponent<AudioSource>();


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
        if (equipWeapon == null || isRun || anim.GetBool("IsMultiShot"))
            return;
        
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        skillDelay += Time.deltaTime;
        isSkillReady = skillCoolTime <= skillDelay;

        if (fDown && isFireReady)
        {
            anim.SetBool("IsFire", true);
            if(equipWeapon.type == Weapon.Type.Range)
            {
                equipWeapon.Use();
            }
            
            fireDelay = 0;
        }

        if (skillDown && isSkillReady && !fDown)
        {
            shotCnt = 10;
            skillFilter.fillAmount = 1;
            anim.SetTrigger("MultiShot");
            anim.SetBool("IsMultiShot", true);
            equipWeapon.UseSkill();
        }

    }

    void Move()
    {

        if (fDown) return;
        

        //anim.SetBool("IsFire", false);

        moveSpeed = playerStat.CurrentStatus(2);
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

            anim.SetBool("IsFire", false);



            isJump = true;
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            isJump = false;
        }
    }

    public void ArrowFire()
    {
        audioSource.PlayOneShot(audioShoot);
        equipWeapon.fireAnimation();
        
    }

    public void AnimEvent_FireEnd()
    {
        anim.SetBool("IsFire", false);
        equipWeapon.fireEndAnimation();
    }



    public void MultiShotAnim_Start()
    {
        audioSource.PlayOneShot(audioArrowSkill);
    }


    public void MultiShotAnim_End(int cnt)
    {


        shotCnt -= cnt;
        if(shotCnt <= 0)
        {
            
            anim.SetBool("IsMultiShot", false);
            StartCoroutine("Cooltime");
            currentCoolTime = skillCoolTime;
            coolTimeCounter.text = "" + currentCoolTime;
            StartCoroutine("CoolTimeCounter");
            skillDelay = 0;
            shotCnt = 10;
            
        }
    }

    public void Walking_Start()
    {
        audioSource.clip = audioWalk;
        audioSource.Play();
    }

    public void Running_Start()
    {
        audioSource.clip = audioRun;
        audioSource.Play();
    }

    public void Idle_Start()
    {

        audioSource.Pause();
    }

    IEnumerator Cooltime()
    {
        while(skillFilter.fillAmount > 0)
        {
            skillFilter.fillAmount -= 1 * Time.smoothDeltaTime / skillCoolTime;

            yield return null;
        }

        yield break;

    }


    IEnumerator CoolTimeCounter()
    {
        while (currentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            currentCoolTime -= 1.0f;
            coolTimeCounter.text = "" + currentCoolTime;
        }

        coolTimeCounter.text = "";
        yield break;
    }

   

}
