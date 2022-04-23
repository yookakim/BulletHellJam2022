using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntEventListener : MonoBehaviour
{
	[SerializeField] private IntEvent intEvent;
	[SerializeField] private UnityEvent<int> response;

	private void OnEnable()
	{
		intEvent.RegisterListener(this);
	}

	private void OnDisable()
	{
		intEvent.UnregisterListener(this);
	}

	public virtual void OnEventRaised(int value)
	{
		response?.Invoke(value);
	}
}
