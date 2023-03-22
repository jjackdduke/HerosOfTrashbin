using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    // ���� ��� ������Ʈ ���� ����
    GameObject pathGO;

    // ���Ͱ� �̵��� ������Ʈ ���� ����
    Transform targetPathNode;

    // ���Ͱ� ���� ���� path ������Ʈ ����
    int pathNodeIdx = 0;
    public int PathNodeIdx { get { return pathNodeIdx; } }

    //�ִϸ��̼� �ο�
    Animator anim;
    
    // ���� ����� �� ���
    //[SerializeField] int goldReward = 2;

    
    public GM gm;
    // wavesystem ������ ������...

    // ��� �Է�
    public string pathString;
    // ���� ������ ���� ����� ����
    public int lifePenalty;
    // ���� �ӵ�
    public float speed;
    bool deBuffed;

    private EnemySpawner enemySpawner;

    // ���� status
    public float mobHP;
    public float armor;
    public bool isBoss;

    private float currentHP;
    public float CurrentHP { get { return currentHP; } }
    private float currentSpeed;

    [SerializeField] GameObject greenHP;
    [SerializeField] GameObject damageText;
    [SerializeField] GameObject hpbar;


    void Start()
    {
        // ���ӿ�����Ʈ�� ����� pathGO�� ���̾��Ű�� path���� ���� �ִ� Paths ������Ʈ �Ҵ�
        pathGO = GameObject.Find(pathString);
        // Bank ������Ʈ�� ���̾��Ű���� ã�� bank�� �Ҵ�
        gm = FindObjectOfType<GM>();
        //anim = GetComponentInChildren<Animator>();

        currentHP = mobHP;
        currentSpeed = speed;
        anim = this.transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // �̵��� Ÿ���� ���� ���
        if (targetPathNode == null)
        {
            // ���� Ÿ�� ã�� �޼ҵ� ȣ��
            GetNextPathNode();
            if(targetPathNode == null)
            {
                // ���� Ÿ���� ������ �̵� �����ϴ� �Լ� ȣ��
                ReachedGoal();
                return;
            }
        }

        // Ÿ�� ���� ���� ��ġ�� ���� �� �Ÿ� Ȯ��
        Vector3 dir = targetPathNode.position - this.transform.position;
        
        // ���� �̵� �ִϸ��̼� ����
        //anim.SetBool("isRun", dir != Vector3.zero);

        
        float distThisFrame = currentSpeed * Time.deltaTime;
        if(dir.magnitude <= distThisFrame)
        {
            // ��忡 ����
            targetPathNode = null;
        }
        else
        {
            // ���� �̵�
            transform.Translate(dir.normalized * distThisFrame, Space.World);
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 5);

        }

        greenHP.GetComponent<Image>().fillAmount = currentHP / mobHP;
    }

    public void GetNextPathNode()
    {
        // path �迭�� ������ �ʾҴٸ� ���� �ε����� �̵�
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
        // ���� ��� ���� ��Ȱ��ȭ
        LooseLife();
        // pathNodeIdx = 0;
        // targetPathNode = null;
        // gameObject.SetActive(false);
        Death();
    }


    public void LooseLife()
    {
        // Bank�� ����� ���� �޼ҵ� ȣ��
        if (gm == null)
        {
            return;
        }
        gm.LooseLife(lifePenalty);
    }

    public void Death()
    {
        enemySpawner = GameObject.Find("startPoint").GetComponent<EnemySpawner>();
        // EnemySpawner���� ����Ʈ�� �� ������ �����ϱ� ������ Destroy()�� ���� ������� �ʰ�
        // �Լ��� ȣ��
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
        // ü�� ����
        if (Damage > armor)
        {
            Damage -= armor;
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

        // �׽�Ʈ�� ü�°���
        // currentHP -= 1;


        if (currentHP <= 0)
        {
            currentSpeed = 0;
            Destroy(hpbar);
            anim.SetBool("isDeath", true);
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
            Debug.Log("�̼� ����!");
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
