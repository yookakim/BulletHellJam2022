using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Hitbox : MonoBehaviour
{


	public bool destroysProjectiles;
	public DamageComponent.Alignment ownerAlignment;

    public event Action<GameObject> hitboxTriggeredEvent;

    public void TriggerHitbox(GameObject gameObjectHitBy)
	{
		// defer on-hit behavior to event listeners
		hitboxTriggeredEvent?.Invoke(gameObjectHitBy);
		if (gameObjectHitBy.layer == LayerMask.NameToLayer("Projectile"))
		{

			Projectile projectile = gameObjectHitBy.GetComponent<Projectile>();
			CheckDestroyProjectile(projectile);
		}
	}

	public void CheckDestroyProjectile(Projectile projectile)
	{
		if (projectile.ProjectileAlignment != ownerAlignment)
		{
			Destroy(projectile.gameObject);
		}
	}
}
