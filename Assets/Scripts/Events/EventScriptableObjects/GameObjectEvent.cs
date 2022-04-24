using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Game Event/Game Object Event")]
public class GameObjectEvent : ScriptableObject
{
	private List<GameObjectEventListener> listeners = new List<GameObjectEventListener>();

	public void Raise(GameObject gameObject)
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
		{
			listeners[i].OnEventRaised(gameObject);
		}
	}

	public void RegisterListener(GameObjectEventListener listener)
	{
		listeners.Add(listener);
	}

	public void UnregisterListener(GameObjectEventListener listener)
	{
		listeners.Remove(listener);
	}
}
