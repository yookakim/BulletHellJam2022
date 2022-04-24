using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBombChargesUI : MonoBehaviour
{
	[SerializeField] private GameObject bombSlotUIPrefab;

	private PlayerBombLauncher bombLauncher;
	private List<PlayerBombSlot> bombSlots;
	private int currentMaxSlots;

	private void Awake()
	{
		bombSlots = new List<PlayerBombSlot>();
	}

	private void Update()
	{
		if (bombLauncher == null)
		{
			// wait until it first gets assigned through event after Player gets loaded
			return;
		}
		UpdateBombChargeFill();
	}

	public void OnBombLauncherUpdated(GameObject bombLauncherObject)
	{
		bombLauncher = bombLauncherObject.GetComponent<PlayerBombLauncher>();

		if (currentMaxSlots != bombLauncher.MaxBombCharges)
		{
			UpdateNumberOfSlots();
		}

		foreach (PlayerBombSlot bombSlot in bombSlots)
		{
			bombSlot.RefreshSlotUI(bombLauncher);
		}
	}

	private void UpdateBombChargeFill()
	{
		int bombToFill = bombLauncher.BombCharges.Count;

		if (!bombLauncher.IsFull)
		{
			bombSlots[bombToFill].UpdateBombChargeFill(bombLauncher.CurrentChargeAmount, bombLauncher.BombData.bombChargeTime);
		}
	}

	private void UpdateNumberOfSlots()
	{
		// int newMaxSlots = bombLauncher.MaxBombCharges;

		if (bombLauncher.MaxBombCharges > currentMaxSlots)
		{
			int slotsToAdd = bombLauncher.MaxBombCharges - currentMaxSlots;

			for (int i = 0; i < slotsToAdd; i++)
			{
				GameObject newBombSlot = Instantiate(bombSlotUIPrefab, transform);

				PlayerBombSlot bombSlot = newBombSlot.GetComponent<PlayerBombSlot>();
				bombSlot.Index = bombSlots.Count;

				bombSlots.Add(bombSlot);
			}
		}
		else
		{
			int slotsToRemove = currentMaxSlots - bombLauncher.MaxBombCharges;
			for (int i = 0; i < slotsToRemove; i++)
			{
				GameObject bombSlot = bombSlots[bombSlots.Count - 1].gameObject;
				bombSlots.RemoveAt(bombSlots.Count - 1);
				Destroy(bombSlot);
			}
		}

		currentMaxSlots = bombLauncher.MaxBombCharges;
	}
}
