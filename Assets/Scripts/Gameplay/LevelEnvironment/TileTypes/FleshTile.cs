using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshTile : TerrainTile
{
	[SerializeField] int numberCoinsDropped;

	private WeaponController weaponController;
	private PlayerScanRaycast scanForPlayer;

	private CoinSpawner coinSpawner;
	private EntityTweenEffects entityTweenFX;

	protected override void Awake()
	{
		base.Awake();
		weaponController = GetComponent<WeaponController>();
		coinSpawner = GetComponent<CoinSpawner>();
		entityTweenFX = GetComponent<EntityTweenEffects>();
		scanForPlayer = GetComponent<PlayerScanRaycast>();
	}

	private void Update()
	{
		if (scanForPlayer.HasSightOfPlayer)
		{
			weaponController.AttemptUse();
			weaponController.CurrentWeaponTarget = scanForPlayer.CurrentPlayerPosition;
		}
	}

	protected override void OnHealthChanged(Health healthComponent, int damageAmountDealt)
	{
		
	}

	protected override void OnHealthZero(GameObject tileObject)
	{
		DestroyTile();

		// maybe we spawn some flesh pile entity later
	}

	protected override void OnHit(GameObject gameObjectHitBy)
	{

		DamageComponent incomingDamageComponent = gameObjectHitBy.GetComponent<DamageComponent>();

		if (incomingDamageComponent != null)
		{
			if (incomingDamageComponent.DamageAlignment != hitbox.ownerAlignment || incomingDamageComponent.HitsAllies)
			{
				// start flash tween
				entityTweenFX.OnDamageTween();

				health.DealDamage(incomingDamageComponent.DamageAmount);
			}
		}
	}
}
