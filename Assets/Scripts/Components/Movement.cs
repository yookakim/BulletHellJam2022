using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // perhaps editing speed in inspector is only temp until we starting loading in values from player data SO
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 currentMoveDirection;

	private void Awake()
	{
        rb = GetComponent<Rigidbody2D>();
    }

	public void Move(Vector2 moveDirection)
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        currentMoveDirection = moveDirection;
    }
}
