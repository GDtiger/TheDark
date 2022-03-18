using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
public class EffectManager : SerializedMonoBehaviour
{

    public enum EffectType { nun, sword};
    public static EffectManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EffectManager>();
                //if (instance == null)
                //{
                //    var instanceContainer = new GameObject("EffectSetting");
                //    instance = instanceContainer.AddComponent<EffectSetting>();
                //}
            }

            return instance;
        }

    }

    private static EffectManager instance;

    public Dictionary<EffectType, EffectController> effectPrefabs;


    public void Initiate(EffectType effectType, Vector3 pos, Quaternion rot, float deadTime, float delayTime)
    {

#if UNITY_EDITOR
        if (!effectPrefabs.ContainsKey(effectType))
        {
            Debug.Log($"{effectType} is not contain in EffectManager");
        }
#endif

        DOTween.Sequence().InsertCallback(delayTime, () =>
        {
            var temp = Instantiate(effectPrefabs[effectType], pos, rot);
            temp.Initiate(deadTime);
        });
    }

    public void Initiate(EffectType effectType, Vector3 pos, Quaternion rot)
    {
        Instantiate(effectPrefabs[effectType], pos, rot);
    }

    public void Initiate(EffectType effectType, Vector3 pos, Quaternion rot, float deadTime)
    {
        var temp = Instantiate(effectPrefabs[effectType], pos, rot);
        temp.Initiate(deadTime);
    }

}

