using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageComponent : MonoBehaviour
{
	public enum Alignment
	{
		friendly,
		enemy,
		neutral
	}

	public Alignment DamageAlignment { get => damageAlignment; set => damageAlignment = value; }
	public bool HitsAllies { get => hitsAllies; set => hitsAllies = value; }
	public int DamageAmount { get => damageAmount; set => damageAmount = value; }
	public float KnockbackForce { get => knockbackForce; set => knockbackForce = value; }

	private Alignment damageAlignment;
	private bool hitsAllies;
	private int damageAmount;
	private float knockbackForce;

	public void InitializeDamageComponent(Alignment damageAlignment, bool hitsAllies, int damageAmount, float knockbackForce)
	{
		this.damageAlignment = damageAlignment;
		this.hitsAllies = hitsAllies;
		this.damageAmount = damageAmount;
		this.knockbackForce = knockbackForce;
	}
}
