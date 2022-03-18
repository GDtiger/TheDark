using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Town
{
	public class Town_SkillManager : MonoBehaviour
	{
		#region instance
		static Town_SkillManager instance = null;
		public static Town_SkillManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<Town_SkillManager>();
				}
				return instance;
			}
		}
		#endregion



		public SkillSlot curSlot;
		public SkillSlot[] skillSlots;
		public SkillQuickSlot[] skillQuickslots;
		public bool isOpenSkillUI;
		[TabGroup("Ref", "Auto")] public GameManager gm;

		//public SkillTreeUI skillUI;
		//public UI_SkillQuickSlots skillPocketUI;
		[TabGroup("Ref", "Need")] public UI_SkillToolTip skillTooltipUI;
		[TabGroup("Ref", "Need")] public TextMeshProUGUI skillPointTxt;
		[TabGroup("Ref", "Need")] public GameObject activePanel;
		[TabGroup("Ref", "Need")] public GameObject passivePanel;

        //public int curIndex;
        public bool isSkillSlot;


		public void Initiate() {
			gm = GameManager.Instance;
            foreach (var slot in skillSlots)
            {
				slot.Initiate();

			}
            for (int i = 0; i < skillQuickslots.Length; i++)
            {
				skillQuickslots[i].Initiate();
				skillQuickslots[i].slotNum = i;
			}

			RedrawQuickSlots();
			OnClickCloseSkillWindow();
		}

        public void OnclickEquipment()
		{
			gm.soundManager.PlaySFXSound("Confirm");
			if (isSkillSlot)
			{
				AddSkillToQuickSlot(curSlot.GetSkillData());
			}
			else
			{

				RemoveSkillInQuickSlot();
			}
		}
		private void AddSkillToQuickSlot(SkillData skillData)
		{
			var playerStatus = gm.GetUnitStatus();
            foreach (var skill in playerStatus.skills)
            {
                if (skillData.ID == skill)
                {
                    //TODO UI: ���� ��ų�̹Ƿ� �߰��Ҽ����� UI ǥ��

                    Debug.Log($"���� ��ų�� �ش����ֿ� �̹� �����ϹǷ� �߰��Ҽ� �����ϴ�.");
					return;
                }
            }
			if (playerStatus.skills.Count - 2 < gm.maxQuickSlotSkillSize)
            {
				skillQuickslots[playerStatus.skills.Count - 2].SetSkillData(skillData);
				gm.town_UIManager.statusManager.statusSkillQuickSlots[playerStatus.skills.Count -2].SetSkillData(skillData);
				if (gm.isTutorial && TutorialController.Instance.tutorialNumber == 9)
				{
					TutorialController.Instance.tutorialDescriptionTitle.text = "����Ʈ ����";
					Debug.Log($"{TutorialController.Instance.tutorialNumber}");
					TutorialController.Instance.OpenPopup();
					TutorialController.Instance.Reward(6, "Item");
					TutorialController.Instance.tutorialDescriptionTitle.text = "����Ʈ ����";
				}
				playerStatus.skills.Add(skillData.ID);
            }
            else
            {
				//TODO UI: ��ų ������ ����á�ٴ� UI ǥ��
				Debug.Log($"��ų�� ����á���ϴ�.");
			}
		}
		private bool RemoveSkillInQuickSlot()
        {
            var playerStatus = gm.GetUnitStatus();
            playerStatus.skills.RemoveAt(curSlot.slotNum + 2);
			RedrawQuickSlots();
			if (gm.isTutorial)
			{
				TutorialController.Instance.UnArrow();
			}
			return true;
        }
		public void OnClickOpenSkillWindow()
		{
			isOpenSkillUI = true;
			gameObject.SetActive(isOpenSkillUI);
			RedrawQuickSlots();
			UpdateskillPointTxt();
			skillTooltipUI.SetToolTip();
			if (gm.isTutorial && TutorialController.Instance.tutorialNumber == 8)
			{
				Debug.Log($"{TutorialController.Instance.tutorialNumber}");
				TutorialController.Instance.OpenPopup();
				if (TutorialController.Instance.tutorialNumber == 8)
				{
					TutorialController.Instance.InfinityArrow(3, DirEight.B);

				}
			}
		}


		public void OnClickCloseSkillWindow()
		{
			gm.soundManager.PlaySFXSound("Close");
			isOpenSkillUI = false;
			gameObject.SetActive(isOpenSkillUI);
			//skillTooltipUI.gameObject.SetActive(false);
			Town_UIManager.Instance.CloseWindow();
			if (gm.isTutorial && TutorialController.Instance.tutorialNumber == 10)
			{
				Debug.Log($"{TutorialController.Instance.tutorialNumber}");
				TutorialController.Instance.OpenPopup();
				TutorialController.Instance.UnArrow();
				gm.isTutorial = false;
			}
		}
		public void ResetSkillPoint() {
			gm.GetUnitStatus().curSkillPoint = gm.GetUnitStatus().skillPoint;
		 var skill =	gm.GetUnitStatus().skills;
			while (skill.Count > 2)
			{
				skill.RemoveAt(skill.Count - 1);
			}
			for (int i = 0; i < skillQuickslots.Length; i++)
			{
				skillQuickslots[i].ResetSkillButton();
			}
			for (int i = 0; i < skillSlots.Length; i++)
            {
				skillSlots[i].ResetSkillButton();
			}
			UpdateskillPointTxt();
		}
		private void RedrawQuickSlots()
        {
			var playerStatus = gm.GetUnitStatus();
			for (int i = 0; i < skillQuickslots.Length; i++)
            {
                if (playerStatus.skills.Count - 2 > i)
                {
					skillQuickslots[i].SetSkillData(gm.GetSkillDataFromDB(playerStatus.skills[i + 2]));
					gm.town_UIManager.statusManager.statusSkillQuickSlots[i].SetSkillData(gm.GetSkillDataFromDB(playerStatus.skills[i + 2]));

				}
                else
                {
					skillQuickslots[i].EmptySkillSlot();
					gm.town_UIManager.statusManager.statusSkillQuickSlots[i].EmptySkillSlot();

				}

            }
        }

        public void ClickSlot(SkillSlot slot)
		{
			gm.soundManager.PlaySFXSound("Confirm");
			UpdateskillPointTxt();
			isSkillSlot = slot is SkillQuickSlot ? false : true;
			if (curSlot == null || curSlot != slot)
			{
				curSlot = slot;
                skillTooltipUI.SetToolTip(slot.GetSkillData());
			

				Debug.Log("1");
			}
			else
			{
				OnclickEquipment();
			}
		}
		public void UpdateskillPointTxt()
		{
			skillPointTxt.text = $"��ų����Ʈ : {gm.unitStatus[0].curSkillPoint} / {gm.unitStatus[0].skillPoint}";
		}

		public void OpenActivePanel() {
			activePanel.SetActive(true);
			passivePanel.SetActive(false);
		}

		public void OpenPassivePanel() {
			activePanel.SetActive(false);
			passivePanel.SetActive(true);
		}
	}
}
