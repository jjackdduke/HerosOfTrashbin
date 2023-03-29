using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public enum Type { Melee, Range};
    public Type type;

    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    Animator anim; // Melee 일 경우: 검, Range일 경우: 화살
    Animator anim2;// Range일 경우: 활
    GameObject bow;
    GameObject arrows;
    public ParticleSystem hitEffect;
    public Transform arrowPos;
    public GameObject arrow;


    private void Awake()
    {
        if(type == Type.Range)
        {
            arrows = GameObject.Find("Arrows");
            anim = arrows.GetComponent<Animator>();
            bow = GameObject.Find("Bows");
            anim2 = bow.GetComponent<Animator>();
        }else if(type == Type.Melee)
        {
            anim = GetComponentInParent<Animator>();
        }
        
    }



    public void Use()
    {
        if (type == Type.Melee)
        {
        }

        if(type == Type.Range)
        {
            StartCoroutine("Shot");
        }
            
    }

    public void UseSkill()
    {
        if(type == Type.Melee)
        {
            anim.SetBool("IsWhirlwind", true);
        }
    }

    // Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���η�ƾ
    // Use() ���η�ƾ + Swing() �ڷ�ƾ

    IEnumerator Shot()
    {

        //#1 총알 발사
        GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward * 50;
        yield return new WaitForSeconds(3f);

        Destroy(instantArrow);

    }


    // 궁수 애니메이션
    public void fireAnimation()
    {

        anim.SetBool("IsFire", true);
        anim2.SetBool("IsFire", true);
    }

    public void fireEndAnimation()
    {
        anim.SetBool("IsFire", false);
        anim2.SetBool("IsFire", false);
    }


    // 소드맨 애니메이션
    public void SwingAnimation()
    {
        meleeArea.enabled = true;
        trailEffect.enabled = true;

    }

    public void SwingEndAnimation()
    {
        anim.SetBool("IsFire", false);
        anim2.SetBool("IsFire", false);
        meleeArea.enabled = false;
        trailEffect.enabled = false;
    }

    public void WhirlwindAnimation()
    {
        meleeArea.enabled = true;
        trailEffect.enabled = true;
    }

    public void WhirlwindEndAnimation()
    {
        meleeArea.enabled = false;
        trailEffect.enabled = false;
        anim.SetBool("IsWhirlwind", false);
    }



}
