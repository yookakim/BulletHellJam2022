using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public Hitbox.Alignment ProjectileAlignment { get; set; }

	[SerializeField] private ProjectileData projectileData;
	private LayerMask hitboxLayer;

	private void Awake()
	{
		hitboxLayer = LayerMask.GetMask("Hitbox");
		ProjectileAlignment = projectileData.projectileAlignment;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
/*		Debug.Log(LayerMask.LayerToName(collision.gameObject.layer));
		LayerMask otherColliderLayer = LayerMask.GetMask(LayerMask.LayerToName(collision.gameObject.layer));

		if (otherColliderLayer != hitboxLayer)
		{
			return;
		}*/
		// pass own gameobject into hitbox right here
		Hitbox hitbox = collision.gameObject.GetComponent<Hitbox>();
		hitbox.TriggerHitbox(gameObject);

/*		if (hitboxLayer == otherColliderLayer)
		{
			OnDestructibleCollision(collision);
			return;
		}
		if (true)
		{

		}*/
	}

	private void OnDestructibleCollision(Collider2D collsion)
	{
		Destroy(gameObject);
	}
}
