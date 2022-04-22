using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Basic Shooter")]
public class BasicShooterData : WeaponData
{
	public float launchForce;
	public float clusterAccuracyDeviation;
	public float individualAccuracyDeviation;
	public bool directionFacesTarget;
	public int clusterAmount;
	public float clusterSpreadAngleInDegrees;
	public float spawnDistanceFromWeapon;

	public override void UseWeapon(WeaponController weaponController)
	{
		Vector2 targetDirection = 
			(weaponController.CurrentWeaponTarget - (Vector2)weaponController.transform.position).normalized;

		float angleInBetween = 0;
		if (clusterAmount > 1)
		{
			angleInBetween = clusterSpreadAngleInDegrees / (clusterAmount - 1);
		}

		Vector2 currentShotVector =
			HelperFunctions.RotateVectorRad(targetDirection, -(clusterSpreadAngleInDegrees * Mathf.Deg2Rad) / 2);

		float clusterAngleDeviation = Random.Range(-clusterAccuracyDeviation / 2, clusterAccuracyDeviation / 2);

		for (int i = 0; i < clusterAmount; i++)
		{
			float individualAngleDeviation = Random.Range(-individualAccuracyDeviation / 2, individualAccuracyDeviation / 2);

			GameObject projectileObject = Instantiate(
				projectilePrefab,
				(Vector2)weaponController.transform.position + targetDirection * spawnDistanceFromWeapon,
				Quaternion.identity
			);

			Vector2 directionProcessedForAccuracy =
				HelperFunctions.RotateVectorRad(currentShotVector, clusterAngleDeviation * Mathf.Deg2Rad + individualAngleDeviation * Mathf.Deg2Rad);

			if (directionFacesTarget)
			{
				projectileObject.transform.up = directionProcessedForAccuracy;
			}

			projectileObject.GetComponent<Rigidbody2D>()
				.AddForce(directionProcessedForAccuracy * launchForce, ForceMode2D.Impulse);

			currentShotVector = HelperFunctions.RotateVectorRad(currentShotVector, angleInBetween * Mathf.Deg2Rad);

			Projectile projectile = projectileObject.GetComponent<Projectile>();

			projectile.Direction = directionProcessedForAccuracy;

			if (projectile != null)
			{
				projectile.Owner = weaponController.WeaponOwner;
			}
		}
	}
}
