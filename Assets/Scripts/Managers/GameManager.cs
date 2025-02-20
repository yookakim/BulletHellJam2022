using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Debug.LogError("Multiple instances of singleton GameManager");
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	[SerializeField] private GameStateEvent gameStateChangedEvent;
	public GameState CurrentGameState
	{
		get => currentGameState;
		set => currentGameState = value;
	}

	public enum GameState
	{
		LoadLevelState,
        PreGameState,
        InGameState,
        VictoryState,
		DefeatState
	}

	private GameState currentGameState;

	public void OnGameplaySceneLoadCompleted()
	{
		// Event called when gameplay scene finishes loading in SceneLoader.
		Debug.Log("finished loading scene");
		ChangeState(GameState.LoadLevelState);

		Time.timeScale = 1;
	}

	public void OnGameEndZoneReached(GameObject playerObject)
	{
		// Debug.Log("player win");
		ChangeState(GameState.VictoryState);
	}

	public void OnPlayerDeath()
	{
		ChangeState(GameState.DefeatState);
	}

	private void ChangeState(GameState newState)
	{
		CurrentGameState = newState;
		gameStateChangedEvent.Raise(newState);
	}
}
