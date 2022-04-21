using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainTile : MonoBehaviour
{

	[SerializeField] protected Hitbox hitbox;
	[SerializeField] protected Sprite[] tileBreakSprites = new Sprite[3];
	[SerializeField] protected SpriteRenderer tileBreakSpriteRenderer;
	[SerializeField] protected SpriteRenderer tileBreakSpriteRendererRoof;
	[SerializeField] protected TerrainTileData tallerColliderVersion; // for tiles with empty space above it, change to this one

	public Vector3Int TilePosition { get; set; }
	public TilemapManager TilemapManager { get; set; }
	// public TerrainTileData Tile
	public TerrainTileData TallerColliderVersion { get => tallerColliderVersion; }

	protected Health health;
	protected LayerMask bombMask;

	protected virtual void Awake()
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

	public void DestroyTile()
	{
		TilemapManager.RemoveTile(TilePosition);
	}

	protected abstract void OnHit(GameObject gameObjectHitBy);
	protected abstract void OnHealthChanged(Health healthComponent, int damageAmountDealt);
	protected abstract void OnHealthZero(GameObject tileObject);

}
