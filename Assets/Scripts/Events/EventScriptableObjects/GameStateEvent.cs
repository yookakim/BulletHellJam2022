using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Game Event/Game State Changed Event")]
public class GameStateEvent : ScriptableObject
{
	private List<GameStateChangedEventListener> listeners = new List<GameStateChangedEventListener>();

	public void Raise(GameManager.GameState newState)
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
		{
			listeners[i].OnEventRaised(newState);
		}
	}

	public void RegisterListener(GameStateChangedEventListener listener)
	{
		listeners.Add(listener);
	}

	public void UnregisterListener(GameStateChangedEventListener listener)
	{
		listeners.Remove(listener);
	}
}
