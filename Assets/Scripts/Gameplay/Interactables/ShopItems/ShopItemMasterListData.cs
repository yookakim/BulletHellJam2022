using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Shop Item Master List")]
public class ShopItemMasterListData : ScriptableObject
{
    public List<ShopItemData> masterList;

    public List<ShopItemData> GetRandomShopItems(int numberItems)
	{
		List<ShopItemData> shopItemsToReturn = new List<ShopItemData>();

		// int[] indicesAlreadyUsed = new int[numberItems];
		List<int> indicesAlreadyUsed = new List<int>();

		if (numberItems >= masterList.Count)
		{
			int startIndex = Random.Range(0, masterList.Count);

			for (int i = 0; i < numberItems; i++)
			{
				shopItemsToReturn.Add(masterList[startIndex % masterList.Count]);
				startIndex++;
			}
		}
		else
		{
			for (int i = 0; i < numberItems; i++)
			{
				int randomIndex = Random.Range(0, masterList.Count);

				while (indicesAlreadyUsed.Contains(randomIndex))
				{
					randomIndex = Random.Range(0, masterList.Count);
				}

				shopItemsToReturn.Add(masterList[randomIndex]);

				indicesAlreadyUsed.Add(randomIndex);
			}
		}

		return shopItemsToReturn;
	}
}
