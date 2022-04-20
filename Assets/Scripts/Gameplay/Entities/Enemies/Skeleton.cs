using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Entity
{
	[SerializeField] private PlayerScanRaycast playerScanRaycast;
	[SerializeField] private TransformReference playerTransform;
	public float enterAttackRangeThreshold;
	public float exitAttackRangeThreshold;

	public Transform PlayerTransform { get => playerTransform.reference; }

	protected override void OnHit(GameObject gameObjectHitBy)
	{
		base.OnHit(gameObjectHitBy);
	}

	protected override void OnEntityHealthZero(GameObject deadObject)
	{
		// change state to dead or something later
		base.OnEntityHealthZero(deadObject);
		Debug.Log("skelly died");
		Destroy(deadObject);
	}

	public void ChasePlayer()
	{
		Vector2 chaseDirection = (playerTransform.reference.position - transform.position).normalized;
		movement.Move(chaseDirection);
	}

	public void AttackPlayer()
	{
		weaponController.AttemptUse();
		Vector2 chaseDirection = (playerTransform.reference.position - transform.position).normalized / 2;
		movement.Move(chaseDirection);
	}

	public bool PlayerWithinAttackDistance()
	{
		return (playerTransform.reference.transform.position - transform.position).magnitude <= enterAttackRangeThreshold;
	}
}
