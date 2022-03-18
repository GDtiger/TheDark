using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Sirenix.OdinInspector;

namespace Town
{
    public class SkillSlot : MonoBehaviour, IPointerClickHandler
    {
        public int slotNum;
        [TabGroup("Ref", "Need")] [SerializeField] protected Image icon;
        [TabGroup("Ref", "Need")] [SerializeField] protected SkillData skillData;
        [TabGroup("Ref", "Auto")] [SerializeField] protected Town_SkillManager skillManager;
        [TabGroup("Ref", "Auto")] [SerializeField] protected GameManager gm;
        [TabGroup("Ref", "Auto")] [SerializeField] protected bool ableToUse;
        [TabGroup("Ref", "Need")] [SerializeField] protected GameObject learnSkill;
        [TabGroup("Ref", "Need")] [SerializeField] protected GameObject image;
        [TabGroup("Ref", "Need")] [SerializeField] protected bool inactive;

        

        public virtual void Initiate()
        {
            if (skillManager == null) skillManager = Town_SkillManager.Instance;
            if (gm == null) gm = GameManager.Instance;

            icon.sprite = skillData.sprite;
        }

        public SkillData GetSkillData() => skillData;


        public void EmptySkillSlot()
        {
            icon.enabled = false;
            skillData = null;
        }

        [Button]
        void ExDebug() {
            icon.sprite = skillData.sprite;

        }

        public virtual void ResetSkillButton()
        {
			if (image != null)
			{
                image.SetActive(true);
            }
            if (learnSkill != null)
            {
                learnSkill.SetActive(true);
            }

            ableToUse = false;
        }

        public void LearnSkill() {
            if (gm == null) gm = GameManager.Instance;
            gm.soundManager.PlaySFXSound("Confirm");
            if (!inactive && gm.GetUnitStatus().curSkillPoint > 0)
            {
                Debug.Log("LearnSkill");
                image.SetActive(false);
                learnSkill.SetActive(false);
                ableToUse = true;
                gm.GetUnitStatus().curSkillPoint--;
                skillManager.UpdateskillPointTxt();
                if (gm.isTutorial)
				{
                    TutorialController.Instance.UnArrow();
                    if (TutorialController.Instance.tutorialNumber == 9)
					{
                        TutorialController.Instance.InfinityArrow(2, DirEight.B);
                    }
                }
            }

        }
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (ableToUse)
            {
                skillManager.ClickSlot(this);
                if (gm.isTutorial)
                {
                    TutorialController.Instance.UnArrow();
                }
            }
           
        }
    }
}


