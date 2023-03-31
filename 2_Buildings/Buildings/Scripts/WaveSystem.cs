using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    // 현재 스테이지의 모든 웨이브 정보
    [SerializeField] private Wave[] waves;
    public Wave[] AllWaveInfos { get { return waves; } }

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Invest invest;
    private GameObject stageObejct;

    // 현재 웨이브 인덱스
    private int currentWaveIndex = -1;
    // 현재 웨이브 참조

    private void Awake()
    {
        stageObejct = GameObject.Find("Stages").gameObject;
        invest = stageObejct.transform.Find("Stage2/EventBuildings/StockBuilding/Event_4_Stock").GetComponent<Invest>();
    }

    public void StartWave()
    {
        // 현재 맵에 적이 없고, Wave가 남아있으면
        if (currentWaveIndex < waves.Length - 1)
        {
            // 인덱스의 시작이 -1이기 때문에 웨이브 인덱스 증가를 제일 먼저 함
            currentWaveIndex++;
            // EnemySpawner의 Startwave() 함수 호출, 현재 웨이브 정보 제공
            enemySpawner.StartWave(waves[currentWaveIndex]);
            invest.ThisWave(currentWaveIndex);

        }
    }
}

// 웨이브 커스텀 시스템
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