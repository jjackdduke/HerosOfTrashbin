using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{

    // 캐릭터 status


    // 외부 스크립트에서 접근하는 변수들
    public float CurrentDamage { get { return playerDamage; } }
    public float CurrentSpeed { get { return playerSpeed; } }


    public float playerDamage;
    public float playerSpeed;


    

    // Update is called once per frame
    void Update()
    {
        
    }
}
