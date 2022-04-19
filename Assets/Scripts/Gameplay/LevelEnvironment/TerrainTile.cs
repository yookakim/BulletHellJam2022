using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile : MonoBehaviour
{
	[SerializeField] private Hitbox hitbox;
    public Vector3Int TilePosition { get; set; }
	public TilemapManager TilemapManager { get; set; }

	private LayerMask bombMask;

	private void Awake()
	{
		hitbox.hitboxTriggeredEvent += OnHit;
		bombMask = LayerMask.GetMask("Bomb");
	}

	private void OnHit(GameObject gameObjectHitBy)
	{
		// if hit by bomb, destroy itself
		if (LayerMask.GetMask(LayerMask.LayerToName(gameObjectHitBy.layer)) == bombMask)
		{
			DestroyTile();
		}
	}

	public void DestroyTile()
	{
		TilemapManager.RemoveTile(TilePosition);
	}
}
