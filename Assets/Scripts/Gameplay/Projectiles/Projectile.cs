using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public GameObject Owner { get; set; }
	public DamageComponent.Alignment ProjectileAlignment { get => projectileDamageComponent.DamageAlignment; }

	[SerializeField] private ProjectileData projectileData;
	[SerializeField] private DamageComponent projectileDamageComponent;
	private LayerMask hitboxLayer;

	private float elapsedLifetime;

	private void Awake()
	{
		hitboxLayer = LayerMask.GetMask("Hitbox");
		projectileDamageComponent.DamageAlignment = projectileData.projectileAlignment;
		projectileDamageComponent.DamageAmount = projectileData.projectileDamage;
		projectileDamageComponent.KnockbackForce = projectileData.knockbackForce;
	}

	private void Update()
	{
		elapsedLifetime += Time.deltaTime;

		if (elapsedLifetime >= projectileData.projectileLifetime)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// pass own gameobject into hitbox right here
		Hitbox hitbox = collision.gameObject.GetComponent<Hitbox>();
		hitbox.TriggerHitbox(gameObject);
	}

	private void OnDestructibleCollision(Collider2D collsion)
	{
		Destroy(gameObject);
	}
}
