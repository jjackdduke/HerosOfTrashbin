using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TowerTemplate;
using Random = System.Random;
using TMPro;

public class ClickNode : MonoBehaviour
{
    // ��Ȱ��ȭ ����
    public Color StartColor;

    // Ȱ��ȭ ����
    public Color SelectColor;

    // �ش� ��ġ�� ��ġ�Ǿ��ִ��� ����
    [SerializeField] int isBuilt = 0;

    // ��ġ�� Ÿ�������� ���ø�
    [SerializeField] TowerTemplate TurretPrefab;
    [SerializeField] TowerTemplate FirePrefab;
    [SerializeField] TowerTemplate IcePrefab;
    [SerializeField] TowerTemplate GunPrefab;
    [SerializeField] TowerTemplate SlowPrefab;


    [SerializeField]
    private TextMeshProUGUI actionText;

    // �������� ����
    public Renderer rend;

    // ������ �� ���� ����
    Random randomObj = new Random();
    int randomValue;

    GM gm;

    // �Ǽ�������
    private float buildDelay = 0.5f;
    GameObject arrow;

    bool buildActivated;


    void Awake()
    {
        gm = FindObjectOfType<GM>();
        rend = GetComponent<Renderer>();
        rend.material.color = StartColor;
        arrow = transform.GetChild(2).gameObject;
    }


    private void OnMouseUp()
    {
        if (isBuilt == 0)
        {
            bool isSuccessful;
            
            randomValue = randomObj.Next(2, 6);
            //Debug.Log(randomValue);
            rend.material.color = SelectColor;
            // Ÿ���� ��ġ�Ǿ� ���� ���� ��ġ��� Ÿ�������տ� ����� ��ũ��Ʈ������ Ÿ�� ��ġ �޼ҵ� ����
            if (randomValue >= 5)
            {
                isSuccessful = CreateTower2(SlowPrefab, transform.position);
                isBuilt = 5;
            }
            else if (randomValue == 4)
            {
                isSuccessful = CreateTower2(GunPrefab, transform.position); 
                isBuilt = 4;
            }
            else if (randomValue == 3)
            {
                isSuccessful = CreateTower2(TurretPrefab, transform.position);
                isBuilt = 3;
            }
            else if (randomValue == 2)
            {
                isSuccessful = CreateTower2(FirePrefab, transform.position);
                isBuilt = 2;
            }
            else
            {
                isSuccessful = CreateTower2(IcePrefab, transform.position);
                isBuilt = 1;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            BuildInfoAppear();

            if (Input.GetKey(KeyCode.E))
            {
                BuildTower();
            }

            Debug.Log("타워노드 접근");
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        BuildInfoDisappear();
    }

    public bool CreateTower(TowerTemplate towerTemplate, Vector3 position)
    {
        GameObject projectilePrefab = towerTemplate.weapon[0].Prefab;

        if (gm == null)
        {
            return false;
        }
        if (gm.TowerTO > 0)
            {
            GameObject towerClone = Instantiate(projectilePrefab, position, Quaternion.identity);
            //bank.BuildTower();
            //StartCoroutine(Build());
            // cost += inflation;

            towerClone.GetComponent<TowerWeapon>().SetUp(towerTemplate, actionText);
            

            gameObject.SetActive(false);
            return true;
        }
        
        return false;
    }

    public bool CreateTower2(TowerTemplate towerTemplate, Vector3 position)
    {
        GameObject projectilePrefab = towerTemplate.weapon[0].Prefab;

        if (gm == null)
        {
            return false;
        }
        if (gm.TowerTO > 0)
        {
            GameObject towerClone = Instantiate(projectilePrefab, position, Quaternion.identity);
            //bank.BuildTower();
            //StartCoroutine(Build());
            // cost += inflation;

            towerClone.GetComponent<TowerWeapon>().SetUp2(towerTemplate);


            gameObject.SetActive(false);
            return true;
        }

        return false;
    }

    IEnumerator Build()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);

            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(false);
            }
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildDelay);
            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(true);
            }
        }

    }


    private void BuildTower()
    {
        if (isBuilt == 0 && buildActivated)
        {
            bool isSuccessful;

            randomValue = randomObj.Next(2, 6);
            //Debug.Log(randomValue);
            rend.material.color = SelectColor;
            // Ÿ���� ��ġ�Ǿ� ���� ���� ��ġ��� Ÿ�������տ� ����� ��ũ��Ʈ������ Ÿ�� ��ġ �޼ҵ� ����
            if (randomValue >= 5)
            {
                isSuccessful = CreateTower(SlowPrefab, transform.position);
                isBuilt = 5;
            }
            else if (randomValue == 4)
            {
                isSuccessful = CreateTower(GunPrefab, transform.position);
                isBuilt = 4;
            }
            else if (randomValue == 3)
            {
                isSuccessful = CreateTower(TurretPrefab, transform.position);
                isBuilt = 3;
            }
            else if (randomValue == 2)
            {
                isSuccessful = CreateTower(FirePrefab, transform.position);
                isBuilt = 2;
            }
            else
            {
                isSuccessful = CreateTower(IcePrefab, transform.position);
                isBuilt = 1;
            }

            BuildInfoDisappear();
        }
    }


    private void BuildInfoAppear()
    {
        buildActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text =  "타워 건설 " + "<color=yellow>" + "(E)" + "</color>";
    }

    private void BuildInfoDisappear()
    {
        buildActivated = false;
        actionText.gameObject.SetActive(false);
    }

    public void Arrow(bool show)
    {
        arrow.SetActive(show);
    }

}
