using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public enum Type {Buff,Weapon};
    public Type type;
    public int value;


    private void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);   
    }


}
