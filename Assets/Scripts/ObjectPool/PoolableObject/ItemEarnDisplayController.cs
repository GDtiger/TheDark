
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ItemEarnDisplayController : PoolableObject
{
    public TMPro.TMP_Text text;
    public float damage;
    public float lifeTime;
    public Vector3 offset;

    public float curTime;
    public Vector3 speed;
    public void ShowText(string itemCode,  Vector3 pos, int count = 1, bool buff = false)
    {
        text.text = itemCode + " x " + count.ToString();
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
