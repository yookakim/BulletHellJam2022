using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
	private GameObject owner;
	private int ownerLayer;
	public bool destroysProjectiles;
	public DamageComponent.Alignment ownerAlignment;

    public event Action<GameObject> hitboxTriggeredEvent;

	private void Awake()
	{
		owner = transform.parent.gameObject;
		ownerLayer = owner.layer;
	}

	public void TriggerHitbox(GameObject gameObjectHitBy)
	{
		if (gameObjectHitBy.layer == LayerMask.NameToLayer("Projectile"))
		{

			Projectile projectile = gameObjectHitBy.GetComponent<Projectile>();

			if (ReferenceEquals(projectile.Owner, owner))
			{
				// what happens when we want entities to get hit by their own projectiles?
				return;
			}

			CheckDestroyProjectile(projectile);
		}
		// defer on-hit behavior to event listeners
		hitboxTriggeredEvent?.Invoke(gameObjectHitBy);
	}

	public void CheckDestroyProjectile(Projectile projectile)
	{

		if (ownerLayer == LayerMask.NameToLayer("Destructible"))
		{

			if (projectile.NumberBounces < 1 && projectile.ProjectileAlignment != ownerAlignment)
			{
				Destroy(projectile.gameObject);

			}
			return;
		}
		else if (projectile.ProjectileAlignment != ownerAlignment)
		{
			Destroy(projectile.gameObject);
		}
	}
}
