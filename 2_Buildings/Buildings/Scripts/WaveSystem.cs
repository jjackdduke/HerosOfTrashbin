using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    // ���� ���������� ��� ���̺� ����
    [SerializeField] private Wave[] waves;

    [SerializeField] private EnemySpawner enemySpawner;
    
    // ���� ���̺� �ε���
    private int currentWaveIndex = -1;

    public void StartWave()
    {
        // ���� �ʿ� ���� ����, Wave�� ����������
        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1)
        {
            // �ε����� ������ -1�̱� ������ ���̺� �ε��� ������ ���� ���� ��
            currentWaveIndex++;
            Debug.Log(currentWaveIndex);
            Debug.Log("���̺� �ε����� ����");
            Debug.Log(waves.Length);
            // EnemySpawner�� Startwave() �Լ� ȣ��, ���� ���̺� ���� ����
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
        else
        {
            Debug.Log("���� �����ܰ� ���� ����");
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
