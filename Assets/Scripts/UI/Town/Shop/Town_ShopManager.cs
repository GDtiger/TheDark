using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Town
{
    public class Town_ShopManager : MonoBehaviour
    {
		#region instance
		static Town_ShopManager instance = null;
		public static Town_ShopManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<Town_ShopManager>();
				}
				return instance;
			}
		}
		#endregion

		public GameManager gm;

		public Town_ShopUI shopUI;
        public Town_ShopTooltip ShopTooltip;
		public Town_Slot curSlot;
		public ItemData curItemData;
		public List<ItemData> shopItemData;
		public int randomItemIndex;
		public int curIndex;
		public bool isShopItem;
		public bool isOpenShop = false;
		public ItemDataBase database;
		public GameObject shop;
		public ItemType type = ItemType.Null;

		public void Initiate()
		{
			shopUI.Initiate();
			ShopTooltip.Initiate();
			gameObject.SetActive(false);
			shop.SetActive(false);
			gm = GameManager.Instance;
			UpdateFilter();
		}

		public bool AddItemToInventory(ItemData item)
		{
			for (int i = 0; i < gm.inventory.Count; i++)
			{
				if (gm.inventory[i] == null || gm.inventory[i].isnullItem)
				{
					continue;
				}

				if (gm.inventory[i].ID == item.ID /*&& inventory[i].count > 1*/)
				{
					//수량 추가
					gm.inventory[i].count += item.count;
					Town_InventoryManager.Instance.inventoryUI.UpdateSlot(i);
					return true;
				}
			}

			Town_InventoryManager.Instance.inventoryUI.AddSlot(item);
			gm.inventory.Add(item);
			return true;
		}

		public void OnClickOpenShop()
		{
			isOpenShop = true;
			shop.SetActive(isOpenShop);
			gameObject.SetActive(isOpenShop);
			shopUI.gameObject.SetActive(isOpenShop);
			Town_UIManager.Instance.inventoryManager.inventoryUI.gameObject.SetActive(isOpenShop);
			if (gm.isTutorial&& TutorialController.Instance.tutorialNumber == 1)
			{
				TutorialController.Instance.tutorialDescriptionTitle.text = "퀘스트 보상";
				Debug.Log($"{TutorialController.Instance.tutorialNumber}");
				TutorialController.Instance.OpenPopup();
				TutorialController.Instance.Reward(30,"Gold");
			}
		}
		public void OnClickCloseShop()
		{
			gm.soundManager.PlaySFXSound("Close");
			isOpenShop = false;
			shop.SetActive(isOpenShop);
			gameObject.SetActive(isOpenShop);
			shopUI.gameObject.SetActive(isOpenShop);
			ShopTooltip.gameObject.SetActive(isOpenShop);
			Town_UIManager.Instance.CloseWindow();
			if (gm.isTutorial && TutorialController.Instance.tutorialNumber == 4)
			{
				Debug.Log($"{TutorialController.Instance.tutorialNumber}");
				TutorialController.Instance.OpenPopup();
				TutorialController.Instance.UnArrow();
			}
		}

		public void ClickSlot(Town_ShopSlot slot)
		{
			gm.soundManager.PlaySFXSound("Confirm");
			if (!ShopTooltip.gameObject.activeSelf)
			{
				ShopTooltip.gameObject.SetActive(true);
			}
			curIndex = slot.slotNum;
			Town_InventoryManager.Instance.isSellItem = false;
			ShopTooltip.SetTooltip(curIndex);
		}

		public bool SetShopSlot(ItemData item)
		{
			shopUI.AddSlot(item);
			shopItemData.Add(item);
			return true;
		}
		public bool RemoveItemInShop(int i)
		{
			shopUI.RemoveSlot(i);
			shopItemData.RemoveAt(i);
			return true;
		}
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.A))
			{
				var temp = database.ItemCount;
				Debug.Log($"{temp}");
			}
		}
		[Button]
		public void OnClickFilterHeal()
		{
			type = ItemType.Heal;
			UpdateFilter();
		}
		[Button]
		public void OnClickFilterAttraction()
		{
			type = ItemType.Attraction;
			UpdateFilter();
		}
		[Button]
		public void OnClickFilterTrap()
		{
			type = ItemType.Trap;
			UpdateFilter();
		}
		[Button]
		public void OnClickFilterBuff()
		{
			type = ItemType.Buff;
			UpdateFilter();
		}
		[Button]
		public void OnClickFilterDebuff()
		{
			type = ItemType.Debuff;
			UpdateFilter();
		}
		public void UpdateFilter()
		{
			gm.soundManager.PlaySFXSound("Confirm");
			shopUI.RemoveSlotAll();
			shopItemData.Clear();
			ShopTooltip.gameObject.SetActive(false);
			for (int i = 0; i < database.ItemCount; i++)
			{
				if (database.GetItemFromDataBase(i).itemType == (database.GetItemFromDataBase(i).itemType | type))
				{
					SetShopSlot(database.GetItemFromDataBase(i));
				}
			}
		}
	}
}
