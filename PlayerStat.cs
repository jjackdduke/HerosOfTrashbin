using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{

    // ĳ���� status


    // �ܺ� ��ũ��Ʈ���� �����ϴ� ������
    public float CurrentDamage { get { return playerDamage; } }
    public float CurrentSpeed { get { return playerSpeed; } }


    public float playerDamage;
    public float playerSpeed;


    

    // Update is called once per frame
    void Update()
    {
        
    }
}
