using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
	[SerializeField] private GameObject coinToSpawn;

    public void SpawnCoin()
	{
		Instantiate(coinToSpawn, transform.position, Quaternion.identity);
	}
}
