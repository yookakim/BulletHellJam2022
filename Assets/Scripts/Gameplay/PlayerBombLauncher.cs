using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBombLauncher : MonoBehaviour
{
	[SerializeField] private float launchStrength;
	[SerializeField] private GameObject bombPrefab;

	/// <summary>
	/// Use the data for the current BombLauncherData and fire a bomb
	/// </summary>
    public void LaunchBomb(Vector2 targetPosition)
	{
		Vector2 launchDirection = (targetPosition - (Vector2)transform.position).normalized;
		GameObject instantiatedBomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);

		instantiatedBomb.GetComponent<Rigidbody2D>().AddForce(launchDirection * launchStrength, ForceMode2D.Impulse);
	}
}
