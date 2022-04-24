using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBombLauncher : MonoBehaviour
{
	public Queue<BombCharge> BombCharges { get => bombCharges; }
	public float CurrentChargeRate { get => currentChargeRate; }
	public float CurrentChargeAmount { get => currentChargeAmount; }
	public BombData BombData { get => bombData; }
	public int MaxBombCharges { get => maxBombCharges; }
	public bool IsFull { get => bombCharges.Count == maxBombCharges; }

	[SerializeField] private float launchStrength;
	[SerializeField] private BombData bombData;
	[SerializeField] private int initialMaxBombCharges;
	[SerializeField] private int initialChargeRate;
	[SerializeField] private GameObjectEvent bombLauncherLoadedEvent;

	private float currentChargeRate;
	private Queue<BombCharge> bombCharges;
	private int maxBombCharges;
	private float currentChargeAmount;

	private void Awake()
	{
		bombCharges = new Queue<BombCharge>();
		maxBombCharges = initialMaxBombCharges;
		currentChargeRate = initialChargeRate;

		bombLauncherLoadedEvent.Raise(gameObject);
	}

	private void Update()
	{
		if (bombCharges.Count < maxBombCharges)
		{

			currentChargeAmount += Time.deltaTime * currentChargeRate;
			if (currentChargeAmount >= bombData.bombChargeTime)
			{
				bombCharges.Enqueue(new BombCharge());
				currentChargeAmount = 0;
				bombLauncherLoadedEvent.Raise(gameObject);
			}
		}
	}

	/// <summary>
	/// Use the data for the current BombLauncherData and fire a bomb
	/// </summary>
	public void LaunchBomb(Vector2 targetPosition)
	{
		if (bombCharges.Count > 0)
		{
			bombCharges.Dequeue();

			Vector2 launchDirection = (targetPosition - (Vector2)transform.position).normalized;
			GameObject instantiatedBomb = Instantiate(bombData.bombPrefab, transform.position, Quaternion.identity);

			instantiatedBomb.GetComponent<Rigidbody2D>().AddForce(launchDirection * launchStrength, ForceMode2D.Impulse);

			bombLauncherLoadedEvent.Raise(gameObject);
		}
	}

	// Data needed for bomb charges system:
	// Max bomb charges count, current 
}
