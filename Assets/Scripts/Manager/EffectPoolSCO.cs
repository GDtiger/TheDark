using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

[CreateAssetMenu(fileName = "EffectPool", menuName = "Manager/EffectPool")]
public class EffectPoolSCO : ScriptableObject
{

    private static EffectPoolSCO instance;

    public Dictionary<EffectID, EffectController> effectPrefabs;
    public List<EffectController> effectControllers;

    public void Initiate() {
        effectPrefabs = new Dictionary<EffectID, EffectController>();
        foreach (var effect in effectControllers)
        {
            effectPrefabs.Add(effect.effectID, effect);
        }
    }



    public void RequestEffect(EffectID effectType, Vector3 pos, Quaternion rot, float deadTime, float delayTime)
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

    public void RequestEffect(EffectID effectType, Vector3 pos, Quaternion rot)
    {
        Instantiate(effectPrefabs[effectType], pos, rot);

        Debug.Log($"{effectType} ¹ßµ¿");

    }

    public void RequestEffect(EffectID effectType, Vector3 pos, Quaternion rot, float deadTime)
    {
        var temp = Instantiate(effectPrefabs[effectType], pos, rot);
        temp.Initiate(deadTime);
    }

}

