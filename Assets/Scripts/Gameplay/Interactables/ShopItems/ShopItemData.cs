using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemData : ScriptableObject
{
    public int itemPrice;
    [TextArea(15, 5)]
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public GameObject shopInteractablePrefab;

    public GameObject CreateShopInteractable(Transform parent)
	{
        GameObject shopItemObject;
           
        shopItemObject = Instantiate(shopInteractablePrefab, parent);

        ShopInteractable shopInteractableComponent = shopItemObject.GetComponent<ShopInteractable>();

        shopInteractableComponent.InitializeItemData(this);

        return shopItemObject;
	}

    public GameObject CreateShopInteractable(Vector2 spawnPosition)
	{
        GameObject shopItemObject;

        shopItemObject = Instantiate(shopInteractablePrefab, spawnPosition, Quaternion.identity);

        ShopInteractable shopInteractableComponent = shopItemObject.GetComponent<ShopInteractable>();
        shopInteractableComponent.InitializeItemData(this);

        return shopItemObject;
    }
}