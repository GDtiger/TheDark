using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Town
{
	public class Town_InventoryUI : MonoBehaviour
	{
		//public ItemToolTipUI tooltip;
		//public GameObject inventoryPanel;


		public List<Town_Slot> slots;
		public Transform slotHolder;
		public ObjectPoolSCO objectPool;


		public void Initiate()
		{
		}

		public void ResetNumIndex()
		{
			for (int i = 0; i < slots.Count; i++)
			{
				slots[i].slotNum = i;
			}
		}

		public Town_Slot AddSlot(ItemData itemData)
		{
			var slotItem = (Town_Slot)objectPool.RequestObject(PrefabID.ItemSlot);
			slotItem.transform.SetParent(slotHolder);
			slotItem.slotNum = slots.Count;
			slotItem.Initiate();
			slotItem.SetItem(itemData);
			slots.Add(slotItem);
			return slotItem;
		}

		public void RemoveSlot(int index)
		{
			objectPool.ReturnObject(slots[index]);
			slots.RemoveAt(index);
			ResetNumIndex();
		}

		


		public void UpdateSlot(int i)
		{
			slots[i].SetItem();
		}
		public void UpdateSlot()
		{
			for (int i = 0; i < slots.Count; i++)
			{
				slots[i].SetItem();
			}
		}
		//public void OnClickAddSlot()
		//{
		//	inventory.SlotCount++;
		//}
	}

}

