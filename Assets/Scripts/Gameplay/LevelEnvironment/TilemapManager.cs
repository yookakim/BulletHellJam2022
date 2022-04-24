using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
	[SerializeField] private Tilemap terrainTilemap;
	[SerializeField] private Tilemap terrainRoofTilemap;

	[SerializeField] private float randomSearchRetryAngleInterval;

	private void Awake()
	{
		// terrainTilemap = GetComponent<Tilemap>();
	}

	public void RemoveTile(Vector3Int tilePosition)
	{
		if (!terrainTilemap.HasTile(tilePosition))
		{
			Debug.LogError("Tried to remove nonexisting tile");
			return;
		}
		GameObject tileObjectToRemove = terrainTilemap.GetInstantiatedObject(tilePosition);
		

		StartCoroutine(DelayedDestroy(tileObjectToRemove, tilePosition));
		Destroy(tileObjectToRemove);




		// terrainTilemap.SetTile(tilePosition, null);
	}

	public void ReplaceTile(Vector3Int postion, TileBase replaceWith)
	{

	}

	public void RemoveTileBlock(BoundsInt boundsToRemove)
	{
		BoundsInt.PositionEnumerator positionsToRemove = boundsToRemove.allPositionsWithin;
		// StartCoroutine(DelayedDestroyBlock(boundsToRemove));
		foreach (Vector3Int position in positionsToRemove)
		{
			RemoveTile(position);
			// GameObject tileObjectToRemove = terrainTilemap.GetInstantiatedObject(position);
			// Destroy(tileObjectToRemove);
		}
	}

	private IEnumerator DelayedDestroy(GameObject objectToRemove, Vector3Int tilePosition)
	{
		yield return new WaitForEndOfFrame();
		// Destroy(objectToRemove);
		terrainTilemap.SetTile(tilePosition, null);
		terrainRoofTilemap.SetTile(tilePosition, null);


		if (terrainTilemap.HasTile(new Vector3Int(tilePosition.x, tilePosition.y - 1, 0)))
		{
			GameObject tileOneBelowObject = terrainTilemap.GetInstantiatedObject(new Vector3Int(tilePosition.x, tilePosition.y - 1, 0));

			if (tileOneBelowObject != null)
			{
				TerrainTile tileOneBelow = tileOneBelowObject.GetComponent<TerrainTile>();
				StartCoroutine(DelayedReplaceTile(new Vector3Int(tilePosition.x, tilePosition.y - 1, 0), tileOneBelow.TallerColliderVersion));
				Destroy(tileOneBelow.gameObject);

			}
		}
	}

	private IEnumerator DelayedReplaceTile(Vector3Int position, TileBase tileToReplace)
	{
		yield return new WaitForEndOfFrame();
		terrainTilemap.SetTile(position, tileToReplace);
	}

	public static Vector2 GetRandomNearbyLocation(Vector2 position, float distance, float randomSearchRetryAngleInterval)
	{
		Vector2 positionToReturn = Vector2.zero;

		bool randomPositionFound = false;
		int antiFreezeCounter = 0; // LMAO
		float searchAngleIncrement = 0;

		float randomRotateAngle = Random.Range(0, 360);
		Vector2 randomDirection = HelperFunctions.RotateVectorRad(Vector2.up, randomRotateAngle * Mathf.Deg2Rad);

		while (!randomPositionFound)
		{


			Vector2 incrementedSearchDirection = HelperFunctions.RotateVectorRad(randomDirection, searchAngleIncrement * Mathf.Deg2Rad);

			RaycastHit2D raycastCheck = Physics2D.Raycast(position, incrementedSearchDirection, distance, LayerMask.GetMask("Destructible"));

			if (raycastCheck.collider == null)
			{
				positionToReturn = position + (incrementedSearchDirection * distance);
				// Debug.Log(randomDirection);
				Debug.DrawLine(position, positionToReturn, Color.red);
				randomPositionFound = true;
			}
			else
			{
				// Debug.Log("check ran into terrain, checking again: " + raycastCheck.collider.gameObject);
			}

			antiFreezeCounter++;

			if (antiFreezeCounter >= (360 / randomSearchRetryAngleInterval))
			{
				Debug.LogError("Could not find any suitable destination, exiting to avoid freeze");
				return position;
			}

			searchAngleIncrement += randomSearchRetryAngleInterval;
		}

		return positionToReturn;
	}
}
