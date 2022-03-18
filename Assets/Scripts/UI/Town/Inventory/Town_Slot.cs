using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace Town
{
    public class Town_Slot : PoolableObject, IPointerClickHandler
    {
        [TabGroup("Slot", "Slot")] public int slotNum;
        [TabGroup("Slot", "Need")] [SerializeField] protected Image icon;
        [TabGroup("Slot", "Need")] [SerializeField] protected TextMeshProUGUI countText;
        [TabGroup("Slot", "Auto")] [SerializeField] protected Town_InventoryManager inventoryManager;
        [TabGroup("Slot", "Auto")] [SerializeField] protected Town_ShopManager shopManager;
        [TabGroup("Slot", "Auto")] [SerializeField] protected GameManager gm;

        public virtual void Initiate()
        {
			if (inventoryManager == null) inventoryManager = Town_InventoryManager.Instance;
			if (shopManager == null) shopManager = Town_ShopManager.Instance;
			if (gm == null)	gm = GameManager.Instance;
		}

		public void RemoveItem()
		{
			//item = null;
			icon.enabled = false;

		}

		public void SetItem(ItemData _item)
		{
			icon.sprite = gm.GetImage(_item);
			icon.enabled = true;
			countText.text = $"{_item.count}";
			Debug.Log($"SetItem {_item.count}");
		}

		public virtual void SetItem()
		{
			Debug.Log("?");
			icon.sprite = gm.GetImage(gm.inventory[slotNum]);
			countText.text = $"{gm.inventory[slotNum].count}";
		}

		public virtual ItemData GetItem()
		{
			return gm.inventory[slotNum];
		}


		//public void UpdateSlotUI()
		//{
		//	icon.sprite = item.image;
		//	icon.gameObject.SetActive(true);
		//}
		//public void RemoveSlot()
		//{
		//	icon.gameObject.SetActive(false);
		//}


		//Äü½½·ÔÀ¸·Î ²¨Áú³ð
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (Town_ShopManager.Instance.isOpenShop)
			{
				inventoryManager.isSellItem = true;
				shopManager.ShopTooltip.SetTooltip(slotNum);
				//gm.gold += GetItem().price;
				//inventoryManager.RemoveItemInInventory(slotNum, 1);
				//inventoryManager.inventoryUI.UpdateSlot();
				//Debug.Log($"{gm.gold}");
			}
			else
			{
				inventoryManager.ClickSlot(this);
			}
		}
	}

}

