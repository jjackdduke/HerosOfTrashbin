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
    private string nowScene = "InGame";

    // 유저 정보
    private string playerName;
    private int gold = 0;
    public int Gold { get { return gold; } }

    private int towerCnt = 1;
    public int TowerCnt { get { return towerCnt; } }
    private int killCnt = 0;

    // 게임 정보
    private int stage = 1;
    private int wave = 1;
    private int endWave;
    private int life;
    private int mobCnt = 0;
    private int timer = 100;


    // 받아와야할 값들
    private void Start()
    {
        endWave = GetComponent<WaveSystem>().EndWave;
    }


    // 타워 설치
    public void BuildTower()
    {
        towerCnt -= 1;
        // UI 관리 함수 호출
    }

    // 돈 쓰는거
    public void Withdraw(int amount)
    {
        gold -= Mathf.Abs(amount);
        // UI 관리 함수 호출
    }

    // 사냥 정보
    public void Deposit()
    {
        killCnt += 1;
        mobCnt -= 1;
        // UpdateDisplay();
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
    public IEnumerator Timer()
    {
        while ( timer > 0)
        {
            timer -= 1;
            // 디스플레이 업데이트
            yield return new WaitForSeconds(1);
        }
        timer = 100;
    }

    // 타이머 시작
    public void StartTimer()
    {
        StartCoroutine("Timer");
    }

    // 웨이브 클리어
    private void ClearWave()
    {

    }

    // UI관리자
    private void UpdateUI()
    {
        // 타워 설치
        // 돈 쓰는거
        // 사냥 정보
        // 생명력 감소
        // 타이머
    }

    private void GameEnd(bool gameResult)
    {
        // 결과 UI 출력
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
