using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molechant : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemSpawnTransforms;
    [SerializeField] private ShopItemMasterListData shopItemMasterList;

    private int numberItemsToSell;
    private List<ShopItemData> itemsToSell;

    // Start is called before the first frame update
    void Start()
    {
        numberItemsToSell = itemSpawnTransforms.Count;
        itemsToSell = shopItemMasterList.GetRandomShopItems(itemSpawnTransforms.Count);

		for (int i = 0; i < itemsToSell.Count; i++)
		{
            itemsToSell[i].CreateShopInteractable(itemSpawnTransforms[i].transform.position);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
