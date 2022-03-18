using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateYoYo : MonoBehaviour
{

    public float time;
    public Vector3 offset;
    // Start is called before the first frame update
    public void Start()
    {
        transform.DOScale(offset, time).SetLoops(-1, LoopType.Yoyo);
    }
}
