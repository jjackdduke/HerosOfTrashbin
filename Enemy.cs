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
    bool deBuffedArmor;

    private EnemySpawner enemySpawner;

    // 몬스터 status
    public float mobHP;
    public float armor;
    public bool isBoss;
    public float CurrentHP { get { return currentHP; } }

    private float currentHP;
    private float currentSpeed;
    private float currentArmor;

    [SerializeField] GameObject greenHP;
    [SerializeField] GameObject hpbar;
    [SerializeField] GameObject damageText;

    // 피격 이펙트
    ParticleSystem HitEffects;

    new Collider collider;

    void Start()
    {
        // 게임오브젝트로 선언된 pathGO에 하이어라키의 path들을 갖고 있는 Paths 오브젝트 할당
        pathGO = GameObject.Find(pathString);
        // Bank 오브젝트를 하이어라키에서 찾아 bank에 할당
        gm = FindObjectOfType<GM>();
        //anim = GetComponentInChildren<Animator>();

        currentHP = mobHP;
        currentSpeed = speed;
        currentArmor = armor;
        anim = this.transform.GetChild(0).GetComponent<Animator>();
        collider = gameObject.GetComponent<Collider>();
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
            ReachedGoal();
            //photonView.RPC("ReachedGoal", RpcTarget.All);


        }

    }

    [PunRPC]
    void ReachedGoal()
    {
        // 생명 깎고 몬스터 비활성화
        LooseLife();
        // pathNodeIdx = 0;
        // targetPathNode = null;
        // gameObject.SetActive(false);
        Death();
    }


    public void LooseLife()
    {
        // Bank의 생명력 감소 메소드 호출
        if (gm == null)
        {
            return;
        }
        gm.LooseLife(lifePenalty);
    }

    public void Death()
    {
        enemySpawner = GameObject.Find("startPoint").GetComponent<EnemySpawner>();
        // EnemySpawner에서 리스트로 적 정보를 관리하기 때문에 Destroy()를 직접 사용하지 않고
        // 함수를 호출
        enemySpawner.DestroyEnemy(this);
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.name == "canon")
        {

        }

        if (other.name == "character")
        {
            // Debug.Log("character hit");
            ProcessHit(2 - armor);
            //photonView.RPC("ProcessHit", RpcTarget.All, 2-armor);

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.gameObject.name);
        //ProcessHit(1);

    }

    [PunRPC]
    public void ProcessHit(float Damage, [Optional] ParticleSystem effect)
    {
        if (effect)
        {
        HitEffects = effect;
        Instantiate(HitEffects, transform.position, Quaternion.identity);

        }
        // 체력 감소
        if (Damage > currentArmor)
        {
            Damage -= currentArmor;
            currentHP -= Damage;
            damageText.GetComponent<TMP_Text>().text = Damage.ToString();
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
            PlayerStat playerStat = other.GetComponentInParent<PlayerStat>();
            float attackDamage = weapon.damage + playerStat.CurrentDamage;
            ProcessHit(attackDamage);
            //photonView.RPC("ProcessHit", RpcTarget.All,weaponDamage);
        }
    }

    [PunRPC]
    public void ProcessReduceSpeed(float reduce, ParticleSystem effect)
    {
        if (!deBuffedSpeed && currentSpeed == speed)
        {
            HitEffects = effect;
            Debug.Log("이속 감소!");
            Instantiate(HitEffects, transform.position, Quaternion.identity);
            currentSpeed = speed * (1 - reduce);
            deBuffedSpeed = true;
            StartCoroutine(isDeBuffedSpeed());
        }
    }

    public void ProcessReduceArmor(float reduce, ParticleSystem effect)
    {
        if (!deBuffedArmor && currentSpeed == speed)
        {
            HitEffects = effect;
            Debug.Log("방어 감소!");
            Instantiate(HitEffects, transform.position, Quaternion.identity);
            currentArmor = armor * (1 - reduce);
            deBuffedArmor = true;
            StartCoroutine(isDeBuffedArmor());
        }
    }

    IEnumerator isDeBuffedSpeed()
    {
        yield return new WaitForSeconds(1f);
        deBuffedSpeed = false;
        currentSpeed = speed;
    }

    IEnumerator isDeBuffedArmor()
    {
        yield return new WaitForSeconds(1f);
        deBuffedArmor = false;
        currentArmor = armor;
    }
}
