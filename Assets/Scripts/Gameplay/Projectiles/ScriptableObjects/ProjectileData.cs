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

    public int numberBounces;

    public bool accelerating;
    public float accelerationAmount;
    public float accelerationDelay;

    public bool setTargetAfterDelay;

    public bool rotating;
    public float rotatingAmount;
}
