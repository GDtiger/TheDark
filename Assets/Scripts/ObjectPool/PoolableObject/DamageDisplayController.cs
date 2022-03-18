
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DamageDisplayController : PoolableObject
{
    public TMPro.TMP_Text text;
    public float damage;
    public float lifeTime;
    public Vector3 offset;
    public Vector3 speed;
    public float curTime;
    public void ShowText(int damage, Vector3 pos, bool buff = false)
    {
        //var mySequence = DOTween.Sequence()
        //.InsertCallback(lifeTime, () => gameObject.SetActive(false))
        //.OnComplete(() =>);
        text.text = damage.ToString();
        if (buff)
        {
            text.color = Color.green;
        }
        else
        {
            text.color = Color.red;
        }
        curTime = Time.time + lifeTime;
        transform.position = pos + offset;
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime;
        if (curTime < Time.time)
        {
            Return();
        }
      
    }
}
