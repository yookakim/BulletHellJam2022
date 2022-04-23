using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Game Event/Health Event")]
public class HealthEvent : ScriptableObject
{
	private List<HealthEventListener> listeners = new List<HealthEventListener>();

	public void Raise(Health health)
	{
		for (int i = listeners.Count - 1; i >= 0; i--)
		{
			listeners[i].OnEventRaised(health);
		}
	}

	public void RegisterListener(HealthEventListener listener)
	{
		listeners.Add(listener);
	}

	public void UnregisterListener(HealthEventListener listener)
	{
		listeners.Remove(listener);
	}
}
