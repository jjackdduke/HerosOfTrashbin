using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSubmitButton : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    private HandlePopUps handlePopUps;
    private GM gM;
    private string toggleSelected;
    private void Start()
    {
        handlePopUps = FindObjectOfType<HandlePopUps>();
        GetComponent<Button>().onClick.AddListener(Close);
        // Memo : 추후에 GameManager 오브젝트 이름 확정 필요
        gM = GameObject.Find("GM").GetComponent<GM>();
        toggleSelected = "Character 1"; // 초기값 할당 필요
    }
    // CloseButton의 Close 함수를 오버라이드
    private void Close()
    {
        Debug.Log("Submitted " + toggleSelected);
        handlePopUps.ClosePopUp(popUp);
    }
    public void OnToggleClicked(Toggle toggle)
    {
        if (toggle.isOn) {
            toggleSelected = toggle.gameObject.name;
            Debug.Log("Selected " + toggleSelected);
        }
    }
}
