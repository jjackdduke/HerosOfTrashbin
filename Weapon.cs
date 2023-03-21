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
 



    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Whirlwind");
            StartCoroutine("Swing");
        }
            
    }

    public void UseSkill()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Whirlwind");

        }
    }

    // Use() 메인루틴 -> Swing() 서브루틴 -> Use() 메인루틴
    // Use() 메인루틴 + Swing() 코루틴
    IEnumerator Swing()
    {
        // yield 키워드를 여러 개 사용하여 시간차 로직 작성 가능
        //1
        yield return new WaitForSeconds(0.1f); // 0.1 초 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        //2
        yield return new WaitForSeconds(0.7f); 
        meleeArea.enabled = false;
        //3  
        yield return new WaitForSeconds(0.1f);  
        trailEffect.enabled = false;

    }



    IEnumerator Whirlwind()
    {
        // yield 키워드를 여러 개 사용하여 시간차 로직 작성 가능
        //1
        
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        yield return new WaitForSeconds(3.8f);


        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.1f);
        trailEffect.enabled = false;
        
        
    }




}
