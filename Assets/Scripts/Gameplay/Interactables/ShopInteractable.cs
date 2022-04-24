using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractable : Interactable
{
	[SerializeField] private ShopItemData shopItemData;

	protected override void Awake()
	{
		base.Awake();

		interactableSprite.sprite = shopItemData.itemIcon;
		tooltipHeader = shopItemData.itemName;
		tooltipContent = shopItemData.itemDescription + " Price: " + shopItemData.itemPrice;
	}

	public override void OnInteract(Player player)
	{
		base.OnInteract(player);

		// if player can afford it, give item to player

		if (player.CurrentCoinAmount >= shopItemData.itemPrice)
		{
			player.CurrentCoinAmount -= shopItemData.itemPrice;
			player.OnShopItemPurchased(shopItemData);
			player.OnInteractableTriggerExit(this);
			Destroy(gameObject);
		}
		else
		{
			Debug.Log("Player can't afford this item");
			// maybe make the mole shake his head or something
			// "I THINK YOUR WALLET HAS TO RELOAD"
		}
	}
}
