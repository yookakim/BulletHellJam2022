using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Projectile")]
public class ProjectileData : ScriptableObject
{
    public int projectileDamage;
    public float projectileLifetime;
    public DamageComponent.Alignment projectileAlignment;
    public float knockbackForce;
}
