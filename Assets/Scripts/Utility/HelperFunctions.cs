using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
	public static Vector2 RotateVectorRad(Vector2 inputVector, float angleInRadians)
	{
		Vector2 newVector;
		newVector = new Vector2(
			inputVector.x * Mathf.Cos(angleInRadians) - inputVector.y * Mathf.Sin(angleInRadians),
			inputVector.x * Mathf.Sin(angleInRadians) + inputVector.y * Mathf.Cos(angleInRadians)
		);

		return newVector;
	}
}