using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateChangedEventListener : MonoBehaviour
{
	[SerializeField] private GameStateEvent gameEvent;
	[SerializeField] private UnityEvent<GameManager.GameState> response;

	private void OnEnable()
	{
		gameEvent.RegisterListener(this);
	}

	private void OnDisable()
	{
		gameEvent.UnregisterListener(this);
	}

	public virtual void OnEventRaised(GameManager.GameState newState)
	{
		response?.Invoke(newState);
	}
}
