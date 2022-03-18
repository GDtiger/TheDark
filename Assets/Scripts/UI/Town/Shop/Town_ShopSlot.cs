using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace Town
{
    public class Town_ShopSlot : PoolableObject, IPointerClickHandler
    {
        [TabGroup("Slot", "Auto")] public GameManager gm;
        [TabGroup("Slot", "Auto")] public Town_ShopManager shopManager;
        [TabGroup("Slot", "Need")] public Image icon;
        [TabGroup("Slot", "Need")] public TextMeshProUGUI price;
        [TabGroup("Slot", "Slot")] public int slotNum;

        public void Initiate()
		{
            if (shopManager == null) shopManager = Town_ShopManager.Instance;
            if (gm == null) gm = GameManager.Instance;
		}

        public void SetItem(ItemData _item)
		{
            icon.sprite = gm.GetImage(_item);
            icon.enabled = true;
            price.text = $"{_item.price}";
		}

        public void OnPointerClick(PointerEventData eventData)
        {
            shopManager.ClickSlot(this);
        }
    }
}
