using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(menuName = "GAME FILES/ITEM", fileName = "Item.asset")]
[System.Serializable]

public class WeaponOptions : ScriptableObject {
    [SerializeField]
    [Header("Скорость стрельбы")]
    public float fireRate;
    [SerializeField]
    [Header("Урон")]
    public int damage;
    [SerializeField]
    [Header("Трейл")]
    public GameObject trail;
    [SerializeField]
    [Header("Префаб")]
    public GameObject weapon;
    [Header("Вне обоймы")]
    [SerializeField]
    public int outAmmo;
    [SerializeField]
    [Header("В абойме")]
    public int ammo;

}
