using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GM : MonoBehaviour
{
    /*
    1. �� ���� ���

    2. �˾� �� ���� ���? (������Ʈ�� OnClick���� �ٴ� �� �������� ����)

    3. ���� �˸� �޽��� ǥ�� ���
        - �� ������ ���۵˴ϴ�.
        - ���� ���������� �̵��� �ּ���.
        - �������� ���� ����Ʈ�� �������ּ���.
        - ??���� �������� ����߽��ϴ�!

    4. HUD�� ǥ���� ����
        - ��������, ���̺� ����
        - ���� �ð�
        - ���� �� ������, �÷��̾ ���� �� ������
        - �÷��̾� ���� ���
        - ������ ���� ü��
        - �÷��̾��� ��ž ��ġ ����?

    5. ���� ��� ǥ��
        - ������ ����ؼ� �� �ȿ� �ִ� �÷��̾���� ������ �޾ƿ���

    6. �߰��� �����ؾ��� ������
        - killCount(Tower kill, Player kill)
        - ���� ������ ����
        - �� �� �ΰ��� �������..(�����غ���)
    */

    // ��Ÿ ����
    private string nowScene = "InGame";

    // ���� ����
    private string playerName;
    private int gold = 0;
    public int Gold { get { return gold; } }

    private int towerCnt = 1;
    public int TowerCnt { get { return towerCnt; } }
    private int killCnt = 0;

    // ���� ����
    private int stage = 1;
    private int wave = 1;
    private int endWave;
    private int life;
    private int mobCnt = 0;
    private int timer = 100;


    // �޾ƿ;��� ����
    private void Start()
    {
        endWave = GetComponent<WaveSystem>().EndWave;
    }


    // Ÿ�� ��ġ
    public void BuildTower()
    {
        towerCnt -= 1;
        // UI ���� �Լ� ȣ��
    }

    // �� ���°�
    public void Withdraw(int amount)
    {
        gold -= Mathf.Abs(amount);
        // UI ���� �Լ� ȣ��
    }

    // ��� ����
    public void Deposit()
    {
        killCnt += 1;
        mobCnt -= 1;
        // UpdateDisplay();
    }

    // ����� ����
    public void LooseLife(int amount)
    {
        life -= Mathf.Abs(amount);

        if (life <= 0)
        {
            //���ӿ��� �޼ҵ� ȣ��
        }
    }

    // Ÿ�̸�
    public IEnumerator Timer()
    {
        while ( timer > 0)
        {
            timer -= 1;
            // ���÷��� ������Ʈ
            yield return new WaitForSeconds(1);
        }
        timer = 100;
    }

    // Ÿ�̸� ����
    public void StartTimer()
    {
        StartCoroutine("Timer");
    }

    // ���̺� Ŭ����
    private void ClearWave()
    {

    }

    // UI������
    private void UpdateUI()
    {
        // Ÿ�� ��ġ
        // �� ���°�
        // ��� ����
        // ����� ����
        // Ÿ�̸�
    }

    private void GameEnd(bool gameResult)
    {
        // ��� UI ���
    }

    // �׽�Ʈ�� ��ư
    private void ClickBtn()
    {
        // 1 : 
        if (Input.GetKey(KeyCode.KeypadDivide))
        {
            Debug.Log("/");
        }

        // 0, -, = : �� ��ȯ
        if (Input.GetKey(KeyCode.Keypad0) && nowScene != "Login")
        {
            Debug.Log("����ȯ");
            nowScene = "Login";
            SceneManager.LoadScene(6);
        }
        if (Input.GetKey(KeyCode.KeypadMinus) && nowScene != "InGame")
        {
            Debug.Log("����ȯ");
            nowScene = "InGame";
            SceneManager.LoadScene(5);
        }
        if (Input.GetKey(KeyCode.KeypadEquals) && nowScene != "Main")
        {
            Debug.Log("����ȯ");
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
