using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 적 프리펩
    [SerializeField] private GameObject enemyPrefab;

    // 임의 시작용 wavesystem
    [SerializeField] private GameObject startt;
    
    // 적 체력을 나타내는 UI프리펩
    // [SerializeField] private GameObject enemyHPSliderPrefab;

    // UI를 표현하는 Canvas 오브젝트의 transform
    // [SerializeField] private Transform canvasTransform;

    // 현재 웨이브 정보
    private Wave currentWave;

    // 현재 맵에 존재하는 모든 적의 정보
    public List<Enemy> enemyList;

    // 적의 생성과 삭제는 EnemySpawner에서 하기 때문에 Set은 필요 없다.
    public List<Enemy> EnemyList => enemyList;

    private void Awake()
    {
        // 적 리스트 메모리 할당
        enemyList = new List<Enemy>();
    }

    public void StartWave(Wave wave)
    {
        // 매개변수로 받아온 웨이브 정보 저장
        currentWave = wave;

        // 현재 웨이브 시작
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;

        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {   
           // 몹 바꾸려면 여기서 바꿔주면 됨
            GameObject clone = Instantiate(currentWave.enemyPrefabs[0], transform);
            Enemy enemy = clone.GetComponent<Enemy>();
            //enemy.pathString = currentWave.pathString;
            enemy.mobHP = currentWave.mobHP;
            enemy.isBoss = currentWave.isBoss;
            enemy.lifePenalty = currentWave.lifePenalty;
            enemy.speed = currentWave.speed;
            enemy.armor = currentWave.armor;

            enemyList.Add(enemy);
            spawnEnemyCount++;

            yield return new WaitForSeconds(currentWave.spawnTime);
        }
        // 임시방편으로 이렇게 해놈
        // enemyList = new List<Enemy>();
    }

    public void DestroyEnemy(Enemy enemy)
    {
        // 리스트에서 사망하는 적 정보 삭제
        enemyList.Remove(enemy);
        // 적 오브젝트 삭제
        Destroy(enemy.gameObject);
    }

    // 스타트 버튼 없을때만 쓰는거
    private void Update()
    {
        if (enemyList.Count == 0)
        {
            startt.GetComponent<WaveSystem>().StartWave();
        }
    }
}
