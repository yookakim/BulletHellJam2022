using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	[SerializeField] protected Color entityDeadColor;
	[SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected Hitbox hitbox;
	[SerializeField] protected Health health;
	[SerializeField] protected Movement movement;
	[SerializeField] protected WeaponController weaponController;
	[SerializeField] protected EntityTweenEffects entityTweenFX;
	[SerializeField] protected EntityStatusEffects entityStatusEffects;
	[SerializeField] protected Rigidbody2D rb;
	[SerializeField] protected CoinSpawner coinSpawner;
	[SerializeField] protected int coinsDropped;
	[SerializeField] protected float timeAfterDeathBeforeDestroy;
	[SerializeField] protected float entityMass;
	[SerializeField] protected float knockbackForceOnDeath;
	public SoundEffectData hurtSound;
	public SoundEffectData deathSound;
	public float EntityMass { get => entityMass; }
	public float TimeAfterDeathBeforeDestroy { get => timeAfterDeathBeforeDestroy; }
	public float ElapsedTimeAfterDeath { get; set; }
	public Hitbox Hitbox { get => hitbox; }

	protected Vector2 directionOfAttackHitBy;

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

				Rigidbody2D rbHitBy = gameObjectHitBy.GetComponent<Rigidbody2D>();

				if (rbHitBy != null)
				{
					if (rbHitBy.velocity.magnitude <= 0.01f)
					{
						directionOfAttackHitBy = gameObjectHitBy.transform.up;
					}
					else
					{
						directionOfAttackHitBy = rbHitBy.velocity.normalized;
					}
				}
				hurtSound?.Play();
				health.DealDamage(incomingDamageComponent.DamageAmount);
			}
		}
	}

	protected virtual void OnEntityHealthZero(GameObject deadObject)
	{
		// entityTweenFX.CancelAllTweens();
		// weaponController.CancelTweens();

		rb.AddForce(directionOfAttackHitBy * knockbackForceOnDeath, ForceMode2D.Impulse);
		sprite.color = entityDeadColor;
		hitbox.gameObject.SetActive(false);
		deathSound?.Play();
		// GetComponent<Collider2D>().enabled = false;
	}

	public virtual void FinalDestroy()
	{
		entityTweenFX.CancelAllTweens();
		weaponController.CancelTweens();

		
		Destroy(gameObject);
	}
}
