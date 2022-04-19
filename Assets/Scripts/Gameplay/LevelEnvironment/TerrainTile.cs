using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile : MonoBehaviour
{

	[SerializeField] private Hitbox hitbox;
	[SerializeField] private Sprite[] tileBreakSprites = new Sprite[3];
	[SerializeField] private SpriteRenderer tileBreakSpriteRenderer;
	[SerializeField] private SpriteRenderer tileBreakSpriteRendererRoof;

    public Vector3Int TilePosition { get; set; }
	public TilemapManager TilemapManager { get; set; }

	private Health health;
	private LayerMask bombMask;

	private void Awake()
	{


		health = GetComponent<Health>();
		bombMask = LayerMask.GetMask("Bomb");
	}

	private void OnEnable()
	{
		hitbox.hitboxTriggeredEvent += OnHit;
		health.HealthChangedEvent += OnHealthChanged;
		health.HealthZeroEvent += OnHealthZero;
	}


	private void OnDisable()
	{
		hitbox.hitboxTriggeredEvent -= OnHit;
		health.HealthChangedEvent -= OnHealthChanged;
		health.HealthZeroEvent -= OnHealthZero;
	}

	private void OnHit(GameObject gameObjectHitBy)
	{
		// if hit by bomb, destroy itself
		if (LayerMask.GetMask(LayerMask.LayerToName(gameObjectHitBy.layer)) == bombMask)
		{
			DestroyTile();
		}

		if (gameObjectHitBy.layer == LayerMask.NameToLayer("Projectile"))
		{
			DamageComponent projectileDamage = gameObjectHitBy.GetComponent<DamageComponent>();

			health.DealDamage(projectileDamage.DamageAmount);
		}
	}
	private void OnHealthChanged(Health healthComponent, int damageAmountDealt)
	{
		Debug.Log(healthComponent.CurrentHealth / (float)healthComponent.MaxHealth);
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

	public void DestroyTile()
	{
		TilemapManager.RemoveTile(TilePosition);
	}

	private void OnHealthZero(GameObject tileObject)
	{
		DestroyTile();
	}
}
