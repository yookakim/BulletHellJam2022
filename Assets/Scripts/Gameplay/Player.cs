using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private PlayerInputController inputController;
	private Movement movement;

	private void Awake()
	{
		inputController = new PlayerInputController();
		movement = GetComponent<Movement>();
	}

	private void Update()
	{
		inputController.ReadInput();
		movement.Move(inputController.CurrentMoveInput);
	}
}
