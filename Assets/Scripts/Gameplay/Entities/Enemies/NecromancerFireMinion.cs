using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerFireMinion : Entity
{
	public Necromancer OwnerNecromancer { get; set; }
	public Vector2 Direction { get; set; }

	[SerializeField] private float speedDecelerationLerpAmount;
	[SerializeField] private float speedDecelerationLerpRandomRange;
	[SerializeField] private float initialSpeedMultiplier;
	[SerializeField] private float initialMultiplierRandomRange;
	[SerializeField] private float initialDelayBeforeAttacking;
	[SerializeField] private float minimumSpeedMultiplier;
	[SerializeField] private float randomMoveInterval;
	[SerializeField] private float randomMoveDistance;
	[SerializeField] private float spinRate;

	// private Rigidbody2D rb;

	private float currentMoveSpeedMultiplier;
	private float elapsedLifetime;
	private float currentRandomMoveInterval;
	private Vector2 spinVector;

	private void Awake()
	{
		// rb = GetComponent<Rigidbody2D>();
		currentMoveSpeedMultiplier = initialSpeedMultiplier + Random.Range(-initialMultiplierRandomRange / 2, initialMultiplierRandomRange / 2);
		speedDecelerationLerpAmount = speedDecelerationLerpAmount + Random.Range(-speedDecelerationLerpRandomRange / 2, speedDecelerationLerpRandomRange / 2);
		spinVector = Vector2.up;
		weaponController.CurrentWeaponTarget = (Vector2)transform.position + spinVector;

	}

	private void Update()
	{
		elapsedLifetime += Time.deltaTime;

		spinVector = HelperFunctions.RotateVectorRad(spinVector, spinRate * Mathf.Deg2Rad * Time.deltaTime);
		weaponController.CurrentWeaponTarget = (Vector2)transform.position + spinVector;
		if (elapsedLifetime >= initialDelayBeforeAttacking)
		{
			weaponController.AttemptUse();
		}
	}

	private void FixedUpdate()
	{
		

		if (currentMoveSpeedMultiplier >= minimumSpeedMultiplier)
		{
			currentMoveSpeedMultiplier = Mathf.Lerp(currentMoveSpeedMultiplier, 0, speedDecelerationLerpAmount);
		}
		else
		{
			currentRandomMoveInterval += Time.fixedDeltaTime;
			if (currentRandomMoveInterval >= randomMoveInterval)
			{
				rb.velocity = Vector2.zero;
				Direction = (TilemapManager.GetRandomNearbyLocation(transform.position, randomMoveDistance, 30) - (Vector2)transform.position).normalized;
				// Debug.Log("Direction - current position: " + (Direction - (Vector2)transform.position));
				currentRandomMoveInterval = 0;
			}
		}

		movement.Move(Direction * currentMoveSpeedMultiplier);
	}

	private void OnDisable()
	{
		OwnerNecromancer.necromancerDiedEvent -= OnOwnerDied;
	}

	public void SubscribeOwnerDeathEvent()
	{
		OwnerNecromancer.necromancerDiedEvent += OnOwnerDied;
	}

	protected override void OnHit(GameObject gameObjectHitBy)
	{
		base.OnHit(gameObjectHitBy);
	}

	protected override void OnEntityHealthZero(GameObject deadObject)
	{
		// change state to dead or something later
		OwnerNecromancer.CurrentlyLiveMinions.Remove(gameObject);
		base.OnEntityHealthZero(deadObject);
		Destroy(deadObject);
	}

	private void OnOwnerDied()
	{
		// destroy all owned projectiles too
		Destroy(gameObject);
	}
}
