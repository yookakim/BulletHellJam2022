using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Cluster Shooter")]
public class ClusterShooterData : WeaponData
{
	// shoots patterns of bullets based on starting angle, end angle, cluster time
	public float clusterTimeLength;
	// public float clusterFireRate; cluster fire rate based on weaponcontroller using this
	public float individualFireInterval;
	public float initialAngleOffset;
	public float angleOffset;
	public float maxAngle;
	public float launchForce;
	public float individualSpawnInaccuracy;
	public float individualTargetInaccuracy;
	public bool directionFacesTarget;
	public bool clusterFliesTowardsTarget;
	public bool clusterIgnoresWeaponTarget;
	public float spawnDistanceFromWeapon;
	public bool useSetAngles;

	public AnimationCurve aimCurveOverTime;


	public override void UseWeapon(WeaponController weaponController)
	{
		Vector2 targetDirection;
		// 
		if (clusterIgnoresWeaponTarget)
		{
			targetDirection = Vector2.up;
		}
		else
		{
			targetDirection =
				(weaponController.CurrentWeaponTarget - (Vector2)weaponController.transform.position).normalized;
		}



		// tween targetDirection to targetDirection + endAngle based on curve

		Vector2 offsetTargetDirection = HelperFunctions.RotateVectorRad(targetDirection, initialAngleOffset * Mathf.Deg2Rad);
		// Vector2 endDirection = HelperFunctions.RotateVectorRad(targetDirection, endAngle * Mathf.Deg2Rad);

		float currentTimeElapsedInterval = 0;
		int currentProjectileCount = 0;

		LeanTween.value(0, clusterTimeLength, clusterTimeLength).setEase(LeanTweenType.linear).setOnUpdate((float val) =>
		{
			currentTimeElapsedInterval += Time.deltaTime;
			if (currentTimeElapsedInterval >= individualFireInterval)
			{
				currentTimeElapsedInterval = 0;
				currentProjectileCount++;

				// evaluate current position in curve based on total time length of cluster pattern:
				float angleMultiplier = aimCurveOverTime.Evaluate(val / clusterTimeLength);
				float rotateFromStartAngle = 0;

				if (useSetAngles)
				{
					rotateFromStartAngle = currentProjectileCount * angleOffset;
				}
				else
				{
					rotateFromStartAngle = angleMultiplier * maxAngle;
				}

				float thisInaccuracy = Random.Range(-individualSpawnInaccuracy / 2, individualSpawnInaccuracy / 2);

				// based on position in curve, set aim to target + angleMultiplier / total angle rotation:
				Vector2 thisAngle = HelperFunctions.RotateVectorRad(offsetTargetDirection, rotateFromStartAngle * Mathf.Deg2Rad).normalized;
				thisAngle = HelperFunctions.RotateVectorRad(thisAngle, thisInaccuracy * Mathf.Deg2Rad);

				GameObject projectileObject = Instantiate(
					projectilePrefab,
					(Vector2)weaponController.transform.position + thisAngle * spawnDistanceFromWeapon,
					Quaternion.identity
				);

				if (directionFacesTarget)
				{
					projectileObject.transform.up = thisAngle;
				}

				if (clusterFliesTowardsTarget)
				{
					thisAngle = (weaponController.CurrentWeaponTarget - (Vector2)projectileObject.transform.position).normalized;
				}

				// projectileObject.GetComponent<Rigidbody2D>().AddForce(thisAngle * launchForce, ForceMode2D.Impulse);

				Projectile projectile = projectileObject.GetComponent<Projectile>();



				if (projectile != null)
				{
					projectile.LaunchForce = launchForce;
					projectile.LiveTarget = weaponController.LiveWeaponTargetTransform;
					projectile.Direction = thisAngle;
					projectile.InaccuracyForDelayedStart = individualTargetInaccuracy;
					projectile.Owner = weaponController.WeaponOwner;
				}
			}


		});

/*		LeanTween.value(0, maxAngle, clusterTimeLength).setOnUpdate((float val) =>
		{
			HelperFunctions.RotateVectorRad(offsetTargetDirection, val);
		});*/


	}

}
