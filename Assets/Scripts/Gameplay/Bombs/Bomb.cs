using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	[SerializeField] private BombData bombData;
    private Rigidbody2D rb;
	private SpriteRenderer sprite;
	private LayerMask hitboxMask;

	private float timeRemaining;
	private bool exploded;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
		hitboxMask = LayerMask.GetMask("Hitbox");
		timeRemaining = bombData.bombTickDownTime;
	}

	private void Update()
	{
		timeRemaining -= Time.deltaTime;

		if (timeRemaining <= 0 && !exploded)
		{
			exploded = true;
			Explode();
		}
	}

	private void Explode()
	{
		// refactor into a more generic system
		// instead of only hitting destructibles, hit everything that has a hitbox
		// defer specific behavior on-hit to the thing that was hit (hitbox component becomes middleman)

		// do the circle cast
		RaycastHit2D[] hitByExplosion = Physics2D.CircleCastAll(
			rb.transform.position,
			bombData.bombExplosionRadius,
			Vector2.zero,
			0f,
			hitboxMask
		);

		for (int i = 0; i < hitByExplosion.Length; i++)
		{
			/*			TerrainTile tile = hitByExplosion[i].collider.gameObject.GetComponent<TerrainTile>();

						tile.DestroyTile();*/

			// all the hitboxes hit by circlecast
			Hitbox hitbox = hitByExplosion[i].collider.GetComponent<Hitbox>();
			hitbox.TriggerHitbox(gameObject);
		}

		StartCoroutine(DestroyAfterExplosion());
		// Destroy(gameObject);
	}

	private IEnumerator DestroyAfterExplosion()
	{
		yield return new WaitForSeconds(bombData.timeToDestroyAfterExplosion);

		Destroy(gameObject);
	}
}
