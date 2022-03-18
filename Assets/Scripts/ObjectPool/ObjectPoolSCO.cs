

using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "ObjectPool", menuName = "Manager/PrefabPoolManager")]
public class ObjectPoolSCO : SerializedScriptableObject
{
    public int debug;


    [SerializeField] List<PoolableObjectData> prefabsss;
    //[SerializeField] GameManager gm;
    [SerializeField] Dictionary<PrefabID, PoolableObject> prefabs;
    [SerializeField] Dictionary<PrefabID, Stack<PoolableObject>> pool;
    [SerializeField] Dictionary<PrefabID, Stack<PoolableObject>> active;
    [SerializeField] Dictionary<PrefabID, int> count;

    //[SerializeField] Dictionary<HeroType, PlayerUnitController> heroPrefabs;
    //public Dictionary<HeroType, PlayerUnitController> heroPool;
    private void Awake()
    {
        
    }
    public void Initiate()
    {
        prefabs = new Dictionary<PrefabID, PoolableObject>();

        foreach (var item in prefabsss)
		{
            prefabs.Add(item.iD, item.poolableObject);

        }

        pool = new Dictionary<PrefabID, Stack<PoolableObject>>();
        active = new Dictionary<PrefabID, Stack<PoolableObject>>();
        count = new Dictionary<PrefabID, int>();
        //foreach (var item in prefabs)
        //{
        //    pool.Add(item.Key, new List<GameObject>());
        //}
        //heroPool = new Dictionary<HeroType, PlayerUnitController>();
    }

    //public PlayerUnitController RequestHero(HeroType heroType, Vector3 position, Quaternion rotation)
    //{
    //    if (!heroPool.ContainsKey(heroType))
    //    {
    //        Debug.Log($"{heroType}이 만들어졌습니다");
    //        heroPool.Add(heroType, Instantiate(heroPrefabs[heroType], position, rotation));
    //    }
    //    return heroPool[heroType];
    //}

    public bool ReturnObject(PoolableObject poolableObject) {
        pool[poolableObject.prefabID].Push(poolableObject);
        active[poolableObject.prefabID].Pop();
        poolableObject.gameObject.SetActive(false);
        return true;

    }

    public PoolableObject RequestObject(PrefabID prefabID)
    {
        return CallObject_Active(prefabID, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0), null); ;
    }
    public PoolableObject RequestObject(PrefabID prefabID, Vector3 position)
    {
        return CallObject_Active(prefabID, position, Quaternion.Euler(0, 0, 0), null);
    }
    public PoolableObject RequestObject(PrefabID prefabID, Vector3 position, Quaternion rotation)
    {
        return CallObject_Active(prefabID, position, rotation, null);
    }
    public PoolableObject RequestObject(PrefabID prefabID, Vector3 position, Quaternion rotation, Transform parent)
    {
        return CallObject_Active(prefabID, position, rotation, parent);
    }


    PoolableObject CallObject_Active(PrefabID prefabID, Vector3 position, Quaternion rotation, Transform parent)
    {

        if (!pool.ContainsKey(prefabID))
        {
            pool.Add(prefabID, new Stack<PoolableObject>());
            active.Add(prefabID, new Stack<PoolableObject>());
            count.Add(prefabID, 0);
        }



        if (pool[prefabID].Count == 0) //오브젝트가 없을경우
        {
            var poolObj = CreateNewObjectList(prefabID, position, rotation, parent); //리스트도 새로 생성 하고 오브젝트도 새로 생성해서 추가
            active[prefabID].Push(poolObj);
            return poolObj;
        }
        else //있을 경우
        {
            var GetObject = pool[prefabID].Peek();

            GetObject.transform.position = position;
            GetObject.transform.rotation = rotation;
            GetObject.transform.SetParent(parent);
            GetObject.gameObject.SetActive(true);
            pool[prefabID].Pop();
            active[prefabID].Push(GetObject);
            return GetObject;
        }
    }



    //PoolableObject SearchListActive(PrefabID prefabID)
    //{
    //    int count = pool[prefabID].Count;
    //    for (int i = 0; i < count; i++)
    //    {
    //        if (pool[prefabID][i] == null)
    //        {
    //            pool[prefabID].Remove(pool[prefabID][i]);
    //        }
    //        else if (pool[prefabID][i].gameObject.activeSelf == false)
    //        {
    //            return pool[prefabID][i];
    //        }
    //    }
    //    return null;
    //}

    PoolableObject CreateNewObjectList(PrefabID prefabID, Vector3 position, Quaternion rotation, Transform parent)
    {
        PoolableObject NewObject = Instantiate(prefabs[prefabID], position, rotation, parent);
        NewObject.gameObject.name = NewObject.gameObject.name + $"{  count[prefabID]}";
        NewObject.prefabIndex = count[prefabID];
        count[prefabID]++;

        return NewObject; //선택된 오브젝트 바로 보내기
    }
}

[System.Serializable]
public class PoolableObjectData {
    public PrefabID iD;
    public PoolableObject poolableObject;
}