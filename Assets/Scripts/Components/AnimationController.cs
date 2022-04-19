using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
	public enum LookMode
	{
		ByMovement,
		ByPlayerPosition
	}

	public LookMode lookMode = LookMode.ByMovement; // default to looking towards movement direction

	private PlayerScanRaycast playerScanner;
	private SpriteRenderer spriteRenderer;
	private Movement movement;
	private Animator animator;

	private void Awake()
	{
		playerScanner = GetComponent<PlayerScanRaycast>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		movement = GetComponent<Movement>();
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		HandleSpriteFlip();
		CheckEntityLookDirection();
	}

	/// <summary>
	/// Is sprite looking forward/to the side based on its current movement, or
	/// based on the location of the player position?
	/// </summary>
	/// <param name="lookMode"></param>
	public void SetLookMode(LookMode lookMode)
	{
		this.lookMode = lookMode;
	}

	public void CheckEntityLookDirection()
	{
		Vector2 lookDirection;

		if (lookMode == LookMode.ByPlayerPosition)
		{
			// chase, attack, etc
			lookDirection = playerScanner.VectorToPlayer.normalized;
		}
		else
		{
			// idle
			lookDirection = movement.CurrentMoveDirection;
		}

		if (Mathf.Abs(lookDirection.x) < Mathf.Abs(lookDirection.y))
		{
			animator.SetBool("LookingForward", true);
		}
		else
		{
			animator.SetBool("LookingForward", false);
		}
	}

	private void HandleSpriteFlip()
	{
		Vector2 lookDirection;
		if (lookMode == LookMode.ByPlayerPosition)
		{
			// chase, attack, etc
			lookDirection = playerScanner.VectorToPlayer.normalized;
		}
		else
		{
			// idle
			lookDirection = movement.CurrentMoveDirection;
		}

		if (lookDirection.x < 0 && spriteRenderer.flipX == false)
		{
			spriteRenderer.flipX = true;
		}
		else if (lookDirection.x > 0 && spriteRenderer.flipX == true)
		{
			spriteRenderer.flipX = false;
		}
	}
}
