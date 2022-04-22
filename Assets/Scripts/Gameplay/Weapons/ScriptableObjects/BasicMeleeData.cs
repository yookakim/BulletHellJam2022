using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Basic Melee")]
public class BasicMeleeData : MeleeData
{
    public override void UseMelee(MeleeController meleeController)
	{
		Vector2 targetDirection =
			(meleeController.CurrentWeaponTarget - (Vector2)meleeController.transform.position).normalized;

		// spawn the melee hitbox prefab


		GameObject meleeObject = Instantiate(
			meleeAttackPrefab,
			meleeController.transform.position,
			Quaternion.identity
		);

		meleeObject.transform.up = targetDirection;

		MeleeAttack meleeAttack = meleeObject.GetComponent<MeleeAttack>();

		if (meleeAttack != null)
		{
			meleeAttack.InitializeMeleeData(this);
			meleeAttack.Owner = meleeController.MeleeOwner;
		}
	}
}
