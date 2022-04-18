using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
	[SerializeField] private Tilemap terrainTilemap;

	private void Awake()
	{
		// terrainTilemap = GetComponent<Tilemap>();
	}

	public void RemoveTile(Vector3Int tilePosition)
	{
		GameObject tileObjectToRemove = terrainTilemap.GetInstantiatedObject(tilePosition);


		DelayedDestroy(tileObjectToRemove, tilePosition);
		Destroy(tileObjectToRemove);

		// terrainTilemap.SetTile(tilePosition, null);
	}

	private IEnumerator DelayedDestroy(GameObject objectToRemove, Vector3Int tilePosition)
	{
		yield return new WaitForEndOfFrame();
		// Destroy(objectToRemove);
		terrainTilemap.SetTile(tilePosition, null);
	}
}
