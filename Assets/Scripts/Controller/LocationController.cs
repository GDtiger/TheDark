using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Town
{
	public class LocationController : MonoBehaviour
	{
		public Transform location;
		public enum UIType { Shop, Skill, Chest, StageSelect, };

		public UIType uIType;
		public SceneID sceneID;
		public Town_UIManager mainUIManager;


		public void Initiate() {
			if (mainUIManager == null) mainUIManager = Town_UIManager.Instance;
		}

		public void OpenWindow()
		{
			//이동 확인창
			//취소 확인
			switch (uIType)
			{
				case UIType.Shop:
					mainUIManager.OpenShop();
					Debug.Log("상점열림");
					break;
				case UIType.Skill:
					mainUIManager.OpenSKill();
					Debug.Log("스킬");
					break;
				case UIType.Chest:
					mainUIManager.OpenChest();
					Debug.Log("창고");
					break;
				case UIType.StageSelect:
					mainUIManager.OpneStageSelectUI(sceneID);
					Debug.Log("스테이지 넘어가기 확인창");
					break;
				default:
					break;
			}
		}

	}

}
