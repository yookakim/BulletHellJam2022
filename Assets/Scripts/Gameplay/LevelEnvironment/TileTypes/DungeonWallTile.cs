using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonWallTile : TerrainTile
{
	// perhaps dungeon tiles can do specific things like stay invulnerable while the boss inside it is alive or something

	protected override void OnHealthChanged(Health healthComponent, int damageAmountDealt)
	{
		if (healthComponent.CurrentHealth / (float)healthComponent.MaxHealth >= 0.75)
		{
			tileBreakSpriteRenderer.sprite = null;
			tileBreakSpriteRendererRoof.sprite = null;
		}
		else if (healthComponent.CurrentHealth / (float)healthComponent.MaxHealth >= 0.50 &&
			healthComponent.CurrentHealth / (float)healthComponent.MaxHealth < 0.75)
		{
			tileBreakSpriteRenderer.sprite = tileBreakSprites[0];
			tileBreakSpriteRendererRoof.sprite = tileBreakSprites[0];
		}
		else if (healthComponent.CurrentHealth / (float)healthComponent.MaxHealth >= 0.25 &&
			healthComponent.CurrentHealth / (float)healthComponent.MaxHealth < 0.50)
		{
			tileBreakSpriteRenderer.sprite = tileBreakSprites[1];
			tileBreakSpriteRendererRoof.sprite = tileBreakSprites[1];
		}
		else
		{
			tileBreakSpriteRenderer.sprite = tileBreakSprites[2];
			tileBreakSpriteRendererRoof.sprite = tileBreakSprites[2];
		}
	}

	protected override void OnHealthZero(GameObject tileObject)
	{
		DestroyTile();
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

			// TODO: if damage component does damage to terrain, then deal the damage 
			// health.DealDamage(projectileDamage.DamageAmount);
		}

		if (gameObjectHitBy.layer == LayerMask.NameToLayer("Melee"))
		{
			DamageComponent meleeDamage = gameObjectHitBy.GetComponent<DamageComponent>();

			health.DealDamage(meleeDamage.DamageAmount);
		}
	}
}
