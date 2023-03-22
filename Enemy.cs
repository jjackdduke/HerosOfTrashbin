using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    // 몬스터 경로 오브젝트 변수 선언
    GameObject pathGO;

    // 몬스터가 이동할 오브젝트 변수 선언
    Transform targetPathNode;

    // 몬스터가 현재 따라갈 path 오브젝트 순서
    int pathNodeIdx = 0;
    public int PathNodeIdx { get { return pathNodeIdx; } }
    
    //애니메이션 부여
    //Animator anim;

    // 몬스터 잡았을 시 상금
    //[SerializeField] int goldReward = 2;

    
    public GM bank;
    // wavesystem 참조로 변경중...

    // 경로 입력
    public string pathString;
    // 몬스터 놓쳤을 때의 생명력 감소
    public int lifePenalty;
    // 몬스터 속도
    public float speed;
    bool deBuffed;

    private EnemySpawner enemySpawner;

    // 몬스터 status
    public float mobHP;
    public float armor;
    public bool isBoss;

    private float currentHP;
    private float currentSpeed;

    [SerializeField] GameObject greenHP;
    [SerializeField] GameObject damageText;



    void Start()
    {
        // 게임오브젝트로 선언된 pathGO에 하이어라키의 path들을 갖고 있는 Paths 오브젝트 할당
        pathGO = GameObject.Find(pathString);
        // Bank 오브젝트를 하이어라키에서 찾아 bank에 할당
        bank = FindObjectOfType<GM>();
        //anim = GetComponentInChildren<Animator>();

        currentHP = mobHP;
        currentSpeed = speed;
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
        }

    }


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
        if (bank == null)
        {
            return;
        }
        bank.LooseLife(lifePenalty);
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
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.gameObject.name);
        //ProcessHit(1);

    }

    public void ProcessHit(float Damage)
    {

        
        // 체력 감소
        currentHP -= Damage;
        damageText.GetComponent<TMP_Text>().text = Damage.ToString();
        GameObject Hit = Instantiate(damageText, transform.position, Quaternion.identity, this.GetComponentInChildren<Canvas>().transform);
        Destroy(Hit, 0.6f);

        // 테스트용 체력감소
        // currentHP -= 1;


        if (currentHP <= 0)
        {
            Death();

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Weapon")
        {
  
            Weapon weapon = other.GetComponent<Weapon>();
            float weaponDamage = weapon.damage;
            ProcessHit(weaponDamage);
        }
    }

    public void ProcessReduceSpeed(float reduce)
    {
        if (!deBuffed && currentSpeed == speed)
        {
            Debug.Log("이속 감소!");
            currentSpeed = speed * (1 - reduce);
            deBuffed = true;
            StartCoroutine(isDeBuffed());
        }
    }
    IEnumerator isDeBuffed()
    {
        yield return new WaitForSeconds(1f);
        deBuffed = false;
        currentSpeed = speed;
    }
}
