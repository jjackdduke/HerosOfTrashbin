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

    //private RaycastHit hitInfo; // �浹ü ���� ����

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

    }

    

    private void CanPickUp(Collider other)
    {
        if (pickupActivated)
        {
            if (other.transform != null)
            {
                Debug.Log(other.transform.GetComponent<ItemPickUp>()
            .item.itemName + "ȹ���߽��ϴ�.");
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
            .item.itemName + "ȹ�� " + "<color=yellow>" + "(Z)" + "</color>";
    }

    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
