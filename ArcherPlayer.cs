using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherPlayer : PlayerStat
{
    // Start is called before the first frame update
    private float ArcherDamage = 5f;
    private float ArcherSpeed = 30f;


    void Start()
    {
        playerDamage = ArcherDamage;
        playerSpeed = ArcherSpeed;
    }
}
