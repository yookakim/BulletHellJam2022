using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController
{
	public Vector2 CurrentMoveInput { get; private set; }

	public void ReadInput()
	{
		CurrentMoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
	}
}
