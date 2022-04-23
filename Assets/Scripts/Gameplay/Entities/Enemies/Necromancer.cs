using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Necromancer : Entity
{
	public float TimeInCurrentState { get; set; }

	public float IdleToOneTransitionTime { get => idleToOneTransitionTime; }
	public float PhaseOneTimeLength { get => phaseOneTimeLength; }
	public float OneToTwoTransitionTime { get => oneToTwoTransitionTime; }
	public float PhaseTwoTimeLength { get => phaseTwoTimeLength; }
	public float TwoToOneTransitionTime { get => twoToOneTransitionTime; }
	public int MaxMinionsSpawned { get => maxMinionsSpawned; }
	public int NumberOfMinionsSpawnedThisPhase { get => numberOfMinionsSpawnedThisPhase; set => numberOfMinionsSpawnedThisPhase = value; }
	public bool CurrentlySpawningMinions { get; set; }
	public bool NecromancerCurrentlyHasMinions
	{
		get
		{
			return CurrentlyLiveMinions.Count <= 0 ? false : true;
		}
	}
	public Dictionary<GameObject, GameObject> CurrentlyLiveMinions { get; set; }

	public event Action necromancerDiedEvent;

	[SerializeField] private TransformReference playerTargetTransform;
	[SerializeField] private PlayerScanRaycast playerScanRaycast;
	[SerializeField] private List<WeaponController> necromancerAttacks;
	[SerializeField] private float idleToOneTransitionTime;
	[SerializeField] private float phaseOneTimeLength;
	[SerializeField] private float oneToTwoTransitionTime;
	[SerializeField] private float phaseTwoTimeLength;
	[SerializeField] private float twoToOneTransitionTime;
	[SerializeField] private float phaseOneAttackAngleRange;

	[SerializeField] private GameObject fireMinionPrefab;
	[SerializeField] private float fireMinionSpawnInterval;
	[SerializeField] private float fireMinionLaunchAngleRange;
	[SerializeField] private int maxMinionsSpawned;

	private float elapsedPhaseOneAttackInterval;

	private Vector2 currentAttackDirection;
	private float currentFireMinionSpawningTimer;
	private int numberOfMinionsSpawnedThisPhase;

	private void Awake()
	{
		TimeInCurrentState = 0;

		CurrentlyLiveMinions = new Dictionary<GameObject, GameObject>();

		foreach (WeaponController weaponController in necromancerAttacks)
		{
			weaponController.LiveWeaponTargetTransform = playerTargetTransform.reference;
		}
	}

	private void Update()
	{
		TimeInCurrentState += Time.deltaTime;
	}

	protected override void OnHit(GameObject gameObjectHitBy)
	{
		base.OnHit(gameObjectHitBy);
	}

	protected override void OnEntityHealthZero(GameObject deadObject)
	{
		// change state to dead or something later
		necromancerDiedEvent?.Invoke();
		base.OnEntityHealthZero(deadObject);
	}

	public void PhaseOne()
	{
		necromancerAttacks[0].AttemptUse();

		necromancerAttacks[0].CurrentWeaponTarget = playerScanRaycast.CurrentPlayerPosition;
	}

	public void PhaseTwo()
	{
		currentFireMinionSpawningTimer += Time.deltaTime;
		if (currentFireMinionSpawningTimer >= fireMinionSpawnInterval)
		{
			float randomLaunchAngle = UnityEngine.Random.Range(-fireMinionLaunchAngleRange / 2, fireMinionLaunchAngleRange / 2);

			Vector2 randomLaunchDirection = HelperFunctions.RotateVectorRad(Vector2.down, randomLaunchAngle * Mathf.Deg2Rad);

			GameObject fireMinionObject = Instantiate(fireMinionPrefab, transform.position, Quaternion.identity);

			CurrentlyLiveMinions.Add(fireMinionObject, fireMinionObject);

			NecromancerFireMinion fireMinion = fireMinionObject.GetComponent<NecromancerFireMinion>();

			fireMinion.OwnerNecromancer = this;
			fireMinion.SubscribeOwnerDeathEvent();
			fireMinion.Direction = randomLaunchDirection;

			currentFireMinionSpawningTimer = 0;
			numberOfMinionsSpawnedThisPhase++;
		}
	}

	public void PhaseTwoB()
	{
		// fire large shots at the player
	}
}
