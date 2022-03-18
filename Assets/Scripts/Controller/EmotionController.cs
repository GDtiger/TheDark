using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmotionController : MonoBehaviour
{
    public Sprite iconWhenFoundEnemy;
    public Sprite iconWhenHearSound;
    public Sprite iconWhenMissingTargetFromSight;
    public Image image;
    public float lifeTime;
    Vector3 originSize;



    public void Start()
    {
        image.enabled = false;
    }



    private void ShowImage()
    {
        //gameObject.SetActive(true);
        //var mySequence = DOTween.Sequence()
        //.Append(image.transform.DOScale(originSize, 1).SetEase(Ease.OutBounce))
        //.InsertCallback(lifeTime, () => gameObject.SetActive(false))
        //.OnComplete(() => transform.localScale = Vector3.zero);
        image.enabled = true;
        //image.transform.DOScale(1, 1).SetEase(Ease.OutBounce);


    }

    public void HideImage() {
        image.enabled = false;
    }

    public void ShowWhenFoundTarget() {
        image.sprite = iconWhenFoundEnemy;
        image.color = new Color(204 / 255f, 22 / 255f, 11 / 255f,1);
        ShowImage();
    }

    public void ShowWhenHearSound()
    {
        image.sprite = iconWhenHearSound;
        image.color = new Color(242 / 255f, 152 / 255f, 48 / 255f,1);
        ShowImage();
    }

    public void ShowWhenMissingTarget()
    {
        image.sprite = iconWhenMissingTargetFromSight;
        image.color = new Color(1,1,1,1);
        ShowImage();
    }
}
