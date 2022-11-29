using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public string name;
    public float fireRate;
    public float aimSpeed;
    public float bloom;
    public float recoil;
    public float kickBack;

    public GameObject prefab;
}
