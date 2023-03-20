

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    //Animator anim;

    // ���� ����� �� ���
    //[SerializeField] int goldReward = 2;

    
    public Bank bank;
    // wavesystem ������ ������...

    // ��� �Է�
    public string pathString;
    // ���� ������ ���� ����� ����
    public int lifePenalty;
    // ���� �ӵ�
    public float speed;

    private EnemySpawner enemySpawner;

    // ���� status
    public float mobHP;
    public float armor;
    public bool isBoss;

    private float currentHP;

    [SerializeField] GameObject greenHP;



    void Start()
    {
        // ���ӿ�����Ʈ�� ����� pathGO�� ���̾��Ű�� path���� ���� �ִ� Paths ������Ʈ �Ҵ�
        pathGO = GameObject.Find(pathString);
        // Bank ������Ʈ�� ���̾��Ű���� ã�� bank�� �Ҵ�
        bank = FindObjectOfType<Bank>();
        //anim = GetComponentInChildren<Animator>();

        currentHP = mobHP;
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

        
        float distThisFrame = speed * Time.deltaTime;
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
        if (bank == null)
        {
            return;
        }
        bank.LooseLife(lifePenalty);
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
        ProcessHit(1);

    }
    public void ProcessHit(float Damage)
    {
        // ü�� ����
        // currentHP -= Damage;

        // �׽�Ʈ�� ü�°���
        Debug.Log("ü�� ����!");
        currentHP -= 1;


        if (currentHP <= 0)
        {
            Death();

        }
    }
}
