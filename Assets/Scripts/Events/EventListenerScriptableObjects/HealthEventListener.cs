using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthEventListener : MonoBehaviour
{
	[SerializeField] private HealthEvent healthEvent;
	[SerializeField] private UnityEvent<Health> response;

	private void OnEnable()
	{
		healthEvent.RegisterListener(this);
	}

	private void OnDisable()
	{
		healthEvent.UnregisterListener(this);
	}

	public virtual void OnEventRaised(Health health)
	{
		response?.Invoke(health);
	}
}
