using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionContollrer : MonoBehaviour
{
    [SerializeField]
    private float range; //���� ������ �ִ� �Ÿ�.

    private bool pickupActivated = false; // ���� ������ �� true

    private RaycastHit hitInfo; // �浹ü ���� ����

    [SerializeField]
    private LayerMask layerMask; // ������ ���̾�� �����ؾ� �Ѵ�.

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
            .item.itemName + "ȹ���߽��ϴ�.");
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
            .item.itemName + "ȹ�� " + "<color=yellow>" + "(Z)" + "</color>";
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
