using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BombData : ScriptableObject
{
    [Header("Damage Component")]
    public DamageComponent.Alignment damageAlignment;
    public bool hitsAllies;
    public int damageAmount;
    public float knockbackAmount;

    public float bombExplosionRadius;
    public float bombExplosionStrength;
    public float friendlyDamageMultiplier;
    public int bombEntityDamage;
    public float bombTickDownTime;
    public float timeToDestroyAfterExplosion;
}
