using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private PlayerInputController inputController;
	private Movement movement;
	private PlayerBombLauncher bombLauncher;
	private PlayerWeaponController weaponController;

	private void Awake()
	{
		inputController = new PlayerInputController(Camera.main);
		movement = GetComponent<Movement>();
		bombLauncher = GetComponent<PlayerBombLauncher>();
		weaponController = GetComponent<PlayerWeaponController>();
	}

	private void Update()
	{
		inputController.ReadInput();
		movement.Move(inputController.CurrentMoveInput);

		if (inputController.BombInputPressed)
		{
			bombLauncher.LaunchBomb(inputController.CurrentWorldCursorPoint);
			Debug.Log("bomb input pressed");
		}
	}
}
