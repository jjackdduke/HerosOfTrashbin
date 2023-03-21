using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using Random = System.Random;

public class ObjectPool : MonoBehaviour
{
    // 몬스터 프리팹
    [SerializeField] GameObject enemyPrefab;

    // 맵에 존재할 최대 몬스터 수
    [SerializeField] [Range(0, 50)] int poolSize = 30;
    GameObject[] pool;

    // 몬스터 스폰타임 조절 변수
    [SerializeField] [Range(0.1f, 30f)] float spawnTimer = 5f;

    // 무작위 수 생성 변수
    Random randomObj = new Random();
    int randomValue;


    void Awake()
    {
        PopulatePool();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());   
    }

    // 몬스터 인스턴스 생성
    void PopulatePool()
    {
        // poolSize만큼의 갯수를 가진 pool 배열 선언
        pool = new GameObject[poolSize];
        for (int i = 0; i < pool.Length; i++)
        {
            // 비활성화 상태로 몬스터 인스턴스 일괄 생성
            // 현재 사용하는 기능: 개체(enemyPrefab)를 인스턴스화하고 새 개체에 부모(transform) 설정
            pool[i] = Instantiate(enemyPrefab, transform);
            pool[i].SetActive(false);
        }
    }

    // 몬스터 활성화
    void EnableObjectInPool()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i] != null)
            {
                if (pool[i].activeInHierarchy == false)
                {
                    pool[i].SetActive(true);
                    pool[i].transform.position = transform.position;
                    return;
                }
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            EnableObjectInPool();
            randomValue = randomObj.Next(1, 10);
            // 무작위 숫자로 나누어 리스폰 속도 변칙 부여
            yield return new WaitForSeconds(spawnTimer / randomValue);
        }
    }

 
}
