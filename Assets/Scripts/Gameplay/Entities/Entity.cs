using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	[SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected Hitbox hitbox;
	[SerializeField] protected Health health;
	[SerializeField] protected Movement movement;
	[SerializeField] protected WeaponController weaponController;
	[SerializeField] protected EntityStatusEffects entityStatusEffects;
	[SerializeField] protected float entityMass;
	// [SerializeField] protected DamageComponent.Alignment entityAlignment;

	public float EntityMass { get => entityMass; }
	// public DamageComponent.Alignment EntityAlignment { get => entityAlignment; }

	// private EntityStatusEffects entityStatusEffects;

	private void Awake()
	{
		health.HealthZeroEvent += OnEntityHealthZero;
		hitbox.hitboxTriggeredEvent += OnHit;
	}

	protected virtual void OnHit(GameObject gameObjectHitBy)
	{
		// EntityStatusEffects
		DamageComponent incomingDamageComponent = gameObjectHitBy.GetComponent<DamageComponent>();

		if (incomingDamageComponent != null)
		{
			if (incomingDamageComponent.DamageAlignment != hitbox.ownerAlignment || incomingDamageComponent.HitsAllies)
			{
				// entityStatusEffects.ApplyHitstun();
				Debug.Log("receivign incoming damage");
				entityStatusEffects.ApplyKnockback(incomingDamageComponent);
				health.DealDamage(incomingDamageComponent.DamageAmount);
			}
		}
	}

	protected virtual void OnEntityHealthZero(GameObject deadObject)
	{

	}
}
