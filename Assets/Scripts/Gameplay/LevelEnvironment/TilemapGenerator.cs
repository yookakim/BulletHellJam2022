using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
	[SerializeField] private int tilemapWidth;
	[SerializeField] private int chunkHeight;

	[SerializeField] private float noiseXScale;
	[SerializeField] private float noiseYScale;
	[SerializeField] private float offsetMultiplier;
	[SerializeField] private int minimumDistanceBetweenRooms;
	[SerializeField] private float addTileNoiseOutputThreshold;
	[SerializeField] private float potentialRoomLocationThreshold;
	[SerializeField] private float fleshTileSpawnRange;

	[SerializeField] private float skellySpawnRate;
	[SerializeField] private float caveBatSpawnRate;
	[SerializeField] private GameObject skellyPrefab;
	[SerializeField] private GameObject caveBatPrefab;

	[SerializeField] private TileBase mapEdgeTile;
	[SerializeField] private TileBase dirtTile;
	[SerializeField] private TileBase dirtRoofTile;
	[SerializeField] private TileBase groundTile;
	[SerializeField] private TileBase fleshTile;
	[SerializeField] private TileBase fleshRoofTile;
	[SerializeField] private TileBase potentialRoomLocationTile;
	[SerializeField] private TileBase roomCenterConfirmedTile;

	[SerializeField] private TilemapManager tilemapManager;
	[SerializeField] private Tilemap terrainRoofTilemap;
	[SerializeField] private Tilemap groundTilemap; // tilemap for the ground that player walks on
	[SerializeField] private Tilemap terrainTilemap; // tilemap for actual walls/blocks that player interacts with

	[SerializeField] private Tilemap roomTilemapPrefab;
	[SerializeField] private Transform testPlayerTransform;

	private float randomSeed;
	private int nextChunkY;
	private Dictionary<Vector3Int, TileBase> generatedRooms;
	private PotentialRoomLocationComparer potentialRoomLocationComparer;

	private void Awake()
	{
		// terrainTilemap.SetTile(new Vector3Int(1, 1, 0), testTile);

		randomSeed = Random.Range(0, 1000000);
		// Debug.Log(randomSeed);
		nextChunkY = 16;
		generatedRooms = new Dictionary<Vector3Int, TileBase>();
		potentialRoomLocationComparer = new PotentialRoomLocationComparer();
		// GenerateRandomArea();
		// GenerateRandomAreaUsingNoise();
		GenerateStartingArea();
	}

	private void Update()
	{

		// if Y of player is less than 24 from last generated chunk Y, generate chunk
		if (testPlayerTransform.position.y >= nextChunkY - 32)
		{
			GenerateNextChunk();
		}
	}

	private void GenerateStartingArea()
	{
		for (int i = -tilemapWidth / 2; i < tilemapWidth / 2; i++)
		{
			for (int j = -chunkHeight / 2; j < chunkHeight; j++)
			{
				groundTilemap.SetTile(new Vector3Int(i, j, 0), groundTile);
			}
		}
	}

	private void GenerateRandomArea()
	{
		for (int i = -tilemapWidth / 2; i < tilemapWidth / 2; i++)
		{
			for (int j = -chunkHeight / 2; j < chunkHeight / 2; j++)
			{
				groundTilemap.SetTile(new Vector3Int(i, j, 0), dirtTile);
			}
		}
	}
	
	/*
	 * Generating chunks by distance from player:
	 * 
	 * 1. Keep track of grid position of last generated chunk center
	 * 2. If last generated chunk is less than set Y from player, generate the chunk
	 * 3. 
	 */

	/// <summary>
	/// Generate the next map chunk at the next map chunk Y location
	/// </summary>
	private void GenerateNextChunk()
	{
		int chunkLowerY = nextChunkY - (chunkHeight / 2);
		int chunkUpperY = nextChunkY + (chunkHeight / 2);
		for (int i = 0; i < chunkHeight; i++)
		{
			terrainTilemap.SetTile(new Vector3Int(-tilemapWidth / 2 - 1, chunkLowerY + i, 0), mapEdgeTile);
			terrainTilemap.SetTile(new Vector3Int(tilemapWidth / 2, chunkLowerY + i, 0), mapEdgeTile);
		}
		List<PotentialRoomLocation> potentialRoomLocations = new List<PotentialRoomLocation>();

		for (int i = -tilemapWidth / 2; i < tilemapWidth / 2; i++)
		{
			for (int j = chunkLowerY; j < chunkUpperY; j++)
			{
				groundTilemap.SetTile(new Vector3Int(i, j, 0), groundTile);

				Vector2 thisTilePosition = groundTilemap.GetCellCenterWorld(new Vector3Int(i, j, 0));

				float inputX = 0.5f + i / (float)tilemapWidth;
				float inputY = ((j + chunkHeight / 2) % chunkHeight) / (float)chunkHeight;

				float offset = nextChunkY / chunkHeight;

				float noiseResult = 
					Mathf.PerlinNoise(inputX * noiseXScale, (inputY * noiseYScale) + randomSeed + offset * noiseYScale);
				// Debug.Log("noise input " + inputX + ", " + inputY + ", noise result: " + noiseResult);
				if (noiseResult >= addTileNoiseOutputThreshold && noiseResult <= potentialRoomLocationThreshold)
				{
					// Debug.Log("hmmmm");
					terrainRoofTilemap.SetTile(new Vector3Int(i, j, 0), dirtRoofTile);
					terrainTilemap.SetTile(new Vector3Int(i, j, 0), dirtTile);
				}
				else if (noiseResult >= potentialRoomLocationThreshold && noiseResult <= potentialRoomLocationThreshold + fleshTileSpawnRange)
				{
					if (!terrainTilemap.HasTile(new Vector3Int(i, j - 1, 0)))
					{
						terrainRoofTilemap.SetTile(new Vector3Int(i, j, 0), fleshRoofTile);
						terrainTilemap.SetTile(new Vector3Int(i, j, 0), fleshTile);
					}
				}
				else
				{
					// bool willSpawnSkellyHere = false;
					if (!terrainTilemap.HasTile(new Vector3Int(i, j - 1, 0)))
					{
						float skellyRandomPercent = Random.Range(0f, 1f);
						float caveBatRandomPercent = Random.Range(0f, 1f);

						if (skellyRandomPercent <= skellySpawnRate)
						{
							Instantiate(skellyPrefab, thisTilePosition, Quaternion.identity);
						}
						if (caveBatRandomPercent <= caveBatSpawnRate)
						{
							Instantiate(caveBatPrefab, thisTilePosition, Quaternion.identity);
						}
					}
				}

				if (noiseResult >= potentialRoomLocationThreshold)
				{
					// terrainTilemap.SetTile(new Vector3Int(i, j, 0), potentialRoomLocationTile);
					// tilemapManager.RemoveTile()
					// PotentialRoomLocation potentialLocation = new PotentialRoomLocation(new Vector3Int(i, j, 0), noiseResult);

					// potentialRoomLocations.Add(potentialLocation);
				}
			}
		}
		
		potentialRoomLocations.Sort(potentialRoomLocationComparer);

		// TrySpawnRoom(potentialRoomLocations);

		nextChunkY += chunkHeight;
		// noiseShiftX += 1 / (float)tilemapWidth;
	}

/*	private void TrySpawnRoom(List<PotentialRoomLocation> potentialRoomLocations)
	{
		for (int i = 0; i < potentialRoomLocations.Count; i++)
		{
			if (!ConfirmedRoomExistsNearby(potentialRoomLocations[i].gridPosition))
			{

				terrainTilemap.SetTile(potentialRoomLocations[i].gridPosition, roomCenterConfirmedTile);
				generatedRooms.Add(potentialRoomLocations[i].gridPosition, roomCenterConfirmedTile);
				Debug.Log("Room successfully generated. Current room count: " + generatedRooms.Count);
				// return;
			}
		}
	}*/

	private bool ConfirmedRoomExistsNearby(Vector3Int positionToCheck)
	{
		bool confirmedRoomExistsNearby = false;
		int xMin = positionToCheck.x - minimumDistanceBetweenRooms;
		int xMax = positionToCheck.x + minimumDistanceBetweenRooms;
		int yMin = positionToCheck.y - minimumDistanceBetweenRooms;
		int yMax = positionToCheck.y + minimumDistanceBetweenRooms;

		for (int i = xMin; i < xMax; i++)
		{
			for (int j = yMin; j < yMax; j++)
			{
				if (generatedRooms.ContainsKey(new Vector3Int(i, j, 0)))
				{
					// Debug.Log("Attempted to generate room but room already existed nearby");
					// Debug.Log(generatedRooms.Count);
					return true;
				}
			}
		}

		return confirmedRoomExistsNearby;
	}

	public struct PotentialRoomLocation
	{
		public Vector3Int gridPosition;
		public float noiseOutputValue;

		public PotentialRoomLocation(Vector3Int gridPosition, float noiseOutputValue)
		{
			this.gridPosition = gridPosition;
			this.noiseOutputValue = noiseOutputValue;
		}
	}

	public class PotentialRoomLocationComparer : IComparer<PotentialRoomLocation>
	{
		public int Compare(PotentialRoomLocation x, PotentialRoomLocation y)
		{
			// throw new System.NotImplementedException();
			return -1 * x.noiseOutputValue.CompareTo(y.noiseOutputValue);
		}
	}
}
