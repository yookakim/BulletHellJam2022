using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
	[SerializeField] private Collider2D hitbox;
	[SerializeField] private SpriteRenderer coinSprite;
	[SerializeField] private float maximumBounceDistance;
	[Tooltip("Time it takes to bounce to ground and activate hitbox after first spawning")]
	[SerializeField] private float initialTravelTime;
	[Tooltip("downwards for the 3D bouncing effect")]
	[SerializeField] private float gravity;

	float instantiatedTime;

	private float initialYOffset;

	private Vector2 destination;
	private Vector2 normalizedDirection;
	private float travelSpeed;
	private float velocityY;
	private float spriteYOffset;
	bool destinationReached;

	private void Awake()
	{
		hitbox.enabled = false;
		instantiatedTime = Time.time;

		CalculateBouncePosition();
	}

	private void Start()
	{
		initialYOffset = coinSprite.transform.localPosition.y;
		velocityY = initialTravelTime * (gravity / 2);
	}

	private void Update()
	{
		if (!destinationReached)
		{
			velocityY -= gravity * Time.deltaTime;
			spriteYOffset += velocityY * Time.deltaTime;

			coinSprite.transform.position = new Vector3(
				gameObject.transform.position.x,
				gameObject.transform.position.y + spriteYOffset + initialYOffset,
				0
			);
		}

		if (Time.time - instantiatedTime >= initialTravelTime && !destinationReached)
		{
			destinationReached = true;
			hitbox.enabled = true;
		}
	}

	private void FixedUpdate()
	{
		if (!destinationReached)
		{
			rb.MovePosition((Vector2)transform.position + normalizedDirection * travelSpeed * Time.fixedDeltaTime);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Hitbox hitbox = collision.gameObject.GetComponent<Hitbox>();

		Debug.Log("collision detected in coin");

		if (hitbox != null)
		{

			hitbox.TriggerHitbox(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Hitbox hitbox = collision.gameObject.GetComponent<Hitbox>();

		Debug.Log("trigger detected in coin");

		if (hitbox != null)
		{
			hitbox.TriggerHitbox(gameObject);
		}
	}

	public void OnPickup()
	{
		// destroy, release particles, etc.
		Destroy(gameObject);
	}

	private void CalculateBouncePosition()
	{
		float randomDistance = Random.Range(0, maximumBounceDistance);
		destination = TilemapManager.GetRandomNearbyLocation(transform.position, randomDistance, 40);
		normalizedDirection = (destination - (Vector2)transform.position).normalized;
		travelSpeed = (destination - (Vector2)transform.position).magnitude / initialTravelTime;
		Debug.Log(travelSpeed);
	}
}
