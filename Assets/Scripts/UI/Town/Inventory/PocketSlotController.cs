using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PocketSlotController : MonoBehaviour { 
    public int index;
    public Image img;
    public Transform showCoolTimeObject;
    public Transform showActiveSkillIcon;
    public TMPro.TMP_Text remainCoolTime;
    public TMPro.TMP_Text quantityText;
    


    public void InitiateButton(Sprite skillIcon)
    {
        showCoolTimeObject.gameObject.SetActive(false);
        img.sprite = skillIcon;
    }



    public void DrawButton(Sprite image, int remainTurn, int _quantity)
    {
        img.sprite = image;
        if (remainTurn > 0)
        {
            remainCoolTime.text = remainTurn.ToString();

            showCoolTimeObject.gameObject.SetActive(true);
        }
        else
        {
            showCoolTimeObject.gameObject.SetActive(false);
        }

        if (_quantity > 0)
        {
            quantityText.text = _quantity.ToString();
            quantityText.gameObject.SetActive(true);
        }
        else
        {
            quantityText.gameObject.SetActive(false);
        }
    }

    public void DrawButton()
    {
        img.sprite = GameManager.Instance.imageDataBase.GetImage("Item_000").image;
        showCoolTimeObject.gameObject.SetActive(false);
        quantityText.gameObject.SetActive(false);
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

