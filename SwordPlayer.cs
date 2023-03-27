using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPlayer : PlayerStat
{
    // 占싯삼옙 占쏙옙占쏙옙
    private float SwordManDamage = 10f;
    private float SwordManSpeed = 20f;


    void Start()
    {
        playerDamage = SwordManDamage;
        playerSpeed = SwordManSpeed;
    }


}
