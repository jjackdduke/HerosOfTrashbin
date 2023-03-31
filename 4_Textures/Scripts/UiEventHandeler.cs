using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiEventHandeler : MonoBehaviour
{
    [SerializeField] private GM gameManager;
    [SerializeField] private Invest invest;
    private Statue jangSeung;
    [SerializeField] private ItemController itemController;

    [SerializeField] private UserEventController userEventController;
    private int currentGold;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GM").GetComponent<GM>();
        jangSeung = GameObject.Find("Event_0_JangSeung").GetComponent<Statue>();

        itemController = GameObject.Find("ItemManager").GetComponent<ItemController>();
        userEventController = GameObject.Find("EventManager").GetComponent<UserEventController>();
    }

    // Update is called once per frame
    void Update()
    {
        invest = GameObject.Find("Event_4_Stock").GetComponent<Invest>();
    }
    //public void BuyItem(TextMeshProUGUI textCost)
    //{
    //    ItemEnums.EnhanceItemEnums enhanceItem;
    //    // Debug.Log(textCost);
    //    int amount = int.Parse(textCost.text);
    //    currentGold = gameManager.Gold;
    //    Debug.Log(amount + "   " + currentGold);
    //    if(currentGold < amount)
    //    {
    //        Debug.Log("돈이 없어용");
    //    }
    //    else
    //    {
    //        // 강화하기
    //        if (textCost.text.Contains("Enhance")) // 플레이어 스텟 강화
    //        {
    //            gameManager.Withdraw(amount, false);
    //            switch (textCost.text)
    //            {
    //                case "Item_Enhance_1": enhanceItem = ItemEnums.EnhanceItemEnums.PlayerAttackUp; break;
    //                case "Item_Enhance_2": enhanceItem = ItemEnums.EnhanceItemEnums.PlayerRangeUp; break;
    //                case "Item_Enhance_3": enhanceItem = ItemEnums.EnhanceItemEnums.PlayerMoveSpeedUp; break;
    //                case "Item_Enhance_4": enhanceItem = ItemEnums.EnhanceItemEnums.TowerAttackUp; break;
    //                case "Item_Enhance_5": enhanceItem = ItemEnums.EnhanceItemEnums.TowerAttackSpeedUp; break;
    //                case "Item_Enhance_6": enhanceItem = ItemEnums.EnhanceItemEnums.TowerRangeUp; break;
    //                default: enhanceItem = ItemEnums.EnhanceItemEnums.PlayerAttackUp; break;
    //            }
    //            itemController.enhanceStackByItem(enhanceItem, Random.Range(0, 3));
    //        }
    //        else
    //        {
    //            switch (textCost.text)
    //            {
    //                case "Item_No_8":
    //                    {
    //                        invest.DoInvest();
    //                        userEventController.updateCurrentGold(); break;
    //                    }

    //            }
    //        }
            
    //    }
        
    //}

    public void PlayEvent(GameObject gameObject)
    {
        Debug.Log(gameObject.name);
        if (gameObject.name.Contains("Item_EH"))
        {
            int enhanceCost = int.Parse(gameObject.transform.Find("Text_Cost").GetComponent<TextMeshProUGUI>().text);
            if(gameManager.Gold < enhanceCost) {
                Debug.Log("돈부족!!");
            }
            else
            {
                switch (gameObject.name)
                {
                    case "Item_EHPlayerAttack": itemController.enhanceStackByItem(ItemEnums.EnhanceItemEnums.PlayerAttackUp, Random.Range(0, 2)); break;
                    case "Item_EHPlayerMoveSpeed": itemController.enhanceStackByItem(ItemEnums.EnhanceItemEnums.PlayerMoveSpeedUp, Random.Range(0, 2)); break;
                    case "Item_EHPlayerRange": itemController.enhanceStackByItem(ItemEnums.EnhanceItemEnums.PlayerRangeUp, Random.Range(0, 2)); break;
                    case "Item_EHTowerAttack": itemController.enhanceStackByItem(ItemEnums.EnhanceItemEnums.TowerAttackUp, Random.Range(0, 2)); break;
                    case "Item_EHTowerAttackSpeed": itemController.enhanceStackByItem(ItemEnums.EnhanceItemEnums.TowerAttackSpeedUp, Random.Range(0, 2)); break;
                    case "Item_EHTowerAttackRange": itemController.enhanceStackByItem(ItemEnums.EnhanceItemEnums.TowerRangeUp, Random.Range(0, 2)); break;
                }
            }
            
            
        }
        else
        {
            switch (gameObject.name)
            {
                case "Button_JangSeung":
                    Debug.Log("장승 이벤트!");
                    jangSeung.Pray();
                    jangSeung.punishable = false;
                    jangSeung.StatueParticle.SetActive(jangSeung.punishable);

                    break;


                default:; break;
            }
        }
        
    }

}
