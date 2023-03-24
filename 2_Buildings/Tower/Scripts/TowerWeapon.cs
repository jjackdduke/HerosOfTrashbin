using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.XR;
using static Cinemachine.CinemachineTargetGroup;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;
using TMPro;
using OpenCover.Framework.Model;

public class TowerWeapon : MonoBehaviour
{
    GM gm;
    int level = 1;
    int childCnt;

    Transform attackTarget = null;
    Enemy[] enemies;
    float attackTargetDistance;
    bool isMissile = false;
    bool eDown; 

    GameObject Spawner;
    [SerializeField] private TowerTemplate thisTowerTemplate;
    ParticleSystem effect;

    float towerDamage => thisTowerTemplate.weapon[level - 1].Damage;
    float towerAttackRate => thisTowerTemplate.weapon[level - 1].rate;
    float towerAttackRange => thisTowerTemplate.weapon[level - 1].range;
    bool towerIsParticle => thisTowerTemplate.weapon[level - 1].isParticle;
    int towerIsDeBuff => thisTowerTemplate.weapon[level - 1].isDeBuff;
    bool towerIsZangPan => thisTowerTemplate.weapon[level - 1].isZangPan;
    bool towerIsLaser => thisTowerTemplate.weapon[level - 1].isLaser;
    float towerLaserRate => thisTowerTemplate.weapon[level - 1].LaserRate;
    GameObject towerMissile => thisTowerTemplate.weapon[level - 1].missile;
    float towerMissileUp => thisTowerTemplate.weapon[level - 1].missileUp;
    float towerMissileSpeed => thisTowerTemplate.weapon[level - 1].missileSpeed;
    float towerMissileWaitSecond => thisTowerTemplate.weapon[level - 1].missileWaitSecond;
    ParticleSystem towerHitEffect => thisTowerTemplate.weapon[level - 1].hitEffect;



    [SerializeField]
    private TextMeshProUGUI thisActionText;

    bool upgradeActivated;
    bool isMaxUpgrade;
    bool isGoldEnough;
    public void SetUp(TowerTemplate towerTemplate, TextMeshProUGUI actionText)
    {
        this.thisTowerTemplate = towerTemplate;
        this.thisActionText = actionText;
    }
    
    public void SetUp2(TowerTemplate towerTemplate)
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
        gm = FindObjectOfType<GM>();
    }

    private void Start()
    {
        StartCoroutine(SearchTarget());
        if (towerIsParticle)
        {
            if (towerIsLaser)
            {
                StartCoroutine(Laser());
            }
            else
            {
                StartCoroutine(AttackToTarget());
            }
        }
    }

    public void OnMouseUp()
    {
        bool result = Upgrade();
        Debug.Log(result);
    }

    private void OnTriggerStay(Collider other)
    {

        Debug.Log("level + 1: " + level + ", childCnt : " + childCnt);
        
        if (level + 1 >= childCnt)
            isMaxUpgrade = true;
        else
            isMaxUpgrade = false;

        if (!isMaxUpgrade)
        {
            if (gm.Gold >= thisTowerTemplate.weapon[level].cost)
                isGoldEnough = true;
            else
                isGoldEnough = false;
        }
        
        
        if (other.gameObject.tag == "Player" && !isMaxUpgrade)
        {
            UpgradeInfoAppear();

            if (eDown && upgradeActivated)
            {
                bool result = Upgrade();
                Debug.Log(result);
                eDown = false;
            }

        }
        else if(other.gameObject.tag == "Player" && isMaxUpgrade)
        {
            MaxUpgradeInfoAppear();
        }
        else if (other.gameObject.tag == "Player" && !isGoldEnough)
        {
            NotEnoughGoldInfoAppear();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        UpgradeInfoDisappear();
    }



    private void Update()
    {
        eDown = Input.GetKeyDown(KeyCode.E);
        if (attackTarget != null && attackTargetDistance < towerAttackRange)
        {
            if ((towerIsLaser || !towerIsParticle) && !isMissile && !towerIsZangPan)
            {
                if (isMissile)
                {
                    Fire();
                }
                else
                {
                    StartCoroutine(LaserAttack());
                }
                StartCoroutine(MissileAttack());
            }
        }
        if (towerIsZangPan)
        {
            ZangPanGi(transform.position, towerAttackRange);
        }
    }
    
    // Todo : 최종 업그레이드 시 업그레이드 불가 메시지 띄우기
    private bool Upgrade()
    {
        
        if (level + 1< childCnt && gm.Gold >= thisTowerTemplate.weapon[level].cost)
        {
            transform.GetChild(level).gameObject.SetActive(false);
            level++;
            transform.GetChild(level).gameObject.SetActive(true);
            gm.Withdraw(thisTowerTemplate.weapon[level - 1].cost, false);
            SpecialTower();
            
            return true;
        }
        else
        {
            UpgradeInfoDisappear();
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
            //GetComponentInChildren<LookAtTarget>().SetUp(attackTarget);
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    private IEnumerator MissileAttack()
    {
        yield return new WaitForSeconds(towerAttackRate);
        isMissile = false;
        if (effect)
        {
            effect.Stop();
        }
    }

    private IEnumerator LaserAttack()
    {
        Debug.Log("멈춤");
        isMissile = false;
        yield return new WaitForSeconds(towerLaserRate);
        effect.Stop();
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
                    if (towerIsDeBuff > 0)
                    {
                        DeBuffAttack(enemy);
                    }
                    else
                    {
                        enemy.ProcessHit(towerDamage, towerHitEffect);
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

    private IEnumerator Laser()
    {
        Debug.Log("공격시작");
        while (attackTarget && !isMissile)
        {
            isMissile = true;
            effect.Play();
            
            RaycastHit[] hitInfo = Physics.RaycastAll(Spawner.transform.position, Spawner.transform.forward);
            if (hitInfo.Length > 0) 
            {
                foreach (RaycastHit hit in hitInfo)
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        effect.Play();
                        Enemy enemy = hit.collider.GetComponent<Enemy>();
                        if (towerIsDeBuff > 0)
                        {
                            DeBuffAttack(enemy);
                        }
                        else
                        {
                            enemy.ProcessHit(towerDamage, towerHitEffect);
                        }
                    }
                }

            }
            yield return new WaitForSeconds(towerAttackRate);
        }
        
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.ProcessHit(towerDamage, towerHitEffect);
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

                if (towerIsDeBuff > 0)
                {
                    DeBuffAttack(enemy);
                }
            }
            i++;
        }
    }

    void DeBuffAttack (Enemy enemy)
    {
        switch (towerIsDeBuff)
        {
            case 1:
                enemy.ProcessReduceSpeed(towerDamage, towerHitEffect);
                break;

            case 2:
                enemy.ProcessReduceArmor(towerDamage, towerHitEffect);
                break;
        }
    }


    private void UpgradeInfoAppear()
    {
        upgradeActivated = true;
        thisActionText.gameObject.SetActive(true);
        thisActionText.text = "타워 업그레이드 " + "<color=yellow>" + "(E)" + "</color>";
    }

    private void UpgradeInfoDisappear()
    {
        upgradeActivated = false;
        thisActionText.gameObject.SetActive(false);
    }


    private void MaxUpgradeInfoAppear()
    {

        thisActionText.gameObject.SetActive(true);
        thisActionText.text = "최대 업그레이드 입니다." + "<color=yellow>" + "</color>";
    }

    private void NotEnoughGoldInfoAppear()
    {

        thisActionText.gameObject.SetActive(true);
        thisActionText.text = "골드가 부족합니다." + "<color=yellow>" + "</color>";
    }
}

