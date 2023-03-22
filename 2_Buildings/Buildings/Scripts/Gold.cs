using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Gold : MonoBehaviour
{
    GM gm;
    //ParticleSystem goldEffect;
    [SerializeField] GameObject goldPrefab;
    [SerializeField] int goldCount;
    [SerializeField] int income;
    GameObject goldClone;
    public int Income { get { return income; } set { income = value; } }

    Random randomObj = new Random();
    float randomX;
    float randomY;
    float randomZ;
    [SerializeField] int randomRange;

    [SerializeField] float goldUp;
    [SerializeField] float goldLifeTime = 3f;


    private void Start()
    {
        gm = GetComponent<GM>();
        //goldEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌");
        if (collision.gameObject.tag == "Player") 
        {
            Debug.Log("플레이어");
            if (goldCount > 0)
            { 
                Debug.Log("get gold");
                randomX = randomObj.Next(-randomRange, randomRange);
                randomY = randomObj.Next(0, randomRange);
                randomZ = randomObj.Next(-randomRange, randomRange);

                // need method (if Builder, double income)
                // goldEffect.Play();
                //Destroy(gameObject);
                gm = FindObjectOfType<GM>();

                Vector3 dir = new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z + randomZ);
                goldClone = Instantiate(goldPrefab, dir, Quaternion.identity);
                //goldClone.GetComponent<Rigidbody>().velocity = dir * goldUp;
                goldCount--;
                StartCoroutine(autoDestroy());
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator autoDestroy()
    {
        yield return new WaitForSeconds(goldLifeTime);
        Destroy(goldClone);
    }
}
