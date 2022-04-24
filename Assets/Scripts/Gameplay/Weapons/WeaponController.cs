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
	public WeaponData CurrentWeapon { get; private set; }
	public bool useInputHeld { get; set; }

    [SerializeField] private WeaponData startingWeaponData;
	[SerializeField] private GameObject weaponOwner;

    private float timeLastUsed;

	private void Awake()
	{
		timeLastUsed = Time.time;
		CurrentWeapon = startingWeaponData;
	}

	private void Update()
	{
		if (Time.time - timeLastUsed >= 1 / CurrentWeapon.useRate)
		{
			CanUse = true;
		}

		if (CurrentWeapon.allowsHoldFire && useInputHeld)
		{
			AttemptUse();
		}
	}

	public void SwapWeapon(WeaponData newWeapon)
	{
		CurrentWeapon = newWeapon;
	}

	public void AttemptUse()
	{
		if (Time.time - timeLastUsed >= 1 / CurrentWeapon.useRate)
		{
			Use();
			timeLastUsed = Time.time;
		}
	}

	private void Use()
	{
		CanUse = false;
		CustomEvent.Trigger(gameObject, "WeaponUsed");
		CurrentWeapon.UseWeapon(this);
	}

	public void CancelTweens()
	{
		LeanTween.cancel(gameObject);
	}
}
