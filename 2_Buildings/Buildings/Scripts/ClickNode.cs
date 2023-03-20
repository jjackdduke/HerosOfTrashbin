using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TowerTemplate;
using Random = System.Random;

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

    // �������� ����
    public Renderer rend;

    // ������ �� ���� ����
    Random randomObj = new Random();
    int randomValue;

    Bank bank;

    // �Ǽ�������
    private float buildDelay = 0.5f;


    void Awake()
    {
        bank = FindObjectOfType<Bank>();
        rend = GetComponent<Renderer>();
        rend.material.color = StartColor;
    }


    private void OnMouseUp()
    {
        if (isBuilt == 0)
        {
            bool isSuccessful;
            
            randomValue = randomObj.Next(7, 8);
            //Debug.Log(randomValue);
            rend.material.color = SelectColor;
            // Ÿ���� ��ġ�Ǿ� ���� ���� ��ġ��� Ÿ�������տ� ����� ��ũ��Ʈ������ Ÿ�� ��ġ �޼ҵ� ����
            if (randomValue > 6)
            {
                isSuccessful = CreateTower(GunPrefab, transform.position); 
                isBuilt = 1;
            }
            else if (randomValue > 4 && randomValue <= 6)
            {
                isSuccessful = CreateTower(TurretPrefab, transform.position);
                isBuilt = 2;
            }
            else if (randomValue > 2 && randomValue <= 4)
            {
                isSuccessful = CreateTower(FirePrefab, transform.position);
                isBuilt = 3;
            }
            else
            {
                isSuccessful = CreateTower(IcePrefab, transform.position);
                isBuilt = 4;
            }

        }
    }

    public bool CreateTower(TowerTemplate towerTemplate, Vector3 position)
    {
        GameObject projectilePrefab = towerTemplate.weapon[0].Prefab;

        if (bank == null)
        {
            return false;
        }
        if (bank.TowerTO > 0)
            {
            GameObject towerClone = Instantiate(projectilePrefab, position, Quaternion.identity);
            //bank.BuildTower();
            //StartCoroutine(Build());
            // cost += inflation;

            towerClone.GetComponent<TowerWeapon>().SetUp(towerTemplate, true);
            

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
}
