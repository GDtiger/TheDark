using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Town
{
	public class Town_PocketSlot : Town_Slot, IPointerClickHandler
	{
		public override void OnPointerClick(PointerEventData eventData)
		{
			Debug.Log("QuickSlot Click");
			inventoryManager.ClickSlot(this);
			//if (inventoryManager.isLobby)
			//{
			//	inventoryManager.ClickSlot(this);
			//	//Debug.Log($"QuickSlot Click {Town_InventoryController.Instance.isLobby}");
			//}
			//else
			//{
			//	inventoryManager.RemoveItemInPocket(this.slotNum, 1);
			//	Debug.Log($"QuickSlot Click RemoveItemInPocket{this.slotNum}");
			//}

		}



		public override ItemData GetItem()
		{
			return gm.GetPocket()[slotNum];
		}

		public override void SetItem()
		{
			var pocket = gm.GetPocket();
            if (slotNum < pocket.Count && gm.GetPocket().Count > slotNum)
            {
				
				icon.sprite = gm.GetImage(gm.GetPocket()[slotNum].imageID);
				countText.text = $"{gm.GetPocket()[slotNum].count}";
				countText.gameObject.SetActive(true);
			}
            else
            {
				EmptySlot();
			}

		}

		internal void EmptySlot()
		{
			icon.sprite = null;
			icon.enabled = false;
			countText.gameObject.SetActive(false);
		}
	}

}
