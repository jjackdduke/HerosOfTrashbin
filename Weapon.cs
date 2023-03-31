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
    public Transform[] multiShotPos;
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
        }else if(type == Type.Range)
        {
            
        }
    }

    // Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���η�ƾ
    // Use() ���η�ƾ + Swing() �ڷ�ƾ

    IEnumerator Shot()
    {

        //#1 총알 발사
        GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
        
        yield return new WaitForSeconds(3f); // 3초 뒤에 자동으로 파괴됨

        Destroy(instantArrow);

    }

    IEnumerator MultiShot()
    {
        GameObject[] multiShotArrow = new GameObject[7];
        for (int pos = 0; pos < multiShotPos.Length; pos++)
        {
            multiShotArrow[pos] = Instantiate(arrow, multiShotPos[pos].position, multiShotPos[pos].rotation);
        }

        yield return new WaitForSeconds(1f);

        for (int pos = 0; pos < multiShotPos.Length; pos++)
        {
            Destroy(multiShotArrow[pos]); 
        }

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


    public void MultiShotAnim_Start()
    {
        StartCoroutine("MultiShot");
    }


    


    // 소드맨 애니메이션
    public void SwingAnimation()
    {
        meleeArea.enabled = true;
        trailEffect.enabled = true;

    }

    public void SwingEndAnimation()
    {
        
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
