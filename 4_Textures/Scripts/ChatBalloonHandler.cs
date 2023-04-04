using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBalloonHandler : MonoBehaviour
{
    public Transform PlayerTransform;
    // Use this for initialization 
    void Start()
    {
        // cameraToLookAt = Camera.main;

    }

    // Update is called once per frame 
    void LateUpdate()
    {
        transform.position = PlayerTransform.position + new Vector3(0, PlayerTransform.GetChild(0).localScale.y * 2 / 3, 0);
        transform.rotation = Quaternion.Euler(90f, 0, 0);
    }
}
