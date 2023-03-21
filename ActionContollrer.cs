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

    private RaycastHit hitInfo; // 충돌체 정보 저장

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
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>()
            .item.itemName + "획득했습니다.");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }

    private void CheckItem()
    {
        Debug.DrawRay(transform.position, transform.forward * 20, Color.red);

        
        if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out hitInfo,range,
            layerMask))
        {

            if (hitInfo.collider.name != null)
            {
                Debug.Log(hitInfo.collider.tag);
            }
            //if(hitInfo.transform.tag == "Item")
            //{
            //    ItemInfoAppear();
            //}
            if (hitInfo.collider.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
        {
            InfoDisappear();
        }
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>()
            .item.itemName + "획득 " + "<color=yellow>" + "(Z)" + "</color>";
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
