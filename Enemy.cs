using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Runtime.InteropServices;

public class Enemy : MonoBehaviourPunCallbacks
{
    // 몬스터 경로 오브젝트 변수 선언
    GameObject pathGO;

    // 몬스터가 이동할 오브젝트 변수 선언
    Transform targetPathNode;

    // 몬스터가 현재 따라갈 path 오브젝트 순서
    int pathNodeIdx = 0;
    public int PathNodeIdx { get { return pathNodeIdx; } }

    // 몬스터 잡았을 시 상금
    //[SerializeField] int goldReward = 2;


    public GM gm;

    //애니메이션 부여
    Animator anim;
    // wavesystem 참조로 변경중...

    // 경로 입력
    public string pathString;
    // 몬스터 놓쳤을 때의 생명력 감소
    public int lifePenalty;
    // 몬스터 속도
    public float speed;
    bool deBuffedSpeed;
    public bool DeBuffedSpeed { get { return deBuffedSpeed;  } }
    bool deBuffedArmor;
    public bool DeBuffedArmor { get { return deBuffedArmor; } }

    private EnemySpawner enemySpawner;

    // 몬스터 status
    public float mobHP;
    public float armor;
    public float CurrentHP { get { return currentHP; } set { currentHP = value; } }

    private float currentHP;
    private float currentSpeed;
    private float currentArmor;

    [SerializeField] GameObject greenHP;
    [SerializeField] GameObject hpbar;
    [SerializeField] GameObject damageText;

    // 피격이펙트
    ParticleSystem HitEffects;

    // 보스 status
    public bool isBoss = false;
    public int power = 50;
    public int range = 100;

    // 스킬이펙트
    [SerializeField] GameObject greenCircle;
    [SerializeField] ParticleSystem redCircle;
    [SerializeField] GameObject Heal;
    [SerializeField] GameObject groundCrush;

    new Collider collider;

    void Start()
    {
        // 게임오브젝트로 선언된 pathGO에 하이어라키의 path들을 갖고 있는 Paths 오브젝트 할당
        pathGO = GameObject.Find(pathString);
        gm = FindObjectOfType<GM>();
        //anim = GetComponentInChildren<Animator>();

        currentHP = mobHP;
        currentSpeed = speed;
        currentArmor = armor;
        anim = this.transform.GetChild(0).GetComponent<Animator>();
        collider = gameObject.GetComponent<Collider>();
        StartCoroutine(WarningSkill(0, 5));

    }

    // Update is called once per frame
    void Update()
    {

        // 이동할 타겟이 없을 경우
        if (targetPathNode == null)
        {
            // 다음 타겟 찾는 메소드 호출
            GetNextPathNode();
            if(targetPathNode == null)
            {
                // 다음 타겟이 없으면 이동 종료하는 함수 호출
                ReachedGoal();
                return;
            }
        }

        // 타켓 노드와 몬스터 위치로 방향 및 거리 확인
        Vector3 dir = targetPathNode.position - this.transform.position;

        // 몬스터 이동 애니메이션 설정
        //anim.SetBool("isRun", dir != Vector3.zero);


        float distThisFrame = currentSpeed * Time.deltaTime;
        if(dir.magnitude <= distThisFrame)
        {
            // 노드에 도착
            targetPathNode = null;
        }
        else
        {
            // 노드로 이동
            transform.Translate(dir.normalized * distThisFrame, Space.World);
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 5);

        }

        greenHP.GetComponent<Image>().fillAmount = currentHP / mobHP;
    }

    public void GetNextPathNode()
    {
        // path 배열이 끝나지 않았다면 다음 인덱스로 이동
        if (pathNodeIdx < pathGO.transform.childCount)
        {
            targetPathNode = pathGO.transform.GetChild(pathNodeIdx);
            pathNodeIdx++;
        }
        else
        {
            targetPathNode = null;
            //photonView.RPC("ReachedGoal", RpcTarget.All);


        }

    }

    [PunRPC]
    void ReachedGoal()
    {
        Debug.Log("골도착");
        // 생명 깎고 몬스터 비활성화
        LooseLife();
        // pathNodeIdx = 0;
        // targetPathNode = null;
        // gameObject.SetActive(false);
        Death(false);
    }


    public void LooseLife()
    {
        Debug.Log("enemy생명력감소");
        // GM의 생명력 감소 메소드 호출
        if (gm == null)
        {
            return;
        }
        gm.LooseLife(lifePenalty);
    }

    public void Death(bool isKill = true)
    {
        enemySpawner = GameObject.Find("startPoint").GetComponent<EnemySpawner>();
        // EnemySpawner에서 리스트로 적 정보를 관리하기 때문에 Destroy()를 직접 사용하지 않고
        // 함수를 호출
        if (isKill)
        {
            enemySpawner.DestroyEnemy(this);
        }
        else
        {
            enemySpawner.DestroyEnemy(this, false);
            Destroy(this.gameObject);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.name == "canon")
        {

        }

        if (other.name == "character")
        {
            // Debug.Log("character hit");
            ProcessHit(2 - armor,null,other.transform.position);
            //photonView.RPC("ProcessHit", RpcTarget.All, 2-armor);

        }
    }


    [PunRPC]
    public void ProcessHit(float Damage, [Optional] ParticleSystem effect, Vector3 hitPos)
    {
        if (effect)
        {
        HitEffects = effect;
        Instantiate(HitEffects, transform.position, Quaternion.identity);

        }

        if (isBoss && Damage > (float)(mobHP * 0.1))
        {
           Damage = (float)(mobHP * 0.1);
        }

        // 체력감소 로직
        if (Damage > currentArmor)
        {
            Damage -= currentArmor;
            if (mobHP < Damage)
            {
                damageText.GetComponent<TMP_Text>().text = "OverKill!";
            }
            else
            {
                damageText.GetComponent<TMP_Text>().text = Damage.ToString();
            }
            currentHP -= Damage;
            if (currentHP > 0)
            {
                anim.SetTrigger("Hit");
            }
        }
        else
        {
            damageText.GetComponent<TMP_Text>().text = "Blocked!";
        }

        GameObject Hit = Instantiate(damageText, transform.position, Quaternion.identity, this.GetComponentInChildren<Canvas>().transform);
        Destroy(Hit, 0.6f);

        // 테스트용 체력감소
        // currentHP -= 1;


        if (currentHP <= 0)
        {
            gm.MobCounter(false);
            collider.enabled = false;
            currentSpeed = 0;
            //Destroy(hpbar);
            hpbar.SetActive(false);
            //anim.SetBool("isDeath", true);
            anim.SetTrigger("Death");
            Death();

        }
    }

    void OnCollisionEnter(Collision other)
    {

        if(other.collider.tag == "Weapon")
        {
            Weapon weapon = other.collider.GetComponent<Weapon>();
            PlayerStat playerStat = other.collider.GetComponentInParent<PlayerStat>();
            float attackDamage = weapon.damage + playerStat.CurrentDamage;
            ProcessHit(attackDamage,weapon.hitEffect,other.contacts[0].point);
            //photonView.RPC("ProcessHit", RpcTarget.All,weaponDamage);
        }
    }

    [PunRPC]
    public void ProcessReduceSpeed(float reduce, ParticleSystem effect)
    {
        if (currentSpeed == speed)
        {
            HitEffects = effect;
            Instantiate(HitEffects, transform.position, Quaternion.identity);
            currentSpeed = speed * (1 - reduce);
            deBuffedSpeed = true;
            StartCoroutine(isDeBuffedSpeed());
        }
    }

    public void ProcessReduceArmor(float reduce, ParticleSystem effect)
    {
        if (currentArmor == armor)
        {
            HitEffects = effect;
            Instantiate(HitEffects, transform.position, Quaternion.identity);
            currentArmor = armor * (1 - reduce);
            deBuffedArmor = true;
            StartCoroutine(isDeBuffedArmor());
        }
    }

    IEnumerator isDeBuffedSpeed()
    {
        yield return new WaitForSeconds(4f);
        deBuffedSpeed = false;
        currentSpeed = speed;
    }

    IEnumerator isDeBuffedArmor()
    {
        yield return new WaitForSeconds(4f);
        deBuffedArmor = false;
        currentArmor = armor;
    }

    private IEnumerator Skill_KnockBack(float range = 100, float power = 50)
    {
        redCircle.Stop();
        currentSpeed = speed;
        redCircle.transform.localScale = new Vector3(range / 3, 1, range / 3);
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach ( GameObject player in players)
        {
            float d = Vector3.Distance(transform.position, player.transform.position);

            if (d < range)
            {
                Vector3 dir = (player.transform.position - transform.position).normalized;
                player.GetComponent<Rigidbody>().velocity = new Vector3(dir.x * power, power / 2, dir.z * power);
            }

        }
        //Destroy(skillRange, 0.5f);
        yield return new WaitForSeconds(1f);
    }
    private IEnumerator Skill_Heeling(float range = 100, float power = 1000)
    {
        redCircle.Stop();
        currentSpeed = speed;
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float currentHP = enemy.GetComponent<Enemy>().currentHP;
            float mobHP = enemy.GetComponent<Enemy>().mobHP;
            float d = Vector3.Distance(transform.position, enemy.transform.position);
            if (d < range)
            {
                if( currentHP + power > mobHP)
                {
                    currentHP = mobHP;
                }
                else
                {
                    currentHP += power;
                }
            }

        }
        yield return new WaitForSeconds(1f);
    }
    private IEnumerator Skill_Stun(float range = 100, float power = 50)
    {
        redCircle.Stop();
        currentSpeed = speed;
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            float d = Vector3.Distance(transform.position, player.transform.position);

            if (d < range)
            {
                player.transform.localScale = new Vector3(7, 1, 7);
                // 움직임 멈추는 로직 발현
            }

        }
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator WarningSkill(int skillIdx, float coolDown)
    {
        while (true)
        {
            currentSpeed = 0;
            redCircle.Play();
            yield return new WaitForSeconds(3f);
            /*
            0 : 넉백
            1 : 힐링
            2 : 스턴(짜부)
            */
            if (skillIdx == 0)
            {
                StartCoroutine(Skill_KnockBack(range, power));
            }
            else if (skillIdx == 1)
            {
                StartCoroutine(Skill_Heeling(range, power));
            }
            else if (skillIdx == 2)
            {
                StartCoroutine(Skill_Stun(range, power));
            }
            yield return new WaitForSeconds(coolDown);
        }
    }
}
