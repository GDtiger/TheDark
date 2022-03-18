using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



	public class InventoryManager : MonoBehaviour
	{
		#region instance
		static InventoryManager instance = null;
		public static InventoryManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<InventoryManager>();
				}
				return instance;
			}
		}
		#endregion

		[TabGroup("Ref", "Auto")] public GameManager gm;
		[TabGroup("Ref", "Need")] public ItemDataBase database;



		public void Initiate()
		{
			gm = GameManager.Instance;

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
					return true;
				}
			}
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
					return true;
				}
			}
			gm.inventory.Add(item);
			gm.inventory[gm.inventory.Count - 1].count = count;
			return true;
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
	}


