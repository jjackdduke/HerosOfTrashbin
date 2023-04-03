using Photon.Pun.Demo.Procedural;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class Invest : MonoBehaviour
{
    GM gm;
    Random randomObj = new Random();
    int randomValue;
    [SerializeField] int ratio;

    [SerializeField ]int stockPrice;

    [SerializeField] ParticleSystem startInvest;
    [SerializeField] ParticleSystem doneInvest;

    [SerializeField]
    private TextMeshProUGUI investText, stockPriceText;
    private GameObject uiManager;
    private Invest invest;

    BGM bgm;
    AudioSource audioSource;
    bool isNear = false;

    // Start is called before the first frame update

    private void Awake()
    {
        invest = GameObject.Find("Event_4_Stock").GetComponent<Invest>();
        uiManager = GameObject.Find("UI").gameObject;

        stockPriceText = uiManager.transform.Find("MidCenter_EventUI/UI_Stock/UI_Shop/Item_No8_Stock/Text_Cost").GetComponent<TextMeshProUGUI>();
        investText = uiManager.transform.Find("MidCenter_EventUI/UI_Stock/UI_Shop/InvestGoldInfo/Text_InvestGoldInfo").GetComponent<TextMeshProUGUI>();
        gm = FindObjectOfType<GM>();
        
        bgm = FindObjectOfType<BGM>();
        audioSource = GetComponent<AudioSource>();

    }
    void Start()
    {
        stockPrice = 100; // 초기 주식가격 : 100원
        stockPriceText.text = stockPrice.ToString();
        investText.text = 0.ToString();
    }

    // Update is called once per frame

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        isNear = true;
    //        InfoAppear(gm.Gold);
    //        if (gm.Gold >= currentStock)
    //        {
    //            if (Input.GetKey(KeyCode.E))
    //            {
    //                DoInvest();                    
    //            }

    //        }
    //        if (Bills > 0)
    //        {
    //            if (Input.GetKey(KeyCode.R))
    //            {
    //                GetResult();
    //            }
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    isNear = false;
    //    InfoDisappear();
    //}

    //private void InfoAppear(int money)
    //{
    //    actionText.gameObject.SetActive(true);
    //    if (Bills > 0)
    //    {
    //        actionText.text = ratio + "% sell now" + (int)(Bills * currentStock) + "Gold" + "<color=yellow>" + "(R)" + "</color>";
    //        if (money > currentStock)
    //        {
    //            actionText.text = ratio + "% sell now" + (int)(Bills * currentStock) + "Gold" + "<color=yellow>" + "(R)" + "</color>" + 
    //                "more money, more happy" + "Invest " + currentStock + "Gold more" + "<color=yellow>" + "(E)" + "</color>";
    //        }
    //    }
    //    else
    //    {
    //        if (Bills == 0)
    //        {
    //            actionText.text = "SSJJ stock is at a bargain right now!" + "Invest " + currentStock + "Gold" + ratio + "%" + "<color=yellow>" + "(E)" + "</color>";
    //        }
    //        else if (money > currentStock)
    //        {
    //            actionText.text = "more money, more happy" + "Invest " + currentStock + "Gold more" + "<color=yellow>" + "(E)" + "</color>";
    //        }
    //        else
    //        {
    //            actionText.text = "you don up sir.";
    //        }
    //    }

    //}

    //private void InfoDisappear()
    //{
    //    actionText.gameObject.SetActive(false);
    //}

    public void DoInvest()
    {
        // gm.Withdraw(currentStock, false);
        gm.UpdateStocks(1);
        investText.text = (gm.StockProperty * stockPrice).ToString();
        // Instantiate(startInvest, transform.position, Quaternion.identity);
    }

    public void ThisWave (int Wave) 
    {
        Debug.Log("wave" + Wave);
        Result();
    }

    public void Result()
    {
        randomValue = randomObj.Next(0, 46);
        ratio = randomValue - 15;

        // Ratio 가 0보다 작으면 하락, 0보다 작으면 상승이라고 띄워주기
        if(ratio > 0)
        {
            Debug.Log($"주식떡상 !!! {ratio}%");
        }
        else
        {
            Debug.Log($"주식하락 ㅠㅠ {ratio}%");
        }

        stockPrice = (int) Mathf.Round(stockPrice * (1 + ((float)ratio / 100))); // 반올림
        stockPriceText.text = stockPrice.ToString();
        investText.text = (gm.StockProperty * stockPrice).ToString();
    }

    public void GetResult()
    {
        gm.Withdraw(gm.StockProperty * stockPrice, true);
        gm.StockProperty = 0;
        investText.text = (gm.StockProperty * stockPrice).ToString();
        Instantiate(doneInvest, transform.position, Quaternion.identity);
    }
}
