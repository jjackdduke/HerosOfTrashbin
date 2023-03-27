using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // �� ������
    [SerializeField] private GameObject enemyPrefab;

    // ���� ���ۿ� wavesystem
    [SerializeField] private GameObject startt;
    
    // �� ü���� ��Ÿ���� UI������
    // [SerializeField] private GameObject enemyHPSliderPrefab;

    // UI�� ǥ���ϴ� Canvas ������Ʈ�� transform
    // [SerializeField] private Transform canvasTransform;

    // ���� ���̺� ����
    private Wave currentWave;
    public Wave CurrentWave;

    // ���� �ʿ� �����ϴ� ��� ���� ����
    private List<Enemy> enemyList;

    // ���� ������ ������ EnemySpawner���� �ϱ� ������ Set�� �ʿ� ����.
    public List<Enemy> EnemyList => enemyList;

    // GM
    GM gm;

    private void Awake()
    {
        // �� ����Ʈ �޸� �Ҵ�
        enemyList = new List<Enemy>();
        gm = FindObjectOfType<GM>();
    }

    public void StartWave(Wave wave)
    {
        // �Ű������� �޾ƿ� ���̺� ���� ����
        currentWave = wave;

        // ���� ���̺� ����
        StartCoroutine("SpawnEnemy");
    }

    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;

        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {   
           // �� �ٲٷ��� ���⼭ �ٲ��ָ� ��
            GameObject clone = Instantiate(currentWave.enemyPrefabs[0], transform);
            Enemy enemy = clone.GetComponent<Enemy>();
            enemy.pathString = currentWave.pathString;
            enemy.mobHP = currentWave.mobHP;
            enemy.isBoss = currentWave.isBoss;
            if (enemy.isBoss) { enemy.transform.GetChild(0).transform.localScale *= 2f; }
            enemy.lifePenalty = currentWave.lifePenalty;
            enemy.speed = currentWave.speed;
            enemy.armor = currentWave.armor;

            enemyList.Add(enemy);
            spawnEnemyCount++;

            gm.MobCounter(true);
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
        gm.isEndWave = true;
        // �ӽù������� �̷��� �س�
        // enemyList = new List<Enemy>();
    }

    public void DestroyEnemy(Enemy enemy)
    {
        // ����Ʈ���� ����ϴ� �� ���� ����
        enemyList.Remove(enemy);
        // �� ������Ʈ ����
        Destroy(enemy.gameObject, 0.8f);
    }

    // ��ŸƮ ��ư �������� ���°�
    //private void Update()
    //{
    //    if (enemyList.Count == 0)
    //    {
    //        startt.GetComponent<WaveSystem>().StartWave();
    //    }
    //}
}