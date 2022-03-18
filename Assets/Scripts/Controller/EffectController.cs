using System.Collections;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] float time;
    public EffectID effectID;

    public void Initiate(float time)
    {
        this.time = time;
    }
    public void Start()
    {
        Destroy(gameObject, time);
    }


}
