using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.XR;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class TowerWeapon : MonoBehaviour
{
    Bank bank;
    int level = 1;
    int childCnt;

    Transform attackTarget = null;
    Enemy[] enemies;
    float attackTargetDistance;
    bool isMissile = false;

    GameObject Spawner;
    [SerializeField] private TowerTemplate thisTowerTemplate;
    ParticleSystem effect;

    float towerDamage => thisTowerTemplate.weapon[level - 1].Damage;
    float towerAttackRate => thisTowerTemplate.weapon[level - 1].rate;
    float towerAttackRange => thisTowerTemplate.weapon[level - 1].range;
    bool towerIsParticle => thisTowerTemplate.weapon[level - 1].isParticle;
    bool towerIsDeBuff => thisTowerTemplate.weapon[level - 1].isDeBuff;
    bool towerIsZangPan => thisTowerTemplate.weapon[level - 1].isZangPan;
    GameObject towerMissile => thisTowerTemplate.weapon[level - 1].missile;
    float towerMissileUp => thisTowerTemplate.weapon[level - 1].missileUp;
    float towerMissileSpeed => thisTowerTemplate.weapon[level - 1].missileSpeed;
    float towerMissileWaitSecond => thisTowerTemplate.weapon[level - 1].missileWaitSecond;
    
    public void SetUp(TowerTemplate towerTemplate)
    {
        this.thisTowerTemplate = towerTemplate;
    }
    
    private void Awake()
    {
        //Debug.Log(level);
        //Debug.Log(thisTowerTemplate.weapon[level - 1]);
        childCnt = transform.childCount;
        for (int i = 0 ; i < childCnt; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        transform.GetChild(0).gameObject.SetActive(true);
        Transform currentLevel = transform.GetChild(level);
        currentLevel.gameObject.SetActive(true);
        
        Spawner = transform.GetChild(0).gameObject;
        this.enemies = FindObjectsOfType<Enemy>();
        SpecialTower();
        GameObject camera = GameObject.FindWithTag("MainCamera");
        transform.rotation = new Quaternion(0, camera.transform.rotation.y -180, 0, transform.rotation.w);
        
    }

    private void Start()
    {
        StartCoroutine(SearchTarget());
        if (towerIsParticle)
        {
            StartCoroutine(AttackToTarget());
            Debug.DrawRay(Spawner.transform.position, Spawner.transform.forward, Color.red);

        }
    }

    public void OnMouseUp()
    {
        bool result = Upgrade();
        Debug.Log(result);
    }

    private void Update()
    {
        
        if (attackTarget != null && attackTargetDistance < towerAttackRange)
        {
            if (!towerIsParticle && !isMissile && !towerIsZangPan)
            {
                Fire();
                StartCoroutine(MissileAttack());
            }
        }
        if (towerIsZangPan)
        {
            ZangPanGi(transform.position, towerAttackRange);
        }
    }
    

    private bool Upgrade()
    {
        bank = FindObjectOfType<Bank>();
        Debug.Log(level);
        Debug.Log(childCnt);
        Debug.Log(thisTowerTemplate.weapon[level].cost);
        if (level < childCnt && bank.CurrentBalance >= thisTowerTemplate.weapon[level].cost)
        {
            transform.GetChild(level).gameObject.SetActive(false);
            level++;
            transform.GetChild(level).gameObject.SetActive(true);
            bank.Withdraw(thisTowerTemplate.weapon[level - 1].cost);
            SpecialTower();
            return true;
        }
        else
        {
            return false;
        }
    }

    void SpecialTower()
    {
        if (towerIsParticle)
        {
            effect = transform.GetChild(level).gameObject.GetComponentInChildren<ParticleSystem>();
        }
        if (towerIsZangPan)
        {
            ParticleSystem zangPan = Spawner.GetComponentInChildren<ParticleSystem>();
            zangPan.transform.localScale = new Vector3(1, (towerAttackRange * 3 / 10), (towerAttackRange * 3 / 10));
        }
    }

    IEnumerator SearchTarget()
    {
            
        while (true)
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
            GetComponentInChildren<TargetLocator>().SetUp(attackTarget, attackTargetDistance, towerAttackRange);
            yield return new WaitForSeconds(0.5f);
        }
    }

    
    private IEnumerator MissileAttack()
    {
        yield return new WaitForSeconds(towerAttackRate);
        isMissile = false;
    }

    private IEnumerator AttackToTarget()
    {
       
        while (true)
        {
        RaycastHit hitInfo;
        Debug.DrawRay(Spawner.transform.position, Spawner.transform.forward, Color.red);
        if (Physics.Raycast(Spawner.transform.position, Spawner.transform.forward, out hitInfo, towerAttackRange))
            {
                if (hitInfo.collider.tag == "Enemy")
                {
                    effect.Play();
                    Enemy enemy = hitInfo.collider.GetComponent<Enemy>();
                    if (towerIsDeBuff)
                    {
                        enemy.ProcessReduceSpeed(towerDamage);
                    }
                    else
                    {
                        enemy.ProcessHit(towerDamage);
                    }
                }
            }
            yield return new WaitForSeconds(towerAttackRate);

        }

    }

    private void Fire()
    {
        if (attackTarget)
        {
            isMissile = true;
            GameObject missileClone = Instantiate(towerMissile, Spawner.transform.position, Quaternion.identity);
            missileClone.GetComponent<Missile>().SetUp(attackTarget, towerDamage, towerMissileSpeed, towerMissileWaitSecond, towerIsDeBuff);
            missileClone.GetComponent<Rigidbody>().velocity = Vector3.up * towerMissileUp;
        }
        
    }

    void ZangPanGi(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.tag == "Enemy")
            {
                Enemy enemy = hitColliders[i].gameObject.GetComponent<Enemy>();
                //Debug.Log(enemy.name);
                //Enemy enemy = hitColliders[i].GetComponent<Enemy>();
                enemy.ProcessReduceSpeed(towerDamage);
            }
            i++;
        }
    }
}

