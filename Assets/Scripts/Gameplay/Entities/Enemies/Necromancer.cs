using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : Entity
{
	public float TimeInCurrentState { get; set; }

	[SerializeField] private TransformReference playerTargetTransform;
	[SerializeField] private PlayerScanRaycast playerScanRaycast;
	[SerializeField] private List<WeaponController> necromancerAttacks;
	[SerializeField] private float phaseOneCycleInterval;
	[SerializeField] private float phaseOneAttackAngleRange;

	private float elapsedPhaseOneAttackInterval;

	private Vector2 currentAttackDirection;

	private void Awake()
	{
		TimeInCurrentState = 0;

		foreach (WeaponController weaponController in necromancerAttacks)
		{
			weaponController.LiveWeaponTargetTransform = playerTargetTransform.reference;
		}
	}

	protected override void OnHit(GameObject gameObjectHitBy)
	{
		base.OnHit(gameObjectHitBy);
	}

	protected override void OnEntityHealthZero(GameObject deadObject)
	{
		// change state to dead or something later
		base.OnEntityHealthZero(deadObject);
		Destroy(deadObject);
	}

	public void AcceleratingBulletAttackPhase()
	{
		necromancerAttacks[0].AttemptUse();

		necromancerAttacks[0].CurrentWeaponTarget = playerScanRaycast.CurrentPlayerPosition;
	}
}
