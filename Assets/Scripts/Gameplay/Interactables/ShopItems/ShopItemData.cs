using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(menuName = "Shop Item")]
public class ShopItemData : ScriptableObject
{
    public int itemPrice;
    [TextArea(15, 5)]
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;


}