using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	[SerializeField] private BombData bombData;
    private Rigidbody2D rb;
	private SpriteRenderer sprite;
	private LayerMask destructiblesMask;

	private float timeRemaining;
	private bool exploded;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
		destructiblesMask = LayerMask.GetMask("Destructible");
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
		// do the circle cast
		RaycastHit2D[] hitByExplosion = Physics2D.CircleCastAll(
			rb.transform.position,
			bombData.bombExplosionRadius,
			Vector2.zero,
			0f,
			destructiblesMask
		);

		for (int i = 0; i < hitByExplosion.Length; i++)
		{
			TerrainTile tile = hitByExplosion[i].collider.gameObject.GetComponent<TerrainTile>();

			tile.DestroyTile();
		}

		StartCoroutine(DestroyAfterExplosion());
		Debug.Log("exploding");
		// Destroy(gameObject);
	}

	private IEnumerator DestroyAfterExplosion()
	{
		yield return new WaitForSeconds(bombData.timeToDestroyAfterExplosion);

		Destroy(gameObject);
	}
}
