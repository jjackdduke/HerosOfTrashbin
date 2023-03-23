using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class CameraFollow : MonoBehaviourPunCallbacks
{
    public Transform target;
    public Vector3 offset;
  

    // offset만큼 떨어져서 카메라 추적
    void Update()
    {

        transform.position = target.position + offset;
    }
}
