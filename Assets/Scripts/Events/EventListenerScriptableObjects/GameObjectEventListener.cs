using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectEventListener : MonoBehaviour
{
	[SerializeField] private GameObjectEvent intEvent;
	[SerializeField] private UnityEvent<GameObject> response;

	private void OnEnable()
	{
		intEvent.RegisterListener(this);
	}

	private void OnDisable()
	{
		intEvent.UnregisterListener(this);
	}

	public virtual void OnEventRaised(GameObject gameObject)
	{
		response?.Invoke(gameObject);
	}
}
