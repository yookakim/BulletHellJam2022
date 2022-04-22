using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Projectile : MonoBehaviour
{
	public GameObject Owner { get; set; }
	public DamageComponent.Alignment ProjectileAlignment { get => projectileDamageComponent.DamageAlignment; }
	public Vector2 Direction { get; set; }
	public float LaunchForce { get; set; }
	public float InaccuracyForDelayedStart { get; set; }
	public int NumberBounces { get; set; }

	public Transform LiveTarget { get; set; }

	[SerializeField] private ProjectileData projectileData;
	[SerializeField] private DamageComponent projectileDamageComponent;
	
	private bool accelerating;
	private float accelerateForce;
	private float accelerationDelay;

	private bool rotating;
	private float fixedRotateAmount;

	private bool directionSet;

	private bool alreadyCollidedThisFrame;

	private Rigidbody2D rb;
	private LayerMask hitboxLayer;
	private float hitboxRadius;

	private float elapsedLifetime;

	private Vector2 positionLastFrame;

	private Vector2 nextVelocityChange;


	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		hitboxLayer = LayerMask.GetMask("Hitbox");
		projectileDamageComponent.DamageAlignment = projectileData.projectileAlignment;
		projectileDamageComponent.DamageAmount = projectileData.projectileDamage;
		projectileDamageComponent.KnockbackForce = projectileData.knockbackForce;

		NumberBounces = projectileData.numberBounces;

		accelerating = projectileData.accelerating;
		accelerateForce = projectileData.accelerationAmount;
		accelerationDelay = projectileData.accelerationDelay;

		rotating = projectileData.rotating;
		fixedRotateAmount = projectileData.rotatingAmount;

		hitboxRadius = GetComponent<CircleCollider2D>().radius;
	}

	private void Update()
	{
		elapsedLifetime += Time.deltaTime;

		if (elapsedLifetime >= projectileData.projectileLifetime)
		{
			Destroy(gameObject);
		}
	}

	private void FixedUpdate()
	{

		if (accelerating && elapsedLifetime >= accelerationDelay)
		{
			if (projectileData.setTargetAfterDelay && LiveTarget != null && !directionSet)
			{
				Direction = ((Vector2)LiveTarget.position - (Vector2)transform.position).normalized;

				float inaccuracy = Random.Range(-InaccuracyForDelayedStart / 2, InaccuracyForDelayedStart / 2);

				Direction = HelperFunctions.RotateVectorRad(Direction, inaccuracy * Mathf.Deg2Rad);

				rb.AddForce(Direction * LaunchForce, ForceMode2D.Impulse);
				directionSet = true;


			}

			rb.AddForce(Direction * accelerateForce, ForceMode2D.Force);
		}

		if (!projectileData.setTargetAfterDelay && !directionSet)
		{
			rb.AddForce(Direction * LaunchForce, ForceMode2D.Impulse);

			directionSet = true;
		}

		if (rotating)
		{
			Direction = HelperFunctions.RotateVectorRad(Direction, fixedRotateAmount * Time.fixedDeltaTime * Mathf.Deg2Rad);
		}

/*		if (NumberBounces > 0)
		{
			CastRaycastCheckForBounce();
		}*/
		positionLastFrame = rb.position;
		alreadyCollidedThisFrame = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// pass own gameobject into hitbox right here
		Hitbox hitbox = collision.gameObject.GetComponent<Hitbox>();
		hitbox.TriggerHitbox(gameObject);

		// if hitbox is owned by destructible, check for bounce
		// LayerMask destructibleMask = LayerMask.GetMask("Destructible");
		/*		if (collision.transform.parent.gameObject.layer == LayerMask.NameToLayer("Destructible") && NumberBounces > 0)
				{
					CastRaycastCheckForBounce(collision);
				}*/

		if (collision.transform.parent.gameObject.layer == LayerMask.NameToLayer("Destructible") && NumberBounces > 0 && !alreadyCollidedThisFrame)
		{

			LayerMask destructibleMask = LayerMask.GetMask("Destructible");

			RaycastHit2D tileToBounceOffOf = Physics2D.Raycast(
				transform.position,
				collision.transform.position - transform.position,
				(collision.transform.position - transform.position).magnitude,
				destructibleMask
			);

			Vector2 positionToCheck = transform.position;

			Debug.DrawLine(transform.position, transform.position + (collision.transform.position - transform.position));
			if (tileToBounceOffOf.fraction == 0)
			{
				RaycastHit2D[] hitTiles = Physics2D.RaycastAll(
					positionLastFrame,
					(Vector2)transform.position - positionLastFrame,
					((Vector2)transform.position - positionLastFrame).magnitude,
					destructibleMask
				);
				foreach (RaycastHit2D hitTile in hitTiles)
				{
					if (hitTile.fraction != 0)
					{
						tileToBounceOffOf = hitTile;
						break;
					}
				}

				positionToCheck = tileToBounceOffOf.point;

				Debug.DrawLine(positionLastFrame, transform.position, Color.green);
			}

			var box = (BoxCollider2D)tileToBounceOffOf.collider;
			if (box != null)
			{
				var scale = Vector2.Scale(box.transform.localScale, box.size);

				var dir = positionToCheck - ((Vector2)box.transform.position + box.offset);

				dir.x = (Mathf.InverseLerp(-scale.x / 2f, scale.x / 2f, dir.x) - 0.5f) * 2;
				dir.y = (Mathf.InverseLerp(-scale.y / 2f, scale.y / 2f, dir.y) - 0.5f) * 2;

				var normal = new[]
				{
					Vector2.right,
					-Vector2.right,
					Vector2.up,
					-Vector2.up,
				}.OrderByDescending(v => Vector2.Dot(dir, v)).First();

				rb.velocity = Vector2.Reflect(rb.velocity, normal);
			}
			alreadyCollidedThisFrame = true;
		}

	}

	public void SetDirection(Vector2 direction)
	{
		transform.up = direction;
	}

	private void OnDestructibleCollision(Collider2D collsion)
	{
		Destroy(gameObject);
	}

	private void CastRaycastCheckForBounce(Collider2D collision)
	{
		// Debug.Log("previous position: " + positionLastFrame);
		// Debug.Log("current: " + rb.position);

		ContactPoint2D[] contactPoints = new ContactPoint2D[1];
		collision.GetContacts(contactPoints);
		Debug.Log(contactPoints[0]);

		Vector2 closestPoint = collision.ClosestPoint(rb.position);


		Vector2 raycastVector = rb.velocity.normalized;
		Vector2 raycastStartPosition = closestPoint - raycastVector;

		LayerMask destructibleMask = LayerMask.GetMask("Destructible");
		RaycastHit2D[] tileToBounceOffOf = Physics2D.RaycastAll(
			raycastStartPosition,
			rb.velocity,
			rb.velocity.magnitude + hitboxRadius,
			destructibleMask
		);

		Debug.Log(tileToBounceOffOf[0].normal);

		Debug.DrawLine(raycastStartPosition, raycastStartPosition + raycastVector);

		foreach (RaycastHit2D raycastHit in tileToBounceOffOf)
		{
			if (raycastHit.fraction == 0)
			{

			}
			else
			{
				if (raycastHit.collider != null)
				{
					Vector2 newDirection = (Vector2.Reflect(rb.velocity, raycastHit.normal));

					rb.velocity = newDirection;
					NumberBounces--;
					break;
				}
			}
		}




		/*		Vector2 vectorFromPreviousFrame = (Vector2)transform.position - positionLastFrame;

				RaycastHit2D tileToBounceOffOf = Physics2D.Raycast(
					positionLastFrame,
					vectorFromPreviousFrame,
					vectorFromPreviousFrame.magnitude + hitboxRadius,
					destructibleMask
				);

				Debug.DrawRay(positionLastFrame, vectorFromPreviousFrame, Color.green);

				if (tileToBounceOffOf.collider != null)
				{
					Debug.Log("raycast detected tile, bouncing");

					// Debug.Log(currentVelocityMagnitude);

					Vector2 newDirection = (Vector2.Reflect(rb.velocity, tileToBounceOffOf.normal));

					// Debug.Log(newDirection);
					Direction = newDirection;

					rb.velocity = newDirection;
					// rb.velocity = Vector2.zero;

					// rb.AddForce(newDirection * currentVelocityMagnitude, ForceMode2D.Impulse);

					NumberBounces--;
				}*/


	}
}
