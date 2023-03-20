using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // �� ������
    [SerializeField] private GameObject enemyPrefab;
    
    // �� ü���� ��Ÿ���� UI������
    // [SerializeField] private GameObject enemyHPSliderPrefab;

    // UI�� ǥ���ϴ� Canvas ������Ʈ�� transform
    // [SerializeField] private Transform canvasTransform;

    // ���� ���̺� ����
    private Wave currentWave;

    // ���� �ʿ� �����ϴ� ��� ���� ����
    public List<Enemy> enemyList;

    // ���� ������ ������ EnemySpawner���� �ϱ� ������ Set�� �ʿ� ����.
    public List<Enemy> EnemyList => enemyList;

    private void Awake()
    {
        // �� ����Ʈ �޸� �Ҵ�
        enemyList = new List<Enemy>();
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
            GameObject clone = Instantiate(enemyPrefab, transform);
            Enemy enemy = clone.GetComponent<Enemy>();

            enemyList.Add(enemy);
            spawnEnemyCount++;

            yield return new WaitForSeconds(currentWave.spawnTime);
        }
        Debug.Log("���̺� ��ȯ �Ϸ�");
        // �ӽù������� �̷��� �س�
        enemyList = new List<Enemy>();
    }

    public void DestroyEnemy(Enemy enemy)
    {
        Debug.Log("�� ����!!!!");
        // ����Ʈ���� ����ϴ� �� ���� ����
        enemyList.Remove(enemy);
        // �� ������Ʈ ����
        Destroy(enemy.gameObject);
    }
}
