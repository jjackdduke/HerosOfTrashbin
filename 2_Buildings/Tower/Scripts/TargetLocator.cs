using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.XR;
using static Cinemachine.CinemachineTargetGroup;
using static TowerTemplate;
using static UnityEngine.GraphicsBuffer;

public class TargetLocator : MonoBehaviour
{
    Transform attackTarget = null;
    private float attackTargetDistance;
    public float attackRange;

    public void SetUp(Transform attackTarget, float attackTargetDistance, float attackRange)
    {
        this.attackTarget = attackTarget;
        this.attackTargetDistance = attackTargetDistance;
        this.attackRange = attackRange;
    }


    private void Update()
    {
        if (attackTarget != null)
        {
            RotateToTarget();
        }
    }
    
    private void RotateToTarget()
    {
        if (attackTargetDistance > attackRange)
        {
            Quaternion spin = Quaternion.Euler(0f, transform.eulerAngles.y + (1f * Time.deltaTime), 0f);
            transform.rotation = spin;
        }
        else
        {

            float dx = attackTarget.position.x - transform.position.x;
            float dz = attackTarget.position.z - transform.position.z;
            Vector3 dir = new Vector3(dx, 0, dz);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime);

        }
    }
    

}
