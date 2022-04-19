using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Basic Shooter")]
public class BasicShooterData : WeaponData
{
	public float launchForce;
	public float accuracyDeviationInAngles;
	public bool directionFacesTarget;

	public override void UseWeapon(WeaponController weaponController)
	{
		Vector2 targetDirection = 
			(weaponController.CurrentWeaponTarget - (Vector2)weaponController.transform.position).normalized;

		GameObject projectileObject = Instantiate(
			projectilePrefab,
			weaponController.transform.position,
			Quaternion.identity
		);

		float angleDeviation = Random.Range(-accuracyDeviationInAngles / 2, accuracyDeviationInAngles / 2);

		Vector2 directionProcessedForAccuracy = HelperFunctions.RotateVectorRad(targetDirection, angleDeviation * Mathf.Deg2Rad);

		if (directionFacesTarget)
		{
			projectileObject.transform.up = directionProcessedForAccuracy;
		}

		projectileObject.GetComponent<Rigidbody2D>().AddForce(directionProcessedForAccuracy * launchForce, ForceMode2D.Impulse);
	}
}
