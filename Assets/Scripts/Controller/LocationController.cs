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
			//�̵� Ȯ��â
			//��� Ȯ��
			switch (uIType)
			{
				case UIType.Shop:
					mainUIManager.OpenShop();
					Debug.Log("��������");
					break;
				case UIType.Skill:
					mainUIManager.OpenSKill();
					Debug.Log("��ų");
					break;
				case UIType.Chest:
					mainUIManager.OpenChest();
					Debug.Log("â��");
					break;
				case UIType.StageSelect:
					mainUIManager.OpneStageSelectUI(sceneID);
					Debug.Log("�������� �Ѿ�� Ȯ��â");
					break;
				default:
					break;
			}
		}

	}

}
