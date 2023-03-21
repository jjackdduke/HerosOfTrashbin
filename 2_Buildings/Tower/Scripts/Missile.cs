using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Missile : MonoBehaviour
{
    Rigidbody rigidBody = null;
    [SerializeField] private Transform thisTarget = null;
    float thisDamage;

    [SerializeField]  private float thisMissileSpeed = 0f;
    float currentSpeed = 0f;
    float thisMissileWaitSecond = 0f;
    [SerializeField] ParticleSystem thisEffect = null;

    TargetLocator missileTurret;
    bool fire = false;
    bool thisDeBuff = false;

    public void SetUp(Transform attackTarget, float damage, float missileSpeed, float missileWaitSecond, bool deBuff)
    {
        this.thisTarget = attackTarget;
        this.thisDamage = damage;
        this.thisMissileSpeed = missileSpeed;
        this.thisMissileWaitSecond = missileWaitSecond;
        this.thisDeBuff = deBuff;
    }

    IEnumerator LaunchDelay()
    {
        yield return new WaitUntil(() => rigidBody.velocity.y < 0f);
        yield return new WaitForSeconds(thisMissileWaitSecond);
        fire = Aimweapon();

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);

    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (thisEffect != null)
        {
            thisEffect.Play();
        }
        StartCoroutine(LaunchDelay());
    }

    void Update()
    {
        if (thisTarget)
        {
            if (currentSpeed <= thisMissileSpeed)
                currentSpeed += thisMissileSpeed * Time.deltaTime;

            transform.position += transform.up * currentSpeed * Time.deltaTime;

            Vector3 t_dir = (thisTarget.position - transform.position).normalized;
            transform.up = Vector3.Lerp(transform.up, t_dir, 0.25f);

        }
        else
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
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
            thisTarget = closestTarget;
        }

    }

    bool Aimweapon()
    {
        if (thisTarget)
        {
            
            float targetDistance = Vector3.Distance(transform.position, thisTarget.position);

            missileTurret = FindObjectOfType<TargetLocator>();
            if (targetDistance < missileTurret.attackRange)
            {
                return true;
            }
            return false;
        }
        return false;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (thisDeBuff)
            {
                enemy.ProcessReduceSpeed(thisDamage);
            }   
            else
            {
                enemy.ProcessHit(thisDamage);
            }
        }
    }

}
