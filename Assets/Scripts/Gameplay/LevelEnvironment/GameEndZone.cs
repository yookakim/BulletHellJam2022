using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndZone : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Hitbox hitbox = collision.gameObject.GetComponent<Hitbox>();
		hitbox.TriggerHitbox(gameObject);
	}
}
