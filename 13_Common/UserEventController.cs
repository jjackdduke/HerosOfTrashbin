using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserEventController : MonoBehaviour
{

    private TextMeshProUGUI actionText;
    private GameObject statusToggleScreen;
    private string eventMessage;
    private int eventNumber;
    private bool tabKeyPressed;
    private bool statusToggleActive;

    // Start is called before the first frame update
    void Start()
    {
        actionText = GameObject.Find("actionText").GetComponent<TextMeshProUGUI>();
        statusToggleScreen = GameObject.Find("StatusToggleScreen");
        actionText.gameObject.SetActive(false);
        statusToggleScreen.SetActive(false);
        tabKeyPressed = false;
        statusToggleActive = false;
    }

    // 영준 : Tab 키를 눌렀다 뗐을 때 스테이터스 창을 열고 닫는 동작 수행
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            tabKeyPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (tabKeyPressed)
            {
                statusToggleActive = !statusToggleActive;
                statusToggleScreen.SetActive(statusToggleActive);
            }
            tabKeyPressed = false;
        }
    }

    public int EventInfoAppear(Collider other)
    {
        switch (other.gameObject.name)
        {
            case "GoddessStatue":
                eventMessage = "여신상을 활성화";
                eventNumber = 0;
                break;
            case "Pharmacy":
                eventMessage = "알약이의 약국 열기";
                eventNumber = 1;
                break;
            case "EnhanceItemStore":
                eventMessage = "스텟 강화 상점 열기";
                eventNumber = 2;
                break;
            case "WeaponItemStore":
                eventMessage = "무기 가게 열기";
                eventNumber = 3;
                break;
            case "EventBank":
                eventMessage = "S전자 주식 구매하기";
                eventNumber = 4;
                break;
            default:
                eventMessage = "이건뭐시여?";
                eventNumber = -1;
                break;
        }
        actionText.gameObject.SetActive(true);
        actionText.text =  $"{eventMessage}" + "<color=yellow>" + "(E)" + "</color>";

        return eventNumber;

    }


    public void EventInfoDisappear()
    {
        actionText.gameObject.SetActive(false);
    }

    public void PlayEvent(int eventNumber)
    {
        switch(eventNumber)
        {
            case 0: // 여신상
                break;
            case 1: // 알약이
                break;
            case 2: // 스텟강화 상점
                break;
            case 3: // 무기가게
                break;
            case 4: // S전자 주식
                break;
            case 5:
                break;
            case 6:
                break;
            case -1: // 알수없는 이벤트 코드
                break;
            default:
                break;

        }
    }

    public void StatusScreenToggle()
    {

    }
}
