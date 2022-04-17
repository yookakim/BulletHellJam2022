using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameStateEvent gameStateChangedEvent;
	public GameState CurrentGameState
	{
		get => currentGameState;
		set => currentGameState = value;
	}

	public enum GameState
	{
        PreGameState,
        InGameState,
        PostGameState
	}

	private GameState currentGameState;

	public void OnGameplaySceneLoadCompleted()
	{
		// Event called when gameplay scene finishes loading in SceneLoader.
		ChangeState(GameState.PreGameState);

	}

	private void ChangeState(GameState newState)
	{
		CurrentGameState = newState;
		gameStateChangedEvent.Raise(newState);
	}
}
