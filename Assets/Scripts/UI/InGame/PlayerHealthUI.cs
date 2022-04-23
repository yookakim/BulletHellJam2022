using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
	[SerializeField] private GameObject healthSlotPrefab;
	[SerializeField] private GameObject healthSlotContainer;
	[SerializeField] private Sprite fullSlotSprite;
	[SerializeField] private Sprite halfSlotSprite;
	[SerializeField] private Sprite emptySlotSprite;

	private List<GameObject> healthSlots;
    private int currentMaxHealth;

	private void Awake()
	{
		healthSlots = new List<GameObject>();
	}

	public void OnPlayerHealthChanged(Health playerHealth)
	{
		if (playerHealth.MaxHealth != currentMaxHealth)
		{
			ChangeMaxHealth(playerHealth);
		}

		int fullSlots = playerHealth.CurrentHealth / 2; // 5/8 HP would be 2 full slots
		Debug.Log("full slots: " + fullSlots);
		bool hasHalfHeart = playerHealth.CurrentHealth % 2 == 1;

		for (int i = 0; i < healthSlots.Count; i++)
		{
			PlayerHealthSlot healthSlotImage = healthSlots[i].GetComponent<PlayerHealthSlot>();
			if (i < fullSlots)
			{
				healthSlotImage.HeartImage.sprite = fullSlotSprite;
			}
			else if (i == fullSlots && hasHalfHeart)
			{
				healthSlotImage.HeartImage.sprite = halfSlotSprite;
			}
			else
			{
				healthSlotImage.HeartImage.sprite = emptySlotSprite;
			}
		}
	}

	private void ChangeMaxHealth(Health health)
	{
		int numberOfHearts = health.MaxHealth / 2;

		if (health.MaxHealth % 2 != 0)
		{
			Debug.LogError("Max player health should always be in multiples of 2!");
		}

		if (health.MaxHealth > currentMaxHealth)
		{
			int slotsToAdd = (health.MaxHealth - currentMaxHealth) / 2;

			for (int i = 0; i < slotsToAdd; i++)
			{
				GameObject newHealthSlot = Instantiate(healthSlotPrefab, healthSlotContainer.transform);
				healthSlots.Add(newHealthSlot);
			}
		}
		else
		{
			int slotsToRemove = (currentMaxHealth - health.MaxHealth) / 2;
			for (int i = 0; i < slotsToRemove; i++)
			{
				GameObject healthSlot = healthSlots[healthSlots.Count - 1];
				healthSlots.RemoveAt(healthSlots.Count - 1);
				Destroy(healthSlot);
			}
		}

		currentMaxHealth = health.MaxHealth;
	}
}
