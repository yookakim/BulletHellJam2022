using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBombSlot : MonoBehaviour
{
	public int Index { get; set; }
	public Image SlotImage { get => slotImage; set => slotImage = value; }
	public Image BombImage { get => bombImage; set => bombImage = value; }

	[SerializeField] private Image slotImage;
	[SerializeField] private Image bombImage;

	public void UpdateBombChargeFill(float currentChargeAmount, float maxCharge)
	{
		bombImage.fillAmount = currentChargeAmount / maxCharge;
	}

	public void RefreshSlotUI(PlayerBombLauncher bombLauncher)
	{
		if (Index == bombLauncher.BombCharges.Count)
		{
			bombImage.fillAmount = bombLauncher.CurrentChargeAmount / bombLauncher.BombData.bombChargeTime;
		}
		else if (Index > bombLauncher.BombCharges.Count)
		{
			bombImage.fillAmount = 0;
		}
	}
}
