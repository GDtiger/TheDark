using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Town
{
    public class UI_SkillToolTip : MonoBehaviour
    {
        public TMPro.TMP_Text skillNameText;
        public TMPro.TextMeshProUGUI skillDescription;
        public TMPro.TextMeshProUGUI skillDetailDescription;
        public TMPro.TextMeshProUGUI skillDetailDescription2;
        public Image skillIconImage;
        //public TMPro.TMP_Text skillLevel;
        public void SetToolTip(SkillData skillData) {
            skillNameText.text = $"{skillData.skillName} - LV{skillData.skillLevel}";
            skillIconImage.sprite = skillData.sprite;
            skillIconImage.enabled = true;
            skillDescription.text = $"{skillData.description}";
            skillDetailDescription.text = 
                $"쿨타임 : {skillData.delay}턴" +
                "\n" +
                $"소리 : {skillData.sound}칸" +
                "\n" +
                $"마나 : {skillData.cost}";
            skillDetailDescription2.text =
                $"데미지 : {skillData.damage}" +
                "\n" +
                $"범위 : {skillData.distance}칸" +
                "\n" +
                $"사거리 : {skillData.range}칸";
            //skillLevel.text = skillData.skillLevel.ToString();
        }

        public void SetToolTip()
        {
            skillNameText.text = $"";
            skillIconImage.enabled = false;
            skillDescription.text = $"";
            skillDetailDescription.text = $"";
            skillDetailDescription2.text = $"";
            //skillLevel.text = skillData.skillLevel.ToString();
        }
    }
}
