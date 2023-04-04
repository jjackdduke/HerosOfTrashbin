using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerPlayerMover : MonoBehaviour
{
    // 이동 속도
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;


    // 회전 속도
    //[SerializeField] float rotateSpeed = 10.0f;

    Animator anim;
    Rigidbody rigid;
    PlayerStat playerStat;


    float h, v;

    bool fDown;
    //bool isFireReady = true;
    bool isSkillReady = true;
    bool skillDown;
    bool isBorder;
    bool shiftPressed;
    bool jDown;

    bool isJump;
    bool isRun;

    Weapon []equipWeapon;

    Vector3 moveVec;


    float fireDelay;
    float skillDelay = 10f;
    [SerializeField] public float skillCoolTime = 1f; // 스킬 쿨타임 정하는 곳 
    int swingCnt = 10;
    //public Image skillFilter;
    //public TextMeshProUGUI coolTimeCounter;
    private float currentCoolTime;

    // 오디오
    public AudioSource audioSource;
    public AudioClip audioSwing1;
    public AudioClip audioSwing2;
    public AudioClip audioWhirlwind;
    public AudioClip audioWalk;
    public AudioClip audioRun;

    void Start()
    {

        anim = GetComponent<Animator>();
        equipWeapon = GetComponentsInChildren<Weapon>();
        rigid = GetComponent<Rigidbody>();
        playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
        jumpPower = 20f;
        Debug.Log(equipWeapon[0]);
        Debug.Log(equipWeapon[1]);

    }

    void Update()
    {
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        //{
        //    anim.ResetTrigger("hammerCombo");
        //}

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
        if (equipWeapon == null || isRun)
            return;

        //fireDelay += Time.deltaTime;
        //isFireReady = equipWeapon.rate < fireDelay;

        skillDelay += Time.deltaTime;
        isSkillReady = skillCoolTime <= skillDelay;

        if (fDown)
        {

            anim.SetTrigger("hammerCombo");
            //equipWeapon.Use();

            //fireDelay = 0;
        }

        if (skillDown && isSkillReady && !fDown)
        {
            anim.applyRootMotion = true;
            anim.SetTrigger("hammerSkill");
            //swingCnt = 10;
            //skillFilter.fillAmount = 1;
            //anim.SetBool("IsWhirlwind", true);
            //equipWeapon.UseSkill();
            //skillDelay = 0;
        }



    }

    void Move()
    {

        if (fDown) return;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Combo1")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Combo2")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Combo3")
            || anim.GetCurrentAnimatorStateInfo(0).IsName("Combo4"))
        {

            return;
        }


        moveSpeed = playerStat.CurrentStatus(2);
        float local_moveSpeed = moveSpeed;

        moveVec = new Vector3(h, 0, v);


        if (fDown)
            moveVec = Vector3.zero;

        if (shiftPressed)
        {
            local_moveSpeed = local_moveSpeed * 2.0f;
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


    public void Hammer1_Start()
    {
        audioSource.clip = audioSwing1;
        audioSource.Play();
        equipWeapon[0].HammerComboAnimation();
        equipWeapon[1].HammerComboAnimation();

    }

    public void Hammer1_End()
    {
        equipWeapon[0].HammerComboEndAnimation();
        equipWeapon[1].HammerComboEndAnimation();
    }

    public void Hammer2_Start()
    {
        audioSource.clip = audioSwing2;
        audioSource.Play();
        equipWeapon[0].HammerComboAnimation();
        equipWeapon[1].HammerComboAnimation();
    }


    public void Hammer2_End()
    {
        equipWeapon[0].HammerComboEndAnimation();
        equipWeapon[1].HammerComboEndAnimation();
    }

    public void Hammer3_Start()
    {
        audioSource.clip = audioSwing2;
        audioSource.Play();
        equipWeapon[0].HammerComboAnimation();
        equipWeapon[1].HammerComboAnimation();
    }


    public void Hammer3_End()
    {
        equipWeapon[0].HammerComboEndAnimation();
        equipWeapon[1].HammerComboEndAnimation();
    }

    public void Hammer4_Start()
    {
        audioSource.clip = audioSwing2;
        audioSource.Play();
        equipWeapon[0].HammerComboAnimation();
        equipWeapon[1].HammerComboAnimation();
    }


    public void Hammer4_End()
    {
        equipWeapon[0].HammerComboEndAnimation();
        equipWeapon[1].HammerComboEndAnimation();
    }

    public void HammerSkill1_Start()
    {
        anim.SetBool("IsSkill", true);
        audioSource.clip = audioWhirlwind;
        audioSource.Play();
        equipWeapon[0].HammerSkillStartAnimation();
        equipWeapon[1].HammerSkillStartAnimation();
    }


    public void HammerSkill1_End()
    {
        audioSource.clip = audioWhirlwind;
        audioSource.Play();
        equipWeapon[0].HammerSkillEndAnimation();
        equipWeapon[1].HammerSkillEndAnimation();
    }

    public void HammerSkill2_Start()
    {
        audioSource.clip = audioWhirlwind;
        audioSource.Play();
        equipWeapon[0].HammerSkillStartAnimation();
        equipWeapon[1].HammerSkillStartAnimation();
    }


    public void HammerSkill2_End()
    {
        audioSource.clip = audioWhirlwind;
        audioSource.Play();
        equipWeapon[0].HammerSkillEndAnimation();
        equipWeapon[1].HammerSkillEndAnimation();
    }

    public void HammerSkill3_Start()
    {
        audioSource.clip = audioWhirlwind;
        audioSource.Play();
        equipWeapon[0].HammerSkillStartAnimation();
        equipWeapon[1].HammerSkillStartAnimation();
    }


    public void HammerSkill3_End()
    {
        audioSource.clip = audioWhirlwind;
        audioSource.Play();
        equipWeapon[0].HammerSkillEndAnimation();
        equipWeapon[1].HammerSkillEndAnimation();
    }


    public void HammerSkill4_Start()
    {
        audioSource.clip = audioWhirlwind;
        audioSource.Play();
        equipWeapon[0].HammerSkillStartAnimation();
        equipWeapon[1].HammerSkillStartAnimation();
    }


    public void HammerSkill4_End()
    {
        audioSource.clip = audioWhirlwind;
        audioSource.Play();
        equipWeapon[0].HammerSkillEndAnimation();
        equipWeapon[1].HammerSkillEndAnimation();
    }


    public void HammerSkill5_Start()
    {
        audioSource.clip = audioWhirlwind;
        audioSource.Play();
        equipWeapon[0].HammerSkillFinalAnimation();
        equipWeapon[1].HammerSkillFinalAnimation();
    }


    public void HammerSkill5_End()
    {
        audioSource.Pause();
        //StartCoroutine("Cooltime");
        currentCoolTime = skillCoolTime;
        //coolTimeCounter.text = "" + currentCoolTime;
        //StartCoroutine("CoolTimeCounter");
        skillDelay = 0;
        equipWeapon[0].HammerSkillFinalEndAnimation();
        equipWeapon[1].HammerSkillFinalEndAnimation();
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
        equipWeapon[0].HammerSkillEndAnimation();
        equipWeapon[1].HammerSkillEndAnimation();
        anim.applyRootMotion = false;
        audioSource.Pause();
    }

    //IEnumerator Cooltime()
    //{
    //    while (skillFilter.fillAmount > 0)
    //    {
    //        skillFilter.fillAmount -= 1 * Time.smoothDeltaTime / skillCoolTime;

    //        yield return null;
    //    }

    //    yield break;

    //}


    //IEnumerator CoolTimeCounter()
    //{
    //    while (currentCoolTime > 0)
    //    {
    //        yield return new WaitForSeconds(1.0f);
    //        currentCoolTime -= 1.0f;
    //        coolTimeCounter.text = "" + currentCoolTime;
    //    }

    //    coolTimeCounter.text = "";
    //    yield break;
    //}
}
