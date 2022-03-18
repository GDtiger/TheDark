using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissTextureController : PoolableObject
{
    public float lifeTime;
    public Vector3 offset;
    public Vector3 speed;
    public float curTime;

    public void ShowText(Vector3 pos)
    {
        //var mySequence = DOTween.Sequence()
        //.InsertCallback(lifeTime, () => gameObject.SetActive(false))
        //.OnComplete(() =>);
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


