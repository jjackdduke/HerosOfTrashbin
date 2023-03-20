using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    // 현재 스테이지의 모든 웨이브 정보
    [SerializeField] private Wave[] waves;

    [SerializeField] private EnemySpawner enemySpawner;
    
    // 현재 웨이브 인덱스
    private int currentWaveIndex = -1;

    public void StartWave()
    {
        // 현재 맵에 적이 없고, Wave가 남아있으면
        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1)
        {
            // 인덱스의 시작이 -1이기 때문에 웨이브 인덱스 증가를 제일 먼저 함
            currentWaveIndex++;
            Debug.Log(currentWaveIndex);
            Debug.Log("웨이브 인덱스와 길이");
            Debug.Log(waves.Length);
            // EnemySpawner의 Startwave() 함수 호출, 현재 웨이브 정보 제공
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
        else
        {
            Debug.Log("아직 다음단계 실행 못함");
            Debug.Log(enemySpawner.EnemyList.Count);
        }
    }
}

[System.Serializable]
public struct Wave
{
    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
}
