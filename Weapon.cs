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
    Animator anim;
    Animator anim2;
    GameObject bow;
    

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        bow = GameObject.Find("Bows");
        anim2 = bow.GetComponent<Animator>();
        Debug.Log(bow);
    }



    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Whirlwind");
            StartCoroutine("Swing");
        }

        if(type == Type.Range)
        {
            StartCoroutine("Shoot");
        }
            
    }

    public void UseSkill()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Whirlwind");
            anim.SetBool("IsWhirlwind", false);

        }
    }

    // Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���η�ƾ
    // Use() ���η�ƾ + Swing() �ڷ�ƾ
    IEnumerator Swing()
    {
        // yield Ű���带 ���� �� ����Ͽ� �ð��� ���� �ۼ� ����
        //1
        yield return new WaitForSeconds(0.1f); // 0.1 �� ���
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        //2
        yield return new WaitForSeconds(0.2f); 
        meleeArea.enabled = false;
        //3  
        yield return new WaitForSeconds(0.05f);  
        trailEffect.enabled = false;

    }



    IEnumerator Whirlwind()
    {
        // yield Ű���带 ���� �� ����Ͽ� �ð��� ���� �ۼ� ����
        //1
        
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        yield return new WaitForSeconds(3.8f);


        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.1f);
        trailEffect.enabled = false;
        

    }

    IEnumerator Shoot()
    {
        anim.SetBool("IsFire", true);
        anim2.SetBool("IsFire", true);
        yield return new WaitForSeconds(0.2f); // 0.1 �� ���
        

        trailEffect.enabled = true;

        anim.SetBool("IsFire", false);
        anim2.SetBool("IsFire", false);
        yield return new WaitForSeconds(0.5f); // 0.1 �� ���
        trailEffect.enabled = false;

    }




}
