using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Town
{
    public class Town_ShopTooltip : MonoBehaviour
    {
        public TextMeshProUGUI itemNameTxt;
        public TextMeshProUGUI itemDescriptionTxt;
        public TextMeshProUGUI itemDetailDescriptionTxt;
        public TextMeshProUGUI itemDetailDescriptionTxt2;
        public TextMeshProUGUI itemPriceTxt;
        public TextMeshProUGUI itemSellAndBuyTxt;
        public TextMeshProUGUI itemtotalCountTxt;
        public TextMeshProUGUI itemBuyCountTxt;
        public Image itemImage;

        public Town_ShopManager shopManager;
        public GameManager gm;

        [SerializeField]private int tempIndex;
        [SerializeField]private int purchaseQuantity;
        [SerializeField]private ItemData tempData;

		public void Initiate()
		{
            if (shopManager == null) shopManager = Town_ShopManager.Instance;
            if (gm == null) gm = GameManager.Instance;
			if (gm.isTutorial) purchaseQuantity = 1;
			//Debug.Log(inventoryManager.isSellItem);
		}

		public void SetTooltip(int index)
		{
            if (!Town_InventoryManager.Instance.isSellItem)
			{
                tempData = shopManager.shopItemData[index];
                itemNameTxt.text = $"[{tempData.name}]";
				itemDescriptionTxt.text =
					$"{tempData.description}";
				itemDetailDescriptionTxt.text =
					$"범위 : {tempData.effectRangeTile} " +
					"\n" +
					$"쿨타임 : {tempData.DelayTurn} 턴";
				itemDetailDescriptionTxt2.text =
					$"가격 : {tempData.price} 골드" +
					"\n" +
					$"포켓 최대 : {tempData.pocketMaxCount} 개" +
					"\n" +
					$"사거리 : {tempData.distanceTile}";
				purchaseQuantity = 0;
				itemPriceTxt.text = $"{tempData.price*purchaseQuantity}";
                itemSellAndBuyTxt.text = "구매";
                tempIndex = index;
				UpdateInventory(tempData);
                itemImage.sprite = gm.GetImage(tempData.imageID);
				itemImage.color = Color.black;
				gameObject.SetActive(true);
                itemBuyCountTxt.text = $"{purchaseQuantity} 개";
				TutorialController.Instance.UnArrow();
            }
            else
			{
				ItemData item = gm.inventory[index];
				itemNameTxt.text = item.name;
				itemDescriptionTxt.text = $"{item.effectRangeTile}";
				itemPriceTxt.text = $"{item.price}";
				itemSellAndBuyTxt.text = "판매";
				tempIndex = index;
				Debug.Log($"{item.imageID},{index}");
				itemImage.sprite = gm.GetImage(item.imageID);
				gameObject.SetActive(true);
			}
		}
        public void CloseTooltip()
		{
            gameObject.SetActive(false);
		}
        public void OnClickPlus()
		{
			gm.soundManager.PlaySFXSound("Confirm");
			if (purchaseQuantity < 100)
			{
				purchaseQuantity++;
				itemBuyCountTxt.text = $"{purchaseQuantity} 개";
				itemPriceTxt.text = $"{tempData.price * purchaseQuantity}";
			}
		}
		public void OnClickMinus()
        {
			gm.soundManager.PlaySFXSound("Confirm");
			if (purchaseQuantity > 0)
			{
				purchaseQuantity--;
				itemBuyCountTxt.text = $"{purchaseQuantity} 개";
				itemPriceTxt.text = $"{tempData.price * purchaseQuantity}";
			}
		}
		public void OnClickBuyItem()
		{
			if (gm.gold > shopManager.curItemData.price)
			{
				gm.soundManager.PlaySFXSound("Confirm");
				Town_InventoryManager.Instance.AddItemToInventory
					(shopManager.shopItemData[tempIndex].DeepCopy(), purchaseQuantity);
				gm.gold -= (shopManager.shopItemData[tempIndex].price * purchaseQuantity);
				itemBuyCountTxt.text = $"{purchaseQuantity} 개";
				UpdateInventory(shopManager.shopItemData[tempIndex]);
				UpdateIndex();
				if (gm.isTutorial && TutorialController.Instance.tutorialNumber == 3&&gm.gold == 0)
				{
					Debug.Log($"{TutorialController.Instance.tutorialNumber}");
					TutorialController.Instance.tutorialDescriptionTitle.text = "퀘스트 보상";
					TutorialController.Instance.OpenPopup();
					TutorialController.Instance.Reward(0, "Item");
				}
			}
			else
			{
				Debug.Log("골드가 부족합니다.");
			}
		}

		private void UpdateInventory(ItemData _itemData)
		{
			for (int i = 0; i < gm.inventory.Count; i++)
			{
				if (_itemData.ID == gm.inventory[i].ID)
				{
					itemtotalCountTxt.text = $"보유수량 {gm.inventory[i].count}개";
					break;
				}
				else
				{
					itemtotalCountTxt.text = $"보유수량 0개";
				}
			}
		}
		private void UpdateIndex()
		{
			purchaseQuantity = 0;
			itemBuyCountTxt.text = $"{purchaseQuantity} 개";
			itemPriceTxt.text = $"{tempData.price * purchaseQuantity}";
		}
	}
}