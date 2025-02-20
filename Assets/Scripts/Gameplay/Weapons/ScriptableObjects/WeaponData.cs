using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
    public GameObject projectilePrefab;
    public float useRate;
    public bool allowsHoldFire;

    public abstract void UseWeapon(WeaponController weaponController);
}
