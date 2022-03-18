using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Town
{
	public class Town_UIManager : MonoBehaviour
	{
		#region instance
		static Town_UIManager instance = null;
		public static Town_UIManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<Town_UIManager>();
				}
				return instance;
			}
		}
		#endregion

		//public OptionUI option;
		[TabGroup("Ref", "Need")] public MainScenePlayerController controller;
		[TabGroup("Ref", "Need")] public StageSelectController stageSelectController;

		[TabGroup("Ref", "Auto")] public Town_InventoryManager inventoryManager;
		[TabGroup("Ref", "Auto")] public Town_SkillManager skillManager;
		[TabGroup("Ref", "Auto")] public Town_ShopManager shopManager;
		[TabGroup("Ref", "Auto")] public Town_StatusManager statusManager;

		public List<GameObject> uiWindow;
		public GameObject optionPanel;

		public void Initiate()
		{
			if (inventoryManager == null) inventoryManager = Town_InventoryManager.Instance;
			if (shopManager == null) shopManager = Town_ShopManager.Instance;
			if (skillManager == null) skillManager = Town_SkillManager.Instance;
			if (statusManager == null) statusManager = Town_StatusManager.Instance;
		
			inventoryManager.Initiate();
			shopManager.Initiate();
			skillManager.Initiate();
			statusManager.Initiate();

		}
		public void OpenShop()
		{
			shopManager.OnClickOpenShop();
		}
		public void OpenSKill()
		{
			skillManager.OnClickOpenSkillWindow();
		}
		public void OpenChest()
		{
			inventoryManager.OnClickOpenInventory();
		}
		public void OpneStageSelectUI(SceneID sceneID)
		{
			stageSelectController.OpenPannel(sceneID);
		}
		public void CloseWindow()
		{
			controller.isMove = true;
			for (int i = 0; i < uiWindow.Count; i++)
			uiWindow[i].SetActive(false);
		}

		public void CloseAllUI()
		{
			gameObject.SetActive(false);
		}
		public void OnClickOpenOption()
		{
			SoundManager.Instance.PlaySFXSound("Confirm");
			optionPanel.SetActive(true);
		}
		public void OnClickCloseOption()
		{
			SoundManager.Instance.PlaySFXSound("Cancle");
			optionPanel.SetActive(false);
		}
	}
	
}
