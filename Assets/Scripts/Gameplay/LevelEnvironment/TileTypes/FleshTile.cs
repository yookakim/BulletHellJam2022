using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshTile : TerrainTile
{

	private WeaponController weaponController;
	private PlayerScanRaycast scanForPlayer;

	protected override void Awake()
	{
		base.Awake();
		weaponController = GetComponent<WeaponController>();
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
		if (LayerMask.GetMask(LayerMask.LayerToName(gameObjectHitBy.layer)) == bombMask)
		{
			DestroyTile();
		}

		if (gameObjectHitBy.layer == LayerMask.NameToLayer("Projectile"))
		{
			DamageComponent projectileDamage = gameObjectHitBy.GetComponent<DamageComponent>();

			health.DealDamage(projectileDamage.DamageAmount);
		}

		if (gameObjectHitBy.layer == LayerMask.NameToLayer("Melee"))
		{
			DamageComponent meleeDamage = gameObjectHitBy.GetComponent<DamageComponent>();

			health.DealDamage(meleeDamage.DamageAmount);
		}
	}
}
