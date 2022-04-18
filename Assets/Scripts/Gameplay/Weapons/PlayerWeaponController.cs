using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
	public Vector2 CurrentWeaponTarget { get; set; }

    [SerializeField] private WeaponData weaponData;

    private float timeLastUsed;

	private void Awake()
	{
		timeLastUsed = Time.time;
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
		weaponData.UseWeapon(this);
	}
}
