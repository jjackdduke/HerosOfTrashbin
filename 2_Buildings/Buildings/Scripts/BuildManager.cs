using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� �� ��

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public GameObject SelectNode;
    public GameObject Tower;

    public void Start()
    {
        instance = this;
    }

    // Ÿ�� �ν��Ͻ� ����
    public void BuildToTower()
    {   
        // ������ ������Ʈ, ������ ��ġ(������), ������ ����
        Instantiate(Tower, SelectNode.transform.position, Quaternion.identity);
    }

}
