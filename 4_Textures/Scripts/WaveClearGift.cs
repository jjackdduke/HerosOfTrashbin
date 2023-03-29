using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveClearGift : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    private HandlePopUps handlePopUps;
    private GM gM;
    // Start is called before the first frame update
    private void Start()
    {
        handlePopUps = FindObjectOfType<HandlePopUps>();
        gM = GameObject.Find("GM").GetComponent<GM>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Close()
    {
        Debug.Log("보상 창을 닫습니다.");
        handlePopUps.ClosePopUp(popUp);
    }
    public void GiftGet()
    {
        Debug.Log("선물을 받았다!");
        Close();
    }
}
