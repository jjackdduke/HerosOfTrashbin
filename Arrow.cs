using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    GameObject gameObject;
    ArcherPlayer playerStat;
    Weapon weapon;
    public float damage;

    void Awake()
    {
        gameObject = GameObject.Find("ArcherPlayerBody");
        playerStat = gameObject.GetComponent<ArcherPlayer>();
        gameObject = GameObject.Find("Bows");
        weapon = gameObject.GetComponent<Weapon>();
        damage = playerStat.CurrentDamage + weapon.damage;

    }


    void Update()
    {
       
    }





}
