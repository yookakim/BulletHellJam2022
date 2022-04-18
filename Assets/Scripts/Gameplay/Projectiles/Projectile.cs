using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private ProjectileData projectileData;
	private LayerMask destructibleLayer;

	private void Awake()
	{
		destructibleLayer = LayerMask.GetMask("Destructible");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(LayerMask.LayerToName(collision.gameObject.layer));
		LayerMask otherColliderLayer = LayerMask.GetMask(LayerMask.LayerToName(collision.gameObject.layer));

		if (destructibleLayer == otherColliderLayer)
		{
			OnDestructibleCollision(collision);
			return;
		}
		if (true)
		{

		}
	}

	private void OnDestructibleCollision(Collider2D collsion)
	{
		Destroy(gameObject);
	}
}
