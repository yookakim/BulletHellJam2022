using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private PlayerInputController inputController;
	private Movement movement;
	private Health health;
	private PlayerBombLauncher bombLauncher;
	private PlayerWeaponController weaponController;

	private void Awake()
	{
		inputController = new PlayerInputController(Camera.main);
		movement = GetComponent<Movement>();
		health = GetComponent<Health>();
		bombLauncher = GetComponent<PlayerBombLauncher>();
		weaponController = GetComponent<PlayerWeaponController>();
	}

	private void Update()
	{
		inputController.ReadInput();
		weaponController.CurrentWeaponTarget = inputController.CurrentWorldCursorPoint;
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
	}
}
