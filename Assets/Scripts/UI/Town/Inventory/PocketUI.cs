using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Town
{
	public class PocketUI : MonoBehaviour
	{
		public Town_PocketSlot[] slots;
		public Transform slotHolder;
		public void Initiate()
		{
			slots = slotHolder.GetComponentsInChildren<Town_PocketSlot>();
			for (int i = 0; i < slots.Length; i++)
			{
				slots[i].slotNum = i;
				slots[i].Initiate();
				slots[i].SetItem();
			}
		}

		public void UpdateSlot()
		{
			for (int i = 0; i < slots.Length; i++)
			{
				slots[i].SetItem();
			}
		}
		internal void UpdateSlot(int i)
		{
			slots[i].SetItem();
		}

		public void AddItem(int i, ItemData item)
		{
			slots[i].SetItem(item);

			Debug.Log(slots[i].slotNum);
		}

		public void RemoveItem(int index)
		{
			slots[index].EmptySlot();
		}
	}

}

