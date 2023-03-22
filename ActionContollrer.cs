using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionContollrer : MonoBehaviour
{
    [SerializeField]
    private float range; //습득 가능한 최대 거리.

    private bool pickupActivated = false; // 습득 가능할 시 true

    //private RaycastHit hitInfo; // 충돌체 정보 저장

    [SerializeField]
    private LayerMask layerMask; // 아이템 레이어에만 반응해야 한다.

    [SerializeField]
    private TextMeshProUGUI actionText;

    [SerializeField]
    private Inventory theInventory;

    void Start()
    { 
        
    }

    void Update()
    {

    }

    

    private void CanPickUp(Collider other)
    {
        if (pickupActivated)
        {
            if (other.transform != null)
            {
                Debug.Log(other.transform.GetComponent<ItemPickUp>()
            .item.itemName + "획득했습니다.");
                theInventory.AcquireItem(other.transform.GetComponent<ItemPickUp>().item);
                Destroy(other.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Item")
        {   
            ItemInfoAppear(other);

            if (Input.GetKey(KeyCode.Z))
            {
                CanPickUp(other);
            }

        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            InfoDisappear();
        }
    }

    private void ItemInfoAppear(Collider other)
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = other.transform.GetComponent<ItemPickUp>()
            .item.itemName + "획득 " + "<color=yellow>" + "(Z)" + "</color>";
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
