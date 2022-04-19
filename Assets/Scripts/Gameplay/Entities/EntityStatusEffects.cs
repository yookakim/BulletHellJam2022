using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatusEffects : MonoBehaviour
{
    [SerializeField] private Entity entity;
    [SerializeField] private Health entityHealth;
    [SerializeField] private Movement entityMovement;
    [SerializeField] private bool immuneToKnockback;

    private bool inKnockback;
    private bool inHitstun;

    public void ApplyKnockback(DamageComponent knockbackToApply)
	{
		if (immuneToKnockback)
		{
            return;
		}

        float knockbackDistance = knockbackToApply.KnockbackForce / entity.EntityMass;


	}
}
