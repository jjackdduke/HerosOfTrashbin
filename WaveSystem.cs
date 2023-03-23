using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    // ���� ���������� ��� ���̺� ����
    [SerializeField] private Wave[] waves;
    public int FinalWave { get { return waves.Length; } }

    [SerializeField] private EnemySpawner enemySpawner;
    
    // ���� ���̺� �ε���
    private int currentWaveIndex = -1;
    // ���� ���̺� ����
    public int CurrentWave { get { return currentWaveIndex + 2; } }

    public void StartWave()
    {
        // ���� �ʿ� ���� ����, Wave�� ����������
        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1)
        {
            // �ε����� ������ -1�̱� ������ ���̺� �ε��� ������ ���� ���� ��
            currentWaveIndex++;
            // EnemySpawner�� Startwave() �Լ� ȣ��, ���� ���̺� ���� ����
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
    }
}

// ���̺� Ŀ���� �ý���
[System.Serializable]
public struct Wave
{
    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
    public int mobHP;
    public bool isBoss;
    public string pathString;
    public int lifePenalty;
    public float speed;
    public float armor;

}