using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Town
{
	public class Town_InventoryManager : MonoBehaviour
	{
		#region instance
		static Town_InventoryManager instance = null;
		public static Town_InventoryManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<Town_InventoryManager>();
				}
				return instance;
			}
		}
		#endregion

		[TabGroup("Ref", "Auto")] public GameManager gm;

		//[TabGroup("Parameter", "Parameter")] public bool isLobby;
		[TabGroup("ParaSmeter", "Parameter")] public Town_Slot curSlot;
		[TabGroup("Parameter", "Parameter")] public int curIndex;
		[TabGroup("Parameter", "Parameter")] public bool isInventoryItem;
		[TabGroup("Parameter", "Parameter")] public bool isOpenInventory = false;
		[TabGroup("Parameter", "Parameter")] public bool isSellItem;

		[TabGroup("Ref", "Need")] public GameObject inventory;
		[TabGroup("Ref", "Need")] public Town_InventoryUI inventoryUI;
		[TabGroup("Ref", "Need")] public ItemToolTipUI tooltipUI;
		[TabGroup("Ref", "Need")] public PocketUI pocketUI;
		[TabGroup("Ref", "Need")] public ItemDataBase database;



		public void Initiate()
		{
			pocketUI.Initiate();
			inventoryUI.Initiate();
			tooltipUI.Initiate();
			//gameObject.SetActive(false);
			gm = GameManager.Instance;

		}

		public void OnClickOpenInventory()
		{
			//for (int i = 0; i < gm.inventory.Count; i++)
			//{
			//	inventoryUI.AddSlot(gm.inventory[i]);
			//}
			isOpenInventory = true;
			inventory.SetActive(isOpenInventory);
			gameObject.SetActive(isOpenInventory);
			inventoryUI.gameObject.SetActive(isOpenInventory);
			pocketUI.gameObject.SetActive(isOpenInventory);

			if (gm.isTutorial && TutorialController.Instance.tutorialNumber == 5)
			{
				Debug.Log($"{TutorialController.Instance.tutorialNumber}");
				TutorialController.Instance.OpenPopup();
				TutorialController.Instance.isContinueTutorial = true;
				TutorialController.Instance.Reward(1, "Skill");
			    TutorialController.Instance.InfinityArrow(0, DirEight.B);
			}

		}

		public void OnClickCloseInventory()
		{
			//for (int i = 0; i < gm.inventory.Count; i++)
			//{
			//	inventoryUI.slots[i].Return();
			//}
			//inventoryUI.slots.Clear();
			gm.soundManager.PlaySFXSound("Close");
			Town_ShopManager.Instance.isOpenShop = false;
			isOpenInventory = false;
			tooltipUI.gameObject.SetActive(false);
			Town_UIManager.Instance.CloseWindow();
			if (gm.isTutorial&& TutorialController.Instance.tutorialNumber == 7)
			{
				Debug.Log($"{TutorialController.Instance.tutorialNumber}");
				TutorialController.Instance.OpenPopup();
				TutorialController.Instance.UnArrow();
			}
		}
		public void OnclickEquipment()
		{
			if (isInventoryItem)
			{
				if (gm.inventory[curIndex].count <= 1)
				{
					AddItemToPocket(gm.inventory[curIndex]);
					RemoveItemInInventory(curIndex);
				}
				else
				{
					AddItemToPocket(gm.inventory[curIndex].DeepCopy());
					gm.inventory[curIndex].count--;
					inventoryUI.UpdateSlot(curIndex);
				}
			}
			else
			{

				if (gm.GetPocket().Count <= curIndex)
				{
					return;
				}
				var pocketSlot = gm.GetPocket()[curIndex];
				if (pocketSlot.count <= 1)
				{
					AddItemToInventory(pocketSlot);
					RemoveItemInPocket(curIndex);
					pocketUI.UpdateSlot(curIndex);
					gm.town_UIManager.statusManager.UpdateSlot(curIndex);
				}
				else
				{
					AddItemToInventory(pocketSlot.DeepCopy());
					pocketSlot.count--;
					pocketUI.UpdateSlot(curIndex);
					gm.town_UIManager.statusManager.UpdateSlot(curIndex);
				}


			}
		}



		public void ClickSlot(Town_Slot slot)
		{
			gm.soundManager.PlaySFXSound("Confirm");
			isInventoryItem = slot is Town_PocketSlot ? false : true;
			if (curSlot == null || curSlot != slot)
			{
				curSlot = slot;
				curIndex = slot.slotNum;
				tooltipUI.SetToolTip(curIndex);
				Debug.Log("1");
			}
			else
			{
				OnclickEquipment();
			}
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
					inventoryUI.UpdateSlot(i);
					return true;
				}
			}

			inventoryUI.AddSlot(item);
			gm.inventory.Add(item);
			return true;
		}

		public bool AddItemToInventory(ItemData item, int count)
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
					gm.inventory[i].count += count;
					inventoryUI.UpdateSlot(i);
					return true;
				}
			}

			gm.inventory.Add(item);
			gm.inventory[gm.inventory.Count - 1].count = count;
			inventoryUI.AddSlot(item);
			if (gm.isTutorial&& gm.gold == 0)
			{
				Debug.Log($"{TutorialController.Instance.tutorialNumber}");
				TutorialController.Instance.OpenPopup();
			}
			return true;
		}

		public bool AddItemToPocket(ItemData item)
		{
			Debug.Log("AddItemToPocket");
			var pocket = gm.GetPocket();

			for (int i = 0; i < pocket.Count; i++)
			{
				//if (pocket[i] == null || pocket[i].isnullItem)
				//{
				//	continue;
				//}
				//Debug.Log($"for is pocket.Count is pocket[i].ID == item.ID {i},{pocket[i].ID},{item.ID}");
				if (pocket[i].ID == item.ID)
				{
					//수량 추가
					pocket[i].count += item.count;
					pocketUI.UpdateSlot(i);
					gm.town_UIManager.statusManager.UpdateSlot(i);
					return true;
				}
			}
			int maxPocketSize = GameManager.Instance.maxQuickSlotSkillSize;
			if (pocket.Count < maxPocketSize)
			{
				pocket.Add(item);
				pocketUI.AddItem(pocket.Count - 1, item);
				gm.town_UIManager.statusManager.AddItem(pocket.Count - 1, item);
				//for (int i = 0; i < maxPocketSize; i++)
				//            {
				//                //Debug.Log($"for is null pocket.Count is {pocket[i].isnullItem} {i}");
				//                if (pocket[i] == null || pocket[i].isnullItem)
				//                {
				//                    //아이템 추가
				//                    pocket[i] = item;
				//                    return true;
				//                }
				//            }
				return true;
			}
			else
			{
				return false;
			}
		}



		public bool RemoveItemInInventory(int index)
		{
			inventoryUI.RemoveSlot(index);
			gm.inventory.RemoveAt(index);
			if (gm.isTutorial)
			{
				TutorialController.Instance.UnArrow();
				TutorialController.Instance.InfinityArrow(1, DirEight.B);
			}
			return true;
		}
		public bool RemoveItemInInventory(int index, int count)
		{
			if (gm.inventory[index].count > count)
			{
				gm.inventory[index].count -= count;
				return true;
			}
			else if (gm.inventory[index].count < count)
			{
				return false;
			}
			else
			{
				return RemoveItemInInventory(index);
			}
		}

		public void TestInputItem()
		{

			Debug.Log("Item ");

			AddItemToInventory(database.GetItemFromDataBase(0));
			AddItemToInventory(database.GetItemFromDataBase(1));
			AddItemToInventory(database.GetItemFromDataBase(2));
			AddItemToInventory(database.GetItemFromDataBase(3));
			AddItemToInventory(database.GetItemFromDataBase(4));

		}
		public bool RemoveItemInPocket(int index)
		{
			gm.GetPocket().RemoveAt(index);
			pocketUI.UpdateSlot();
			gm.town_UIManager.statusManager.UpdateSlot();
			return true;
		}

		public bool RemoveItemInPocket(int index, int count)
		{
			var pocketSlot = gm.GetPocket()[index];
			if (pocketSlot.count > count)
			{
				pocketSlot.count -= count;
				return true;
			}
			else if (pocketSlot.count < count)
			{
				return false;
			}
			else
			{
				return RemoveItemInPocket(index);
			}
		}

	}

}
