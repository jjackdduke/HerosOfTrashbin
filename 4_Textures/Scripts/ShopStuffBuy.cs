using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopStuffBuy : MonoBehaviour
{
    private GM gM;
    // Start is called before the first frame update
    void Start()
    {
        gM = GameObject.Find("GM").GetComponent<GM>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StuffBuy(TextMeshProUGUI textCost)
    {
        int amount = int.Parse(textCost.text);
        Debug.Log(amount + "골드짜리 물품 구매");
        gM.Withdraw(amount, false);
    }
}
