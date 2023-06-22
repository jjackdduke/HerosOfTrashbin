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

    private float currentHP;
    public float CurrentHP { get { return currentHP; } set { currentHP = value; } }
    private float currentSpeed;
    private float currentArmor;

    [SerializeField] GameObject greenHP;
    [SerializeField] GameObject hpbar;
    [SerializeField] GameObject damageText;

    // 피격이펙트, 사운드
    ParticleSystem HitEffects;
    

    // 보스 status
    public bool isBoss = false;
    public int skillIdx = 0;
    public float coolDown = 3;
    public int power = 50;
    public int range = 100;

    // 스킬이펙트
    [SerializeField] ParticleSystem redCircle;

    [SerializeField] ParticleSystem greenCircle;
    [SerializeField] ParticleSystem Heel;
    [SerializeField] ParticleSystem groundCrush;

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
        if (isBoss)
        {
            StartCoroutine(WarningSkill(skillIdx, coolDown));
        }

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
            ProcessHit(2 - armor,null);
            //photonView.RPC("ProcessHit", RpcTarget.All, 2-armor);

        }
    }

    public void processHeel(float num)
    {
        if( currentHP + num > mobHP)
        {
            currentHP = mobHP;
        }
        else
        {
            currentHP += num;
        }
    }


    [PunRPC]
    public void ProcessHit(float Damage, [Optional] ParticleSystem effect)
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
            if (isBoss && Damage > (float)(mobHP * 0.15))
            {
                Damage = (float)(mobHP * 0.15);
            }

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

    void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Weapon")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            PlayerStat playerStat = GameObject.Find("Player").GetComponent<PlayerStat>();
            float attackDamage = weapon.damage + playerStat.CurrentStatus(1);
            ProcessHit(attackDamage,weapon.hitEffect);
            //photonView.RPC("ProcessHit", RpcTarget.All,weaponDamage);
        }else if(other.tag == "Arrow")
        {
            Arrow arrow = other.GetComponent<Collider>().GetComponent<Arrow>();
            float attackDamage = arrow.damage;
            ProcessHit(attackDamage, arrow.hitEffect);
            GameObject arrow01 = other.transform.Find("Arrow01").gameObject;
            Destroy(other.gameObject);
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
        if (currentSpeed != 0)
        {
            currentSpeed = speed;
        }
    }

    IEnumerator isDeBuffedArmor()
    {
        yield return new WaitForSeconds(4f);
        deBuffedArmor = false;
        currentArmor = armor;
    }

    private IEnumerator Skill_KnockBack(float range = 100, float power = 50, float coolDown = 3)
    {
        yield return new WaitForSeconds(0.01f);
        anim.SetBool("isSkill", false);
        yield return new WaitForSeconds(0.667f);
        redCircle.Stop();
        currentSpeed = speed;
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
        yield return new WaitForSeconds(coolDown);
    }
    private IEnumerator Skill_Heeling(float range = 100, float power = 1000, float coolDown = 3)
    {
        yield return new WaitForSeconds(0.01f);
        anim.SetBool("isSkill", false);
        yield return new WaitForSeconds(0.667f);
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            
            float d = Vector3.Distance(transform.position, enemy.transform.position);
            if (d < range)
            {
                if(enemy.gameObject.transform.childCount == 4)
                {
                    ParticleSystem heelingEffect = Instantiate(Heel, enemy.transform);
                    heelingEffect.transform.localScale = enemy.transform.GetChild(0).transform.localScale;
                    heelingEffect.Play();
                }
                enemy.GetComponent<Enemy>().processHeel(power);
            }

        }
        yield return new WaitForSeconds(coolDown);
    }
    private IEnumerator Skill_Stun(float range = 100, float power = 50, float coolDown = 3)
    {
        yield return new WaitForSeconds(0.01f);
        anim.SetBool("isSkill", false);
        yield return new WaitForSeconds(0.667f);
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
        yield return new WaitForSeconds(coolDown);
    }

    private IEnumerator WarningSkill(int skillIdx, float coolDown)
    {
        while (true)
        {

            /*
            0 : 넉백
            1 : 힐링
            2 : 스턴(짜부)
            */
            if (skillIdx == 0)
            {
                currentSpeed = 0;
                redCircle.transform.localScale = new Vector3(range / 2, 1, range / 2);
                redCircle.Play();
                yield return new WaitForSeconds(3f);
                anim.SetBool("isSkill", true);
                StartCoroutine(Skill_KnockBack(range, power, coolDown));
            }
            else if (skillIdx == 1)
            {
                greenCircle.transform.localScale = new Vector3(range / 3, 1, range / 3);
                greenCircle.Play();
                //yield return new WaitForSeconds(3f);
                anim.SetBool("isSkill", true);
                StartCoroutine(Skill_Heeling(range, power, coolDown));
            }
            else if (skillIdx == 2)
            {
                currentSpeed = 0;
                redCircle.transform.localScale = new Vector3(range / 3, 1, range / 3);
                redCircle.Play();
                yield return new WaitForSeconds(3f);
                anim.SetBool("isSkill", true);
                StartCoroutine(Skill_Stun(range, power, coolDown));
            }
            yield return new WaitForSeconds(coolDown);
        }
    }

    public void Skill()
    {
        if (skillIdx == 0)
        {
            Debug.Log("ehlsi");
            StartCoroutine(Skill_KnockBack(range, power, coolDown));
        }
        else if (skillIdx == 1)
        {
            StartCoroutine(Skill_Heeling(range, power, coolDown));
        }
        else if (skillIdx == 2)
        {
            StartCoroutine(Skill_Stun(range, power, coolDown));
        }
    }
}
