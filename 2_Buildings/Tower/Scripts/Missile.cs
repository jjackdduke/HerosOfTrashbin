using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Missile : MonoBehaviour
{
    Rigidbody rigidBody = null;
    [SerializeField] private Transform target = null;
    private float Damage;

    [SerializeField]  private float missileSpeed = 0f;
    float currentSpeed = 0f;
    private float missileWaitSecond = 0f;
    [SerializeField] ParticleSystem Effect = null;

    TargetLocator missileTurret;
    private bool fire = false;

    public void SetUp(Transform attackTarget, float damage, float missileSpeed, float missileWaitSecond)
    {
        this.target = attackTarget;
        this.Damage = damage;
        this.missileSpeed = missileSpeed;
        this.missileWaitSecond = missileWaitSecond;
    }

    IEnumerator LaunchDelay()
    {
        yield return new WaitUntil(() => rigidBody.velocity.y < 0f);
        yield return new WaitForSeconds(missileWaitSecond);
        fire = Aimweapon();

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);

    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        Effect.Play();
        StartCoroutine(LaunchDelay());
    }

    void Update()
    {
        if (target)
        {
            if (currentSpeed <= missileSpeed)
                currentSpeed += missileSpeed * Time.deltaTime;

            transform.position += transform.up * currentSpeed * Time.deltaTime;

            Vector3 t_dir = (target.position - transform.position).normalized;
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
            target = closestTarget;
        }

    }

    bool Aimweapon()
    {
        if (target)
        {
            
            float targetDistance = Vector3.Distance(transform.position, target.position);

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
            EnemyHP enemyHp = collision.gameObject.GetComponent<EnemyHP>();
            enemyHp.ProcessHit(Damage);
        }
    }

}
