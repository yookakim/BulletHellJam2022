using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private UnityEvent response;

	private void OnEnable()
	{
		gameEvent.RegisterListener(this);
	}

	private void OnDisable()
	{
		gameEvent.UnregisterListener(this);
	}

	public virtual void OnEventRaised()
	{
        response?.Invoke();
	}
}
