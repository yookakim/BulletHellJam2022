using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;

public class WeaponController : MonoBehaviour
{
	public Vector2 CurrentWeaponTarget { get; set; }
	public Transform LiveWeaponTargetTransform { get; set; }
	public bool CanUse { get; private set; }
	public GameObject WeaponOwner { get => weaponOwner; }

    [SerializeField] private WeaponData weaponData;
	[SerializeField] private GameObject weaponOwner;

    private float timeLastUsed;

	private void Awake()
	{
		timeLastUsed = Time.time;
	}

	private void Update()
	{
		if (Time.time - timeLastUsed >= 1 / weaponData.useRate)
		{
			CanUse = true;
		}
	}

	public void AttemptUse()
	{
		if (Time.time - timeLastUsed >= 1 / weaponData.useRate)
		{
			Use();
			timeLastUsed = Time.time;
		}
	}

	private void Use()
	{
		CanUse = false;
		CustomEvent.Trigger(gameObject, "WeaponUsed");
		weaponData.UseWeapon(this);
	}

	public void CancelTweens()
	{
		LeanTween.cancel(gameObject);
	}
}
