using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController
{
	public Vector2 CurrentMoveInput { get; private set; }
	public Vector2 CurrentWorldCursorPoint { get; private set; }
	public bool BombInputPressed { get; private set; }
	public bool CursorInputPressed { get; private set; }
	public bool MeleeInputPressed { get; private set; }
	public bool MeleeInputHeld { get; private set; }

	private Camera cam;

	public PlayerInputController(Camera camera)
	{
		cam = camera;
	}

	public void ReadInput()
	{
		CurrentMoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

		BombInputPressed = Input.GetKeyDown("space");

		CurrentWorldCursorPoint = cam.ScreenToWorldPoint(Input.mousePosition);
		CursorInputPressed = Input.GetMouseButtonDown(0);
		MeleeInputPressed = Input.GetMouseButtonDown(1);
		MeleeInputHeld = Input.GetMouseButton(1);
	}
}
