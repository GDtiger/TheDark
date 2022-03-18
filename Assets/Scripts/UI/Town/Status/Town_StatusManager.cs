using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Town
{
	public class Town_StatusManager : MonoBehaviour
	{
		#region instance
		static Town_StatusManager instance = null;
		public static Town_StatusManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<Town_StatusManager>();
				}
				return instance;
			}
		}
		#endregion
		public GameManager gm;
		public Town_InventoryManager im;
		public TextMeshProUGUI goldTxt;
		public TextMeshProUGUI diamondTxt;
		public GameObject optionUI;
		public Transform itemSlotHolder;
		public Town_PocketSlot[] statusItemQuickSlots;
		public Transform skillSlotHolder;
		public SkillQuickSlot[] statusSkillQuickSlots;


		public void Initiate()
		{
			gm = GameManager.Instance;
			im = Town_InventoryManager.Instance;
			statusItemQuickSlots = itemSlotHolder.GetComponentsInChildren<Town_PocketSlot>();
			for (int i = 0; i < statusItemQuickSlots.Length; i++)
			{
				statusItemQuickSlots[i].slotNum = i;
				statusItemQuickSlots[i].Initiate();
				statusItemQuickSlots[i].SetItem();
			}
		}
		void Update()
		{
			goldTxt.text = $"{string.Format("{0:#,###}", gm.gold)}";
			if (gm.gold <= 0)
			{
				goldTxt.text = "0";
			}

			diamondTxt.text = $"{string.Format("{0:#,###}", gm.diamond)}";
			if (gm.diamond <= 0)
			{
				diamondTxt.text = "0";
			}
		}

		public void OnClickOpenSetting()
		{
			gm.soundManager.PlaySFXSound("Confirm");
			optionUI.SetActive(true);
		}

		public void OnClickCloseSetting()
		{
			gm.soundManager.PlaySFXSound("Cancle");
			optionUI.SetActive(false);
		}

		public void UpdateSlot()
		{
			for (int i = 0; i < statusItemQuickSlots.Length; i++)
			{
				statusItemQuickSlots[i].SetItem();
			}
		}
		internal void UpdateSlot(int i)
		{
			statusItemQuickSlots[i].SetItem();
		}

		public void AddItem(int i, ItemData item)
		{
			statusItemQuickSlots[i].SetItem(item);

			Debug.Log(statusItemQuickSlots[i].slotNum);
		}

		public void RemoveItem(int index)
		{
			statusItemQuickSlots[index].EmptySlot();
		}
	}
}

