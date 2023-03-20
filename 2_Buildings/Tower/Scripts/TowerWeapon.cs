using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.XR;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEngine.GraphicsBuffer;

public class TowerWeapon : MonoBehaviour
{
    Bank bank;
    private Transform attackTarget = null;
    private Enemy[] enemies;
    private float attackTargetDistance;
    bool build;
    int childCnt;
    int level = 1;
    bool isMissile = false;
    GameObject Spawner;
    [SerializeField] private TowerTemplate towerTemplate;
    [SerializeField] GameObject projectilePrefab;
    public ParticleSystem effect;

    public GameObject missile => towerTemplate.weapon[level - 1].missile;

    public float damage => towerTemplate.weapon[level - 1].Damage;
    public float attackRate => towerTemplate.weapon[level - 1].rate;
    public float attackRange => towerTemplate.weapon[level - 1].range;
    public bool isParticle => towerTemplate.weapon[level - 1].isParticle;
    public float missileUp => towerTemplate.weapon[level - 1].missileUp;
    public float missileSpeed => towerTemplate.weapon[level - 1].missileSpeed;
    public float missileWaitSecond => towerTemplate.weapon[level - 1].missileWaitSecond;
    
    public void SetUp(TowerTemplate towerTemplate, bool build)
    {
        this.towerTemplate = towerTemplate;

        //TargetLocator[] targetLocators;
        //targetLocators = GetComponentsInChildren<TargetLocator>();
        //foreach (TargetLocator TL in targetLocators)
        //{
        //    TL.SetUp(towerTemplate, level, true);
        //}

    }
    
    private void Awake()
    {
        Debug.Log(towerTemplate.weapon[level - 1]);
        childCnt = transform.childCount;
        for (int i = 0 ; i < childCnt; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(0).gameObject.SetActive(true);
        Transform currentLevel = transform.GetChild(0);
        currentLevel.gameObject.SetActive(true);
        
        this.enemies = FindObjectsOfType<Enemy>();
        if (isParticle)
        {
            effect = currentLevel.GetComponentInChildren<ParticleSystem>();
        }
        else
        {
            Spawner = transform.GetChild(0).gameObject;
        }
        GameObject camera = GameObject.FindWithTag("MainCamera");
        transform.rotation = new Quaternion(0, camera.transform.rotation.y -180, 0, transform.rotation.w);
        
    }


    public void OnMouseUp()
    {
        bool result = Upgrade();
        Debug.Log(result);
    }

private void Update()
    {
        SearchTarget();
        if (attackTarget != null && attackTargetDistance < attackRange)
        {
            if (isParticle || !isMissile)
            {
                if (!isMissile)
                {
                    Fire();
                }
                StartCoroutine(AttackToTarget(isParticle));
            }
        }
    }
    

    private bool Upgrade()
    {
        bank = FindObjectOfType<Bank>();
        Debug.Log(level);
        Debug.Log(childCnt);
        Debug.Log(towerTemplate.weapon[level].cost);
        if (level < childCnt && bank.CurrentBalance >= towerTemplate.weapon[level].cost)
        {
            //Debug.Log("Ok");
            transform.GetChild(level).gameObject.SetActive(false);
            level++;
            transform.GetChild(level).gameObject.SetActive(true);
            bank.Withdraw(towerTemplate.weapon[level - 1].cost);

            //Debug.Log("level" + level);
            //GetComponentInChildren<TargetLocator>().SetUp(towerTemplate, level, true);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SearchTarget()
    {
            enemies = FindObjectsOfType<Enemy>();
            Transform closestTarget = null;
            float maxDistance = Mathf.Infinity;

            foreach (Enemy enemy in enemies)
            {
                float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

                if (targetDistance < maxDistance)
                {
                    closestTarget = enemy.transform;
                    maxDistance = targetDistance;
                }
            }
            attackTarget = closestTarget;
            attackTargetDistance = maxDistance;
            GetComponentInChildren<TargetLocator>().SetUp(attackTarget, attackTargetDistance, attackRange);
    }

    /*
    void Attack(bool isActive)
    {
        var emissionModule = effect.emission;
        emissionModule.enabled = isActive;
    }
    */
    
    private IEnumerator AttackToTarget(bool particle)
    {
        yield return new WaitForSeconds(attackRate);
        if (particle)
        {
            effect.Play();
        }
        else
        {
            isMissile = false;  
        }
    }

    private void Fire()
    {
        if (attackTarget)
        {
            isMissile = true;
            GameObject missileClone = Instantiate(missile, Spawner.transform.position, Quaternion.identity);
            missileClone.GetComponent<Missile>().SetUp(attackTarget, damage, missileSpeed, missileWaitSecond);
            missileClone.GetComponent<Rigidbody>().velocity = Vector3.up * missileUp;
        }
        
    }
}

