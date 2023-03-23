using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GM : MonoBehaviour
{
    /*
    1. 씬 선택 기능

    2. 팝업 씬 선택 기능? (오브젝트에 OnClick으로 다는 게 나을지는 미정)

    3. 게임 알림 메시지 표시 기능
        - 곧 게임이 시작됩니다.
        - 다음 스테이지로 이동해 주세요.
        - 광산으로 가서 퀘스트를 진행해주세요.
        - ??님이 아이템을 사용했습니다!

    4. HUD에 표시할 변수
        - 스테이지, 웨이브 정보
        - 남은 시간
        - 남은 적 마릿수, 플레이어가 잡은 적 마릿수
        - 플레이어 소지 골드
        - 기지의 남은 체력
        - 플레이어의 포탑 설치 여부?

    5. 최종 결과 표시
        - 서버와 통신해서 방 안에 있는 플레이어들의 정보를 받아오기

    6. 추가로 저장해야할 정보들
        - killCount(Tower kill, Player kill)
        - 업적 저장할 공간
        - 그 외 인게임 결과값들..(생각해보기)
    */

    // 기타 변수

    // 1. 씬정보
    private string nowScene = "InGame";

    // 2. 팝업 여부(1개라도 띄워져 있으면 게임 화면은 모든 마우스, 키보드 입력 비활성화)
    private bool isPopup = false;

    // 3. 기지에서 가장 가까운 적의 위치(젤앞적transform.position)
    private List<Enemy> enemyList;
    private Transform enemyNavi;

    // 4. 방장의 게임 시작 여부
    private bool isGameStart = false;

    // 5. 내가 설치한 포탑의 위치
    private Transform myTowerNavi;

    // 6. 상대의 위치, 나와 다른 사람이 설치한 포탑의 위치?, 능력치, 골드 등(추가)

    // 7. 알림창 코멘트(이거 억덕하지 좀 빡세네)

    // 계정 정보
    private int userExp = 1;
    private int userLv = 1;

    // 유저 정보
    private string playerName = "TEST";
    private int characterIdx = 0;
    //default gold is 0
    private int gold = 1000;
    public int Gold { get { return gold; } }

    //deafult 1
    private int towerCnt = 10;
    public int TowerCnt { get { return towerCnt; } }
    private int killCnt = 0;

    // 캐릭터 status(일단 보류)(강화정보)


    // 게임 정보
    private int stage = 1;
    private List<GameObject> myItems;
    public List<GameObject> MyItems { get { return myItems; } }
    private int wave = 0;
    private int waveMobTotal = 10;
    private int life = 100;
    private int mobCnt = 0;
    private int timer = 100;
    private int finalWave = 30;

    GameObject startPoint;
    // SerializeField

    [SerializeField] GameObject stageText;
    [SerializeField] GameObject waveText;
    [SerializeField] GameObject waveMobTotalText;
    [SerializeField] GameObject lifeText;
    [SerializeField] GameObject mobCntText;
    [SerializeField] GameObject timerText;
    [SerializeField] GameObject towerCntText;
    [SerializeField] GameObject killCntText;
    [SerializeField] GameObject goldText;

    [Tooltip("캐릭터 선택 UI")]
    [SerializeField] GameObject CharacterSelectPrefab;
    [Tooltip("보상 UI")]
    [SerializeField] GameObject WaveClearPrefab;
    [Tooltip("결과 UI")]
    [SerializeField] GameObject ResultPrefab;
    [Tooltip("상호작용 UI")]
    [SerializeField] GameObject InteractionPrefeb;

    // 받아와야할 값들
    private void Awake()
    {
        startPoint = GameObject.FindGameObjectWithTag("StartPoint");
    }
    private void Start()
    {
        finalWave = startPoint.GetComponent<WaveSystem>().FinalWave;
        enemyList = startPoint.GetComponent<EnemySpawner>().EnemyList;
    }

    // 타워 설치
    public void BuildTower()
    {
        towerCnt -= 1;
        // towerCntText.GetComponent<TMP_Text>().text = towerCnt.ToString();
    }

    private void Update()
    {
        if (enemyList.Count > 0)
        {
            enemyNavi = enemyList[0].transform;
        }
    }

    // 돈 관리
    public void Withdraw(int amount, bool isPlus)
    {
        if (isPlus)
        {
            gold += amount;
        }
        else
        {
            gold -= Mathf.Abs(amount);
        }
        // goldText.GetComponent<TMP_Text>().text = gold.ToString();
    }

    // 게임 시작
    public void GameStart()
    {
        isGameStart = true;
        // timerText.GetComponent<TMP_Text>().text = "START";
        startPoint.GetComponent<WaveSystem>().StartWave();
    }

    // 몹 정보
    public void Deposit()
    {
        killCnt += 1;
        mobCnt -= 1;
        // killCntText.GetComponent<TMP_Text>().text = killCnt.ToString();
        // mobCntText.GetComponent<TMP_Text>().text = mobCnt.ToString();
    }

    // 생명력 감소
    public void LooseLife(int amount)
    {
        life -= Mathf.Abs(amount);

        if (life <= 0)
        {
            //게임오버 메소드 호출
        }
    }

    // 타이머
    private IEnumerator Timer()
    {
        while ( timer > 0)
        {
            timer -= 1;
            // timerText.GetComponent<TMP_Text>().text = timer.ToString();
            yield return new WaitForSeconds(1);
        }
        // timerText.GetComponent<TMP_Text>().text = "START";
        startPoint.GetComponent<WaveSystem>().StartWave();
    }

    // 타이머 시작
    private void StartTimer()
    {
        timer = 100;
        // timerText.GetComponent<TMP_Text>().text = timer.ToString();
        StartCoroutine("Timer");
    }

    // 웨이브 클리어
    private void ClearWave()
    {
        // 해당 웨이브 클리어, 보상 UI 출력, 최종웨이브 클리어시 게임셋
        if (wave < finalWave)
        {
            wave += 1;
            // waveText.GetComponent<TMP_Text>().text = wave.ToString();
            StartTimer();
        } else
        {
            GameEnd(true);
        }        
    }

    private void GameEnd(bool gameResult)
    {
        // 결과 UI 출력
        // gameResult 가 true면 게임을 클리어한 ending, false면 클리어 실패 ending
        if(gameResult)
        {
            // clear ending
        }
        else
        {
            // failure ending
        }
    }

    // 테스트용 버튼
    private void ClickBtn()
    {
        // 1 : 
        if (Input.GetKey(KeyCode.KeypadDivide))
        {
            Debug.Log("/");
        }

        // 0, -, = : 씬 전환
        if (Input.GetKey(KeyCode.Keypad0) && nowScene != "Login")
        {
            Debug.Log("씬전환");
            nowScene = "Login";
            SceneManager.LoadScene(6);
        }
        if (Input.GetKey(KeyCode.KeypadMinus) && nowScene != "InGame")
        {
            Debug.Log("씬전환");
            nowScene = "InGame";
            SceneManager.LoadScene(5);
        }
        if (Input.GetKey(KeyCode.KeypadEquals) && nowScene != "Main")
        {
            Debug.Log("씬전환");
            nowScene = "Main";
            SceneManager.LoadScene(4);
        }

        // 4 :
        if (Input.GetKey(KeyCode.KeypadMultiply))
        {
            Debug.Log("*");
        }

    }
}
