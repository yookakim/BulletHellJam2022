using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantDeathWorm : MonoBehaviour
{
    [SerializeField] private int deathWormDamage;

	private DamageComponent damageComponent;
	private Rigidbody2D rb;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Hitbox hitbox = collision.gameObject.GetComponent<Hitbox>();
		hitbox.TriggerHitbox(gameObject);
	}

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		damageComponent = GetComponent<DamageComponent>();

		damageComponent.DamageAlignment = DamageComponent.Alignment.neutral;
		damageComponent.DamageAmount = deathWormDamage;
		// damageComponent.
	}

	private void FixedUpdate()
	{
		rb.MovePosition(rb.position + new Vector2(0, 0.75f) * Time.fixedDeltaTime);
	}
}
