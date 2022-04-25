using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
	public Transform PlayerTransform { get; set; }

	[SerializeField] private int tilemapWidth;
	[SerializeField] private int chunkHeight;

	[SerializeField] private int finalGoalChunk;
	[SerializeField] private float noiseXScale;
	[SerializeField] private float noiseYScale;
	[SerializeField] private float offsetMultiplier;
	[SerializeField] private int minimumDistanceBetweenRooms;
	[SerializeField] private float addTileNoiseOutputThreshold;
	[SerializeField] private float potentialRoomLocationThreshold;
	[SerializeField] private float fleshTileSpawnRange;
	[SerializeField] private float dungeonSpawnFrequencyChunks;
	[SerializeField] private float molechantSpawnFrequencyChunks;
	[SerializeField] private float fleshRoomSpawnChance;
	[SerializeField] private float startFleshRoomSpawningAtChunkNumber;
	[Space]
	[Header("Monster Spawns")]
	[SerializeField] private float skellySpawnRate;
	[SerializeField] private float skellySpawnRateChunkScalar;
	[SerializeField] private float skellyMaxSpawnRate;
	[SerializeField] private float caveBatSpawnRate;
	[SerializeField] private float caveBatSpawnRateChunkScalar;
	[SerializeField] private float caveBatMaxSpawnRate;
	[SerializeField] private GameObject skellyPrefab;
	[SerializeField] private GameObject caveBatPrefab;
	[Space]
	[Header("Tile Types")]
	[SerializeField] private TileBase mapEdgeTile;
	[SerializeField] private TileBase dirtTile;
	[SerializeField] private TileBase dirtRoofTile;
	[SerializeField] private TileBase groundTile;
	[SerializeField] private TileBase fleshTile;
	[SerializeField] private TileBase fleshRoofTile;
	[SerializeField] private TileBase potentialRoomLocationTile;
	[SerializeField] private TileBase roomCenterConfirmedTile;
	[Space]
	[Header("Tilemap References")]
	[SerializeField] private TilemapManager tilemapManager;
	[SerializeField] private Tilemap terrainRoofTilemap;
	[SerializeField] private Tilemap groundTilemap; // tilemap for the ground that player walks on
	[SerializeField] private Tilemap terrainTilemap; // tilemap for actual walls/blocks that player interacts with

	[SerializeField] private GameObject startingAreaPrefab;
	[SerializeField] private GameObject molechantAreaPrefab;
	[SerializeField] private GameObject dungeonPrefab;
	[SerializeField] private GameObject fleshRoomPrefab;
	[SerializeField] private GameObject finalGoalChunkPrefab;
	[SerializeField] private GameObject fullDirtChunkPrefab;
	[SerializeField] private Tilemap roomTilemapPrefab;
	[SerializeField] private Transform testPlayerTransform;

	private float randomSeed;
	private int nextChunkY;
	private int currentChunkNumber;
	private bool currentlyGeneratingChunk;

	private int chunkCounterForMolechantSpawn;
	private int chunkCounterForDungeonSpawn;
	private bool molechantSpawnedThisChunk;
	private bool dungeonSpawnedThisChunk;

	private Dictionary<Vector3Int, TileBase> generatedRooms;
	private PotentialRoomLocationComparer potentialRoomLocationComparer;

	private void Awake()
	{
		// terrainTilemap.SetTile(new Vector3Int(1, 1, 0), testTile);

		randomSeed = Random.Range(0, 1000000);
		// Debug.Log(randomSeed);
		nextChunkY = 0;
		currentChunkNumber = 0;
		chunkCounterForMolechantSpawn = 0;
		chunkCounterForDungeonSpawn = 0;
		currentlyGeneratingChunk = false;
		generatedRooms = new Dictionary<Vector3Int, TileBase>();
		potentialRoomLocationComparer = new PotentialRoomLocationComparer();
	}

	private void Update()
	{
		if (currentlyGeneratingChunk)
		{
			// Debug.Log("GENERATING CHUNK: " + Time.time);
		}

		// if Y of player is less than 24 from last generated chunk Y, generate chunk
		// make sure we don't start a new chunk generation when we are in the middle of one
		// (nextChunkY doesn't get updated for the next several frames the chunk is getting loaded)
		if ((PlayerTransform.position.y >= nextChunkY - 32) && !currentlyGeneratingChunk)
		{
			// GenerateNextChunk();
			StartCoroutine(GenerateNextChunkAsync());
		}
	}


	/*
	 * Generating chunks by distance from player:
	 * 
	 * 1. Keep track of grid position of last generated chunk center
	 * 2. If last generated chunk is less than set Y from player, generate the chunk
	 * 3. 
	 */

	private IEnumerator GenerateNextChunkAsync()
	{
		currentlyGeneratingChunk = true;
		
		int chunkLowerY = nextChunkY - (chunkHeight / 2);
		int chunkUpperY = nextChunkY + (chunkHeight / 2);

		if (currentChunkNumber == 0)
		{
			StartCoroutine(GeneratePrefabRoom(startingAreaPrefab));
		}

		if (currentChunkNumber > finalGoalChunk)
		{
			StartCoroutine(GeneratePrefabRoom(fullDirtChunkPrefab));

			// yield break;
		}

		if (currentChunkNumber == finalGoalChunk)
		{
			StartCoroutine(GeneratePrefabRoom(finalGoalChunkPrefab));

			// yield break;
		}

		for (int i = 0; i < chunkHeight; i++)
		{
			terrainTilemap.SetTile(new Vector3Int(-tilemapWidth / 2 - 1, chunkLowerY + i, 0), mapEdgeTile);
			terrainTilemap.SetTile(new Vector3Int(tilemapWidth / 2, chunkLowerY + i, 0), mapEdgeTile);
		}
		List<PotentialRoomLocation> potentialRoomLocations = new List<PotentialRoomLocation>();

		if (chunkCounterForMolechantSpawn >= molechantSpawnFrequencyChunks && !(currentChunkNumber >= finalGoalChunk))
		{
			chunkCounterForMolechantSpawn = 0;
			molechantSpawnedThisChunk = true;
			StartCoroutine(GeneratePrefabRoom(molechantAreaPrefab));
		}

		if (chunkCounterForDungeonSpawn >= dungeonSpawnFrequencyChunks && !molechantSpawnedThisChunk && !(currentChunkNumber >= finalGoalChunk))
		{

			chunkCounterForDungeonSpawn = 0;
			dungeonSpawnedThisChunk = true;
			StartCoroutine(GeneratePrefabRoom(dungeonPrefab));
		}

		if (currentChunkNumber >= startFleshRoomSpawningAtChunkNumber && !dungeonSpawnedThisChunk && !molechantSpawnedThisChunk && !(currentChunkNumber >= finalGoalChunk))
		{
			float spawnRoll = Random.Range(0f, 1f);

			if (spawnRoll <= fleshRoomSpawnChance)
			{
				StartCoroutine(GeneratePrefabRoom(fleshRoomPrefab));
			}
		}

		for (int i = -tilemapWidth / 2; i < tilemapWidth / 2; i++)
		{
			for (int j = chunkLowerY; j < chunkUpperY; j++)
			{
				// if this tile position doesn't already contain ground from some previous spawn (dungeon, etc):

				if (!groundTilemap.HasTile(new Vector3Int(i, j, 0))) 
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
						// if an empty space has a terrain tile underneath, change that tile to its taller version


						// bool willSpawnSkellyHere = false;
						if (!terrainTilemap.HasTile(new Vector3Int(i, j - 1, 0)))
						{
							if (!molechantSpawnedThisChunk)
							{
								float skellyRandomPercent = Random.Range(0f, 1f);
								float caveBatRandomPercent = Random.Range(0f, 1f);

								float scaledSkellySpawnChance = skellySpawnRate + skellySpawnRateChunkScalar * currentChunkNumber;
								float skellySpawnThreshold = scaledSkellySpawnChance >= skellyMaxSpawnRate ? skellyMaxSpawnRate : scaledSkellySpawnChance;
								float scaledCaveBatSpawnChance = caveBatSpawnRate + caveBatSpawnRateChunkScalar * currentChunkNumber;
								float caveBatThreshold = scaledCaveBatSpawnChance >= caveBatMaxSpawnRate ? caveBatMaxSpawnRate : scaledCaveBatSpawnChance;

								if (skellyRandomPercent <= skellySpawnThreshold)
								{
									Instantiate(skellyPrefab, thisTilePosition, Quaternion.identity);
								}
								if (caveBatRandomPercent <= caveBatThreshold)
								{
									Instantiate(caveBatPrefab, thisTilePosition, Quaternion.identity);
								}
							}
						}
						else
						{
							// Debug.Log(terrainTilemap.GetTile<TileBase>(new Vector3Int(i, j - 1, 0)));
							// TerrainTileData thisTerrainTile = terrainTilemap.GetTile<TerrainTileData>(new Vector3Int(i, j - 1, 0));
							TerrainTile thisTile = terrainTilemap.GetInstantiatedObject(new Vector3Int(i, j - 1, 0)).GetComponent<TerrainTile>();

							terrainTilemap.SetTile(new Vector3Int(i, j - 1, 0), thisTile.TallerColliderVersion);
							// thisTerrainTile.gameObject
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

			yield return null;
		}

		potentialRoomLocations.Sort(potentialRoomLocationComparer);

		// TrySpawnRoom(potentialRoomLocations);

		currentChunkNumber++;
		nextChunkY += chunkHeight;
		currentlyGeneratingChunk = false;
		dungeonSpawnedThisChunk = false;
		molechantSpawnedThisChunk = false;
		chunkCounterForDungeonSpawn++;
		chunkCounterForMolechantSpawn++;
		// noiseShiftX += 1 / (float)tilemapWidth;
	}

	private IEnumerator GeneratePrefabRoom(GameObject roomPrefab)
	{
		TilemapPrefabContainer roomContainer = roomPrefab.GetComponent<TilemapPrefabContainer>();
		// Debug.Log(dungeonContainer.GetGameObjectsList()[0].transform.position);
		List<GameObject> objectsToInstantiate = roomContainer.GetGameObjectsList();

		Tilemap prefabWalls = roomContainer.GetTerrainTilemap();
		Tilemap prefabFloor = roomContainer.GetGroundTilemap();
		Tilemap prefabRoof = roomContainer.GetRoofTilemap();

		BoundsInt prefabBounds = roomContainer.GetBoundsOfPrefabTilemap();

		int spawnMinX = -tilemapWidth / 2 + prefabBounds.size.x / 2;
		int spawnMaxX = (tilemapWidth / 2) - prefabBounds.size.x / 2;
		int spawnMinY = -chunkHeight / 2 + prefabBounds.size.y / 2;
		int spawnMaxY = (chunkHeight / 2) - prefabBounds.size.y / 2;

		Vector3Int spawnPosition = new Vector3Int(Random.Range(spawnMinX, spawnMaxX), Random.Range(spawnMinY, spawnMaxY) + nextChunkY, 0);

		if (objectsToInstantiate != null)
		{
			foreach (GameObject gameObject in objectsToInstantiate)
			{
				Instantiate(gameObject, gameObject.transform.position + (spawnPosition - prefabBounds.center), gameObject.transform.rotation);
			}
		}



		BoundsInt.PositionEnumerator positionsInPrefabBounds = prefabBounds.allPositionsWithin;

		Debug.Log(prefabBounds);
		Debug.Log(spawnPosition.y + ", " + nextChunkY);


		foreach (Vector3Int position in positionsInPrefabBounds)
		{
			// GameObject wallToInstantiate = dungeonWalls.GetObjectToInstantiate(position);

			TileBase tileToInstantiate = prefabFloor.GetTile(position);

			if (tileToInstantiate != null)
			{
				Vector3Int floorTilePosition = new Vector3Int(spawnPosition.x + position.x, spawnPosition.y + position.y, 0);

				groundTilemap.SetTile(floorTilePosition, tileToInstantiate);
			}
		}

		yield return null;

		foreach (Vector3Int position in positionsInPrefabBounds)
		{
			// GameObject wallToInstantiate = dungeonWalls.GetObjectToInstantiate(position);

			TileBase tileToInstantiate = prefabWalls.GetTile(position);

			if (tileToInstantiate != null)
			{
				Vector3Int wallTilePosition = new Vector3Int(spawnPosition.x + position.x, spawnPosition.y + position.y, 0);

				terrainTilemap.SetTile(wallTilePosition, tileToInstantiate);
			}
		}

		yield return null;

		foreach (Vector3Int position in positionsInPrefabBounds)
		{
			// GameObject wallToInstantiate = dungeonWalls.GetObjectToInstantiate(position);

			TileBase tileToInstantiate = prefabRoof.GetTile(position);

			if (tileToInstantiate != null)
			{
				Vector3Int roofTilePosition = new Vector3Int(spawnPosition.x + position.x, spawnPosition.y + position.y, 0);

				terrainRoofTilemap.SetTile(roofTilePosition, tileToInstantiate);
			}
		}

		yield return null;
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
