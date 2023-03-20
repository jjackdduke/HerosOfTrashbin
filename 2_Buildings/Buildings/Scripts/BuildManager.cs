using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 현재 안 씀

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    public GameObject SelectNode;
    public GameObject Tower;

    public void Start()
    {
        instance = this;
    }

    // 타워 인스턴스 생성
    public void BuildToTower()
    {   
        // 생성할 오브젝트, 생성될 위치(포지션), 생성될 각도
        Instantiate(Tower, SelectNode.transform.position, Quaternion.identity);
    }

}
