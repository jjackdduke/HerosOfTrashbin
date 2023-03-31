using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 적 프리펩
    [SerializeField] private GameObject[] enemyPrefabs;
    // 스페셜 몹 프리펩
    [SerializeField] private GameObject[] specialPrefabs;

    // 임의 시작용 wavesystem
    [SerializeField] private GameObject startt;
    
    // 적 체력을 나타내는 UI프리펩
    // [SerializeField] private GameObject enemyHPSliderPrefab;

    // UI를 표현하는 Canvas 오브젝트의 transform
    // [SerializeField] private Transform canvasTransform;

    // 현재 웨이브 정보
    private Wave currentWave;
    public Wave CurrentWave { get { return currentWave; } }

    // 현재 맵에 존재하는 모든 적의 정보
    private List<Enemy> enemyList;

    // 적의 생성과 삭제는 EnemySpawner에서 하기 때문에 Set은 필요 없다.
    public List<Enemy> EnemyList => enemyList;

    // GM
    GM gm;

    // 플레이어 수
    private int playerCnt = 1;

    // 적 생성 시 파티클
    [SerializeField] ParticleSystem SpawnEffect;

    private void Awake()
    {
        // 적 리스트 메모리 할당
        enemyList = new List<Enemy>();
        gm = FindObjectOfType<GM>();
        Debug.Log(gm.name + "나오나");
    }

    public void StartWave(Wave wave, int idx)
    {
        // 매개변수로 받아온 웨이브 정보 저장
        currentWave = wave;

        // 현재 웨이브 시작
        StartCoroutine("SpawnEnemy", idx);
    }

    public void StartSpecialWave(SpecialWave SpecialWave, int idx)
    {
        GameObject clone = Instantiate(specialPrefabs[idx], transform);
        Enemy enemy = clone.GetComponent<Enemy>();
        enemy.transform.GetChild(0).transform.localScale *= 2f;
        enemy.pathString = SpecialWave.pathString;
        enemy.mobHP = SpecialWave.mobHP * playerCnt;
        enemy.lifePenalty = SpecialWave.lifePenalty;
        enemy.speed = SpecialWave.speed;
        enemy.armor = SpecialWave.armor;
        enemy.isBoss = true;
        enemy.skillIdx = SpecialWave.skillIdx;
        enemy.coolDown = SpecialWave.coolDown;
        enemy.range = SpecialWave.range;
        enemy.power = SpecialWave.power;
        enemyList.Add(enemy);
        gm.MobCounter(true);
    }

    private IEnumerator SpawnEnemy(int idx)
    {
        int spawnEnemyCount = 0;

        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {   
           // 몹 바꾸려면 여기서 바꿔주면 됨
            GameObject clone = Instantiate(enemyPrefabs[idx], transform);
            Enemy enemy = clone.GetComponent<Enemy>();
            enemy.pathString = currentWave.pathString;
            enemy.mobHP = currentWave.mobHP * playerCnt;
            //enemy.isBoss = currentWave.isBoss;
            //if (enemy.isBoss) { enemy.transform.GetChild(0).transform.localScale *= 2f; }
            enemy.lifePenalty = currentWave.lifePenalty;
            enemy.speed = currentWave.speed;
            enemy.armor = currentWave.armor;

            enemyList.Add(enemy);
            spawnEnemyCount++;
            Instantiate(SpawnEffect, clone.transform.position, Quaternion.identity);
            Debug.Log("여기올텐데");
            gm.MobCounter(true);
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
        gm.isEndWave = true;
        // 임시방편으로 이렇게 해놈
        // enemyList = new List<Enemy>();
    }

    public void DestroyEnemy(Enemy enemy, bool isKill = true)
    {
        // 리스트에서 사망하는 적 정보 삭제
        enemyList.Remove(enemy);
        // 적 오브젝트 삭제
        if (isKill)
        {
            Destroy(enemy.gameObject, 0.8f);
        }
    }

    // 스타트 버튼 없을때만 쓰는거
    //private void Update()
    //{
    //    if (enemyList.Count == 0)
    //    {
    //        startt.GetComponent<WaveSystem>().StartWave();
    //    }
    //}
}
