using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelLoader : MonoBehaviour
{
	[SerializeField] private Transform playerSpawnPoint;
	[SerializeField] private CinemachineVirtualCamera cinemachine;

	// maybe in the future the GameManager will give the loader a copy to spawn
	[SerializeField] private GameObject playerPrefab;
	[SerializeField] private GameObject tilemapPrefab;

	private GameManager gameManager;

	private void Awake()
	{
		gameManager = GameManager.Instance;
	}

	private void Start()
	{
		Debug.Log("level loader start");
	}

	/// <summary>
	/// On level start, pause, end, etc.
	/// </summary>
	/// <param name="newState"></param>
	public void OnGameStateChanged(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.LoadLevelState)
        {
            Debug.Log("Load level state received in LevelLoader, instantiating objects...");

			GameObject playerObject = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
			GameObject tilemapObject = Instantiate(tilemapPrefab, Vector2.zero, Quaternion.identity);

			TilemapGenerator tilemapGenerator = tilemapObject.GetComponent<TilemapGenerator>();
			tilemapGenerator.PlayerTransform = playerObject.transform;

			cinemachine.Follow = playerObject.transform;
			cinemachine.GetComponent<CinemachineCameraController>().PlayerTransform = playerObject.transform;
        }
    }
}
