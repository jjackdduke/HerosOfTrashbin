using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    
    // 다음 레벨로 넘어갈 때의 지연시간
    [SerializeField] float levelLoadDelay = 1f;
    
    // 생명력
    [SerializeField] int lifes = 20;

    // 시작 게임머니
    [SerializeField] int startingBalance = 120;

    // 타워 배치 잔여 횟수
    [SerializeField] int towerTO = 2;
    public int TowerTO { get { return towerTO; } }

    // 킬카운트
    [SerializeField] int kills = 0;

    // 남은 적 수
    [SerializeField] int lefts = 15;

    // 현재 게임머니
    [SerializeField] int currentBalance;

    // 현재 웨이브
    [SerializeField] public int wave = 1;

    public int CurrentBalance{ get { return currentBalance;} }

    // UI 표시용 변수들 (순서대로 잔여 게임머니, 잔여 생명력, 킬카운트, 남은 적 수)
    [SerializeField] TextMeshProUGUI displayBalance;
    [SerializeField] TextMeshProUGUI displayTowerTO;
    [SerializeField] TextMeshProUGUI displayLifes;
    [SerializeField] TextMeshProUGUI displayKills;
    [SerializeField] TextMeshProUGUI displayLefts;
    [SerializeField] TextMeshProUGUI displayWave;    

    void Awake()
    {
        // 현재 게임머니 초기화하고 화면 표시
        currentBalance = startingBalance;
        UpdateDisplay();
    }

    void Update() {
        // 치트키 메소드 호출
        RespondToDebugKeys();
    }

    void RespondToDebugKeys() {
        // L 누르면 바로 다음 웨이브
        if (Input.GetKeyDown(KeyCode.L)) {
            NextLevel();
        }
    }

    // 사냥 성공 시 
    //public void Deposit(int amount)
    public void Deposit ()
    {
        //currentBalance += Mathf.Abs(amount);
        kills += 1;
        lefts -= 1;
        UpdateDisplay();
        if (lefts == 0) 
        {
            // 다 잡았을 때의 메소드 호출
            StartSuccessSequence();
        }
    }

    // 타워 설치
    public void BuildTower()
    {
        towerTO -= 1;
        UpdateDisplay();
    }

    // 잔액에서 인출
    public void Withdraw (int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        UpdateDisplay();
    }

    // 생명력 감소
    public void LooseLife (int amount)
    {
        lifes -= Mathf.Abs(amount);
        UpdateDisplay();

        if (lifes <= 0)
        {
            //게임오버 메소드 호출
            ReloadLevel();
        }
    }

    // 화면표시 갱신
    void UpdateDisplay()
    {
        // 테스트를 위해 글자 표기 제거 (영준)
        displayBalance.text = currentBalance.ToString();
        displayTowerTO.text = towerTO.ToString();
        displayLifes.text = lifes.ToString();
        displayKills.text = kills.ToString();
        displayLefts.text = lefts.ToString();
        displayWave.text = wave.ToString();
    }

    // 클리어 시 실행되는 메소드
    void StartSuccessSequence() 
    {
        // audioSource.Stop();
        // audioSource.PlayOneShot(success);
        // successParticles.Play();
        Invoke("NextLevel", levelLoadDelay);
        wave ++;
        UpdateDisplay();
    }

    // 게임오버 시 바로 재시작
   void ReloadLevel() 
   {
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    SceneManager.LoadScene(currentSceneIndex);
        wave = 1;
        UpdateDisplay();
    }

    // 다음 씬으로 넘어감. 현재 씬이 마지막일 경우 처음 씬으로 돌아감
   void NextLevel() 
   {
    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    int nextScneneIndex = currentSceneIndex + 1;
    if (nextScneneIndex == SceneManager.sceneCountInBuildSettings) {
        nextScneneIndex = 0;
    }
    SceneManager.LoadScene(nextScneneIndex);
   }
}
