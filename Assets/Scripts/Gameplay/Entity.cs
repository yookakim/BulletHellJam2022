using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private Hitbox entityHitbox;
	[SerializeField] private Health health;
	[SerializeField] private Movement movement;
	[SerializeField] private EntityStatusEffects entityStatusEffects;
	// private EntityStatusEffects entityStatusEffects;

	private void Awake()
	{
		entityHitbox.hitboxTriggeredEvent += OnHit;
	}

	protected virtual void OnHit(GameObject gameObjectHitBy)
	{

	}
}
