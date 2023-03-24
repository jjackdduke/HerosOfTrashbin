using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.XR;
using static Cinemachine.CinemachineTargetGroup;
using static TowerTemplate;
using static UnityEngine.GraphicsBuffer;

public class LookAtTarget : MonoBehaviour
{
    Transform LookingTarget = null;

    public void SetUp(Transform attackTarget)
    {
        this.LookingTarget = attackTarget;
    }

    private void Update()
    {
        if (LookingTarget != null)
        {
            transform.LookAt(LookingTarget);
        }
    }
    
    

}
