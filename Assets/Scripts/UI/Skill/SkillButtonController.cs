using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonController : MonoBehaviour
{
    public Image img;
    public Transform showCoolTimeObject;
    public Transform showActiveSkillIcon;
    public TMPro.TMP_Text remain;
    //public int remainCoolTime;

    public void InitiateButton(Sprite skillIcon) {
        DeactiveButton();
        showCoolTimeObject.gameObject.SetActive(false);
        img.sprite = skillIcon;
    }

    public void DrawButton(int remainTurn) {

        if (remainTurn > 0)
        {
            remain.text = remainTurn.ToString();
            
            showCoolTimeObject.gameObject.SetActive(true);
        }
        else
        {
            showCoolTimeObject.gameObject.SetActive(false);
        }
    }

    public void AcctiveButton()
    {
        showActiveSkillIcon.gameObject.SetActive(true);
    }
    public void DeactiveButton()
    {
        showActiveSkillIcon.gameObject.SetActive(false);
    }
}
