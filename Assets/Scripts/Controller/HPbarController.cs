using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPbarController : MonoBehaviour
{
    public Image hpBar;
    public Image hpBackground;
    private void Start()
    {
        hpBar.enabled = false;
        hpBackground.enabled = false;
    }


    public void ShowHpBar() {
        if (!hpBar.enabled)
        {
            hpBar.enabled = true;
            hpBackground.enabled = true;
        }
    }
    
    public void SetHp(float ratio) 
    {
        hpBar.fillAmount = ratio;
    }


}
