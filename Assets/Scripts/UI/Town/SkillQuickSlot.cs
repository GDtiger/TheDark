using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Town
{
    public class SkillQuickSlot : SkillSlot
    {

        public Sprite originImage;
        public override void Initiate()
        {
			if (skillManager == null)
			{
                skillManager = Town_SkillManager.Instance;
                Debug.Log($"{gameObject.name} + {skillManager}");
            }
            if (gm == null) gm = GameManager.Instance;

  
        }

        public void SetSkillData(SkillData val) {
            skillData = val;
            if (val != null)
            {
                icon.sprite = skillData.sprite;
                icon.enabled = true;
            }
            else
            {
                icon.enabled = false;
            }
           
        }

		public override void ResetSkillButton()
		{
            icon.sprite = originImage;
            skillData = null;
            icon.enabled = false;
        }
		public override void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"{gameObject.name}");
            if (skillData != null)
            {
                skillManager.ClickSlot(this);
            }
        }
    }
}

