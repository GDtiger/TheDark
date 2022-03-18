using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace Town
{
    public class ItemToolTipUI : MonoBehaviour
    {
        [TabGroup("Ref", "Need")] public TextMeshProUGUI itemName;
        [TabGroup("Ref", "Need")] public Image itemImage;
        [TabGroup("Ref", "Need")] public TextMeshProUGUI itemDescription;
        [TabGroup("Ref", "Need")] public TextMeshProUGUI itemDetailDescription;
        [TabGroup("Ref", "Need")] public TextMeshProUGUI itemDetailDescription2;
        [TabGroup("Ref", "Need")] public PocketUI pocketUI;


        [TabGroup("Ref", "Auto")] public Town_InventoryManager inventoryManager;
		[TabGroup("Ref", "Auto")] public GameManager gm;

        public void Initiate()
        {
            if (inventoryManager == null) inventoryManager = Town_InventoryManager.Instance;
            if (gm == null) gm = GameManager.Instance;
		}

		public void SetToolTip(int index)
		{
			ItemData item = null;
			if (inventoryManager.isInventoryItem)
			{
				item = gm.inventory[index];
			}
			else
			{
                if (gm.GetPocket().Count > index)
                {
					item = gm.GetPocket()[index];
                }
                else
                {
					return;
                }
			}
			if (item == null)
			{
				itemName.text = "";
				itemImage.sprite = null;
				itemDescription.text = "";
				return;
			}

			itemName.text = $"[{item.name}]";
			itemImage.sprite = gm.GetImage(item.imageID);
			itemImage.color = Color.black;
			itemDescription.text = $"{item.description}";
			itemDetailDescription.text =
				$"범위 : {item.effectRangeTile} " +
				"\n" +
				$"쿨타임 : {item.DelayTurn} 턴";
			itemDetailDescription2.text =
				$"가격 : {item.price} 골드" +
				"\n" +
				$"포켓 최대 : {item.pocketMaxCount} 개" +
				"\n" +
				$"사거리 : {item.distanceTile}";
			Debug.Log($"{item.description}");
			Debug.Log($"???");
			gameObject.SetActive(true);
		}

		public void CloseTooltip()
		{
			gameObject.SetActive(false);
		}


	}
}

