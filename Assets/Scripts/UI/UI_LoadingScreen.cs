using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingScreen : MonoBehaviour
{
    public Image fade;
    public TMPro.TMP_Text timeText;

    private void Update()
    {
        timeText.text = Time.time.ToString();
    }

    public void FadeWindow(bool val)
    {
        if (val)
        {
            gameObject.SetActive(true);
            fade.color =  Color.white;


        }
        else
        {
            Sequence mySequence = DOTween.Sequence();

            mySequence.Insert(3, fade.DOFade(0, 3).OnComplete(() => gameObject.SetActive(false)));
        }

    }
}
