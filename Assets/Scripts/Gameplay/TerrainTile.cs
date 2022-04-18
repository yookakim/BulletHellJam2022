using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile : MonoBehaviour
{
    public Vector3Int TilePosition { get; set; }

	public TilemapManager TilemapManager { get; set; }

	private void Awake()
	{
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Debug.Log(TilemapManager);
		// DestroyTile();
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// DestroyTile();
	}

	public void DestroyTile()
	{
		TilemapManager.RemoveTile(TilePosition);
	}
}
