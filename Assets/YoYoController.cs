using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoYoController : MonoBehaviour
{
    public float time;
    public Vector3 offset;
    public void Start()
    {
        transform.DOMove(offset + transform.position, time).SetLoops(-1, LoopType.Yoyo);
    }
}
