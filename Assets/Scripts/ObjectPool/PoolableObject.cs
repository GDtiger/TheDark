using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class PoolableObject : MonoBehaviour {
    [TabGroup("Ref", "Poolable")] public ObjectPoolSCO objectPool;
    [TabGroup("Ref", "Poolable")] public int prefabIndex;
    [TabGroup("Ref", "Poolable")] public PrefabID prefabID;

    [TabGroup("Ref", "Poolable")] [SerializeField] bool hasDeadTime;
    [TabGroup("Ref", "Poolable")] [SerializeField] float deadTime;
    public float DeadTime { get { return deadTime; } set { deadTime = value; } }

    public virtual void OnEnable()
    {
        if (hasDeadTime)
        {
            Invoke("Return", deadTime);//this will happen after 2 seconds
        }
    }



    public virtual void Return()
    {
        if (objectPool == null)
        {
            objectPool = GameManager.Instance.objectPool;
        }
        //Debug.Log($"{gameObject.name}은 오브젝트풀로 돌려놓습니다. ({Time.time})");
        gameObject.SetActive(false);
        objectPool.ReturnObject(this);
    }
}