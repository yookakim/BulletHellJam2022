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
	[SerializeField] protected EntityTweenEffects entityTweenFX;
	[SerializeField] protected EntityStatusEffects entityStatusEffects;
	[SerializeField] protected float entityMass;

	public float EntityMass { get => entityMass; }


	private void OnEnable()
	{
		health.HealthZeroEvent += OnEntityHealthZero;
		hitbox.hitboxTriggeredEvent += OnHit;
	}

	private void OnDisable()
	{
		health.HealthZeroEvent -= OnEntityHealthZero;
		hitbox.hitboxTriggeredEvent -= OnHit;
	}

	protected virtual void OnHit(GameObject gameObjectHitBy)
	{
		// EntityStatusEffects
		DamageComponent incomingDamageComponent = gameObjectHitBy.GetComponent<DamageComponent>();

		if (incomingDamageComponent != null)
		{
			if (incomingDamageComponent.DamageAlignment != hitbox.ownerAlignment || incomingDamageComponent.HitsAllies)
			{
				// start flash tween
				entityTweenFX.OnDamageTween();
				entityStatusEffects.ApplyKnockback(incomingDamageComponent);
				health.DealDamage(incomingDamageComponent.DamageAmount);
			}
		}
	}

	protected virtual void OnEntityHealthZero(GameObject deadObject)
	{
		entityTweenFX.CancelAllTweens();
		weaponController.CancelTweens();
	}
}
