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
	[SerializeField] private float addTileNoiseOutputThreshold;
	[SerializeField] private int minimumDistanceBetweenRooms;
	[SerializeField] private float potentialRoomLocationThreshold;

	[SerializeField] private TileBase testTile;
	[SerializeField] private TileBase groundTile;
	[SerializeField] private TileBase roundRoomCenterTile;
	[SerializeField] private TileBase roomCenterConfirmedTile;
	[SerializeField] private Tilemap groundTilemap; // tilemap for the ground that player walks on
	[SerializeField] private Tilemap terrainTilemap; // tilemap for actual walls/blocks that player interacts with

	[SerializeField] private Transform testPlayerTransform;

	private float randomSeed;
	private int nextChunkY;
	private float noiseShiftX;
	private Dictionary<Vector3Int, TileBase> generatedRooms;
	private PotentialRoomLocationComparer potentialRoomLocationComparer;

	private void Awake()
	{
		// terrainTilemap.SetTile(new Vector3Int(1, 1, 0), testTile);

		randomSeed = Random.Range(0, 1000000);
		// Debug.Log(randomSeed);
		nextChunkY = 0;
		noiseShiftX = 0;
		generatedRooms = new Dictionary<Vector3Int, TileBase>();
		potentialRoomLocationComparer = new PotentialRoomLocationComparer();
		// GenerateRandomArea();
		// GenerateRandomAreaUsingNoise();
	}

	private void Update()
	{
		if (Input.GetKeyDown("r"))
		{
			GenerateRandomAreaUsingNoise();
		}

		// if Y of player is less than 24 from last generated chunk Y, generate chunk
		if (testPlayerTransform.position.y >= nextChunkY - 32)
		{
			GenerateNextChunk();
		}
	}

	private void GenerateRandomArea()
	{
		for (int i = -tilemapWidth / 2; i < tilemapWidth / 2; i++)
		{
			for (int j = -chunkHeight / 2; j < chunkHeight / 2; j++)
			{
				groundTilemap.SetTile(new Vector3Int(i, j, 0), testTile);
			}
		}
	}

	private void GenerateRandomAreaUsingNoise()
	{
		Debug.Log(Time.time);
		groundTilemap.ClearAllTiles();
		for (int i = -tilemapWidth / 2; i < tilemapWidth / 2; i++)
		{
			for (int j = -chunkHeight / 2; j < chunkHeight / 2; j++)
			{
				// divide by th
				float inputX = 0.5f + i / (float)tilemapWidth;
				float inputY = 0.5f + j / (float)chunkHeight;
				float noiseResult = Mathf.PerlinNoise(inputX * noiseXScale, (inputY * noiseYScale) + Time.time);
				Debug.Log("noise input " + inputX + ", " + inputY + ", noise result: " + noiseResult);
				if (noiseResult >= addTileNoiseOutputThreshold)
				{

					groundTilemap.SetTile(new Vector3Int(i, j, 0), testTile);
				}
				else
				{
					groundTilemap.SetTile(new Vector3Int(i, j, 0), null);
				}

				if (noiseResult >= potentialRoomLocationThreshold)
				{
					groundTilemap.SetTile(new Vector3Int(i, j, 0), roundRoomCenterTile);
				}
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

		List<PotentialRoomLocation> potentialRoomLocations = new List<PotentialRoomLocation>();

		for (int i = -tilemapWidth / 2; i < tilemapWidth / 2; i++)
		{
			for (int j = chunkLowerY; j < chunkUpperY; j++)
			{
				// divide by th
				groundTilemap.SetTile(new Vector3Int(i, j, 0), groundTile);
				float inputX = 0.5f + i / (float)tilemapWidth;
				float inputY = ((j + chunkHeight / 2) % chunkHeight) / (float)chunkHeight;
				// float inputY = j;

				float offset = nextChunkY / chunkHeight;

				float noiseResult = 
					Mathf.PerlinNoise(inputX * noiseXScale, (inputY * noiseYScale) + randomSeed + offset * noiseYScale);
				// Debug.Log("noise input " + inputX + ", " + inputY + ", noise result: " + noiseResult);
				if (noiseResult >= addTileNoiseOutputThreshold)
				{
					// Debug.Log("hmmmm");
					terrainTilemap.SetTile(new Vector3Int(i, j, 0), testTile);
				}
				else
				{
					terrainTilemap.SetTile(new Vector3Int(i, j, 0), null);
				}

				if (noiseResult >= potentialRoomLocationThreshold)
				{
					terrainTilemap.SetTile(new Vector3Int(i, j, 0), roundRoomCenterTile);

					PotentialRoomLocation potentialLocation = new PotentialRoomLocation(new Vector3Int(i, j, 0), noiseResult);

					potentialRoomLocations.Add(potentialLocation);
				}
			}
		}
		
		potentialRoomLocations.Sort(potentialRoomLocationComparer);

		TrySpawnRoom(potentialRoomLocations);

		nextChunkY += chunkHeight;
		// noiseShiftX += 1 / (float)tilemapWidth;
	}

	private void TrySpawnRoom(List<PotentialRoomLocation> potentialRoomLocations)
	{
/*		Debug.Log("PRINTING NOISE OUTPUTS OF POTENTIAL ROOMS IN ORDER");
		for (int i = 0; i < potentialRoomLocations.Count; i++)
		{
			Debug.Log(potentialRoomLocations[i].noiseOutputValue);
		}*/

		// for each grid location in potential locations, check if there is already room nearby;
		// if not, then add that coordinate to dictionary of rooms and return
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
	}

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
					Debug.Log("Attempted to generate room but room already existed nearby");
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
