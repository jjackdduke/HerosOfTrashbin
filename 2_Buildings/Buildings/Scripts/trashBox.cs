using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashBoxWithPhoton : MonoBehaviour
{
    void Start()
    {
        GameObject camera = GameObject.FindWithTag("MainCamera");
        transform.rotation = new Quaternion(0, camera.transform.rotation.y - 180, 0, transform.rotation.w);
    }

    
}
