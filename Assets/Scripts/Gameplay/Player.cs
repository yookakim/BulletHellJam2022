using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private TransformReference playerTransformReference;

	private PlayerInputController inputController;
	private Movement movement;
	private Health health;
	private PlayerBombLauncher bombLauncher;
	private WeaponController weaponController;
	private MeleeController meleeController;
	private Hitbox hitbox;

	private void Awake()
	{
		inputController = new PlayerInputController(Camera.main);
		movement = GetComponent<Movement>();
		health = GetComponent<Health>();
		bombLauncher = GetComponent<PlayerBombLauncher>();
		weaponController = GetComponent<WeaponController>();
		meleeController = GetComponent<MeleeController>();
		hitbox = GetComponentInChildren<Hitbox>();

		playerTransformReference.reference = transform;
	}

	private void Update()
	{
		inputController.ReadInput();
		weaponController.CurrentWeaponTarget = inputController.CurrentWorldCursorPoint;
		meleeController.CurrentWeaponTarget = inputController.CurrentWorldCursorPoint;
		movement.Move(inputController.CurrentMoveInput);

		if (inputController.BombInputPressed)
		{
			bombLauncher.LaunchBomb(inputController.CurrentWorldCursorPoint);
			Debug.Log("bomb input pressed");
		}

		if (inputController.CursorInputPressed)
		{
			weaponController.AttemptUse();
		}

		if (inputController.MeleeInputPressed)
		{
			// for detecting single click actions like double click abilities
		}

		if (inputController.MeleeInputHeld)
		{
			meleeController.AttemptUse();
		}
	}

	private void OnEnable()
	{
		hitbox.hitboxTriggeredEvent += OnHitboxTrigger;
		health.HealthZeroEvent += OnHealthZero;
	}

	private void OnDisable()
	{
		hitbox.hitboxTriggeredEvent -= OnHitboxTrigger;
		health.HealthZeroEvent -= OnHealthZero;
	}

	private void OnHealthZero(GameObject deadPlayerObject)
	{
		// put player into dead state later

		Time.timeScale = 0;
	}

	private void OnHitboxTrigger(GameObject objectHitBy)
	{
		DamageComponent gameObjectDamageComponent = objectHitBy.GetComponent<DamageComponent>();

		if (gameObjectDamageComponent != null)
		{
			if (gameObjectDamageComponent.DamageAlignment != hitbox.ownerAlignment)
			{
				health.DealDamage(gameObjectDamageComponent.DamageAmount);
			}
		}
	}
}
