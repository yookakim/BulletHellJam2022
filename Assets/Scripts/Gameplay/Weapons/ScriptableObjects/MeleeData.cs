using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeData : ScriptableObject
{
    public GameObject meleeAttackPrefab;
    public float useRate;
    public int meleeDamage;
    public DamageComponent.Alignment alignment;
    public float knockbackForce;

    public abstract void UseMelee(MeleeController meleeController);
}
