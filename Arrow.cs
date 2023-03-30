using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    GameObject gameObject;
    PlayerStat playerStat;
    Weapon weapon;
    public float damage;
    public ParticleSystem hitEffect;

    void Start()
    {
        gameObject = GameObject.Find("ArcherPlayerBody");
        playerStat = gameObject.GetComponent<PlayerStat>();
        weapon = gameObject.GetComponent<Weapon>();
        Debug.Log("캐릭터 스탯 : " + playerStat.CurrentDamage);
        Debug.Log("무기 데미지 : " + weapon.damage);
        damage = playerStat.CurrentDamage + weapon.damage;
        weapon.trailEffect = weapon.arrow.GetComponentInChildren<TrailRenderer>();

    }


    void Update()
    {
        transform.Translate(Vector3.forward * 1f);
    }





}
