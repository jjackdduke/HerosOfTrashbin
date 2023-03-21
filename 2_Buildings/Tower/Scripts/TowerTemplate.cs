using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu]
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;
    public Weapon[] weapon;

    [System.Serializable]
    public struct Weapon
    {
        public GameObject Prefab;
        public float Damage;
        public float rate;
        public float range;
        public int cost;
        public bool isParticle;
        public bool isDeBuff;
        public bool isZangPan;
        public GameObject missile;
        public float missileUp;
        public float missileWaitSecond;
        public float missileSpeed;
    }
}
