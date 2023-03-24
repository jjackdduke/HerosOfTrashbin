using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPlayer : PlayerStat
{
    // ∞ÀªÁ Ω∫≈»
    private float SwordManDamage = 10f;
    private float SwordManSpeed = 20f;

    void Start()
    {
        playerDamage = SwordManDamage;
        playerSpeed = SwordManSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
