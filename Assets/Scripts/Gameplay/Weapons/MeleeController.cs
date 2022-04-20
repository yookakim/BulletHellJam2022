using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
	public Vector2 CurrentWeaponTarget { get; set; }

	private float timeLastUsed;

	public bool CanUse { get; private set; }

	[SerializeField] private MeleeData meleeData;

	private void Awake()
	{
		timeLastUsed = Time.time;
	}

	private void Update()
	{
		if (Time.time - timeLastUsed >= 1 / meleeData.useRate)
		{
			CanUse = true;
		}
	}

	public void AttemptUse()
	{
		if (Time.time - timeLastUsed >= 1 / meleeData.useRate)
		{
			Use();
			timeLastUsed = Time.time;
		}
	}

	private void Use()
	{
		CanUse = false;
		// CustomEvent.Trigger(gameObject, "WeaponUsed");
		meleeData.UseMelee(this);
	}
}
