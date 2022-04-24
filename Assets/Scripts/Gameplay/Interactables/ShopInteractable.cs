using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractable : Interactable
{
	public ShopItemData ShopItemData { get => shopItemData; set => shopItemData = value; }
	[SerializeField] private ShopItemData shopItemData;

	protected override void Awake()
	{
		base.Awake();
/*
		interactableSprite.sprite = shopItemData.itemIcon;
		tooltipHeader = shopItemData.itemName;
		tooltipContent = shopItemData.itemDescription + " Price: " + shopItemData.itemPrice;*/
	}

	public void InitializeItemData(ShopItemData thisShopItem)
	{
		InteractableSprite.sprite = thisShopItem.itemIcon;
		TooltipHeader = thisShopItem.itemName;
		TooltipContent = thisShopItem.itemDescription + " Price: " + thisShopItem.itemPrice;
		ShopItemData = thisShopItem;
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
