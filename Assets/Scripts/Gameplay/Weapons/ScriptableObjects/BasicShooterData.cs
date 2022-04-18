using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Basic Shooter")]
public class BasicShooterData : WeaponData
{
	public float launchForce;

	public override void UseWeapon(PlayerWeaponController weaponController)
	{
		Vector2 targetDirection = 
			(weaponController.CurrentWeaponTarget - (Vector2)weaponController.transform.position).normalized;

		GameObject projectileObject = Instantiate(
			projectilePrefab,
			weaponController.transform.position,
			Quaternion.identity
		);

		projectileObject.GetComponent<Rigidbody2D>().AddForce(targetDirection * launchForce, ForceMode2D.Impulse);
	}
}
