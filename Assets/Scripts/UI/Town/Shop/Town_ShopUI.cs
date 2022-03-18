using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Town
{
    public class Town_ShopUI : MonoBehaviour
    {

        public List<Town_ShopSlot> slots;
        public Transform slotHolder;
        public ObjectPoolSCO objectPool;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

		internal void Initiate()
		{
            //테스트 가나다라
		}
        public void ResetNumIndex()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                slots[i].slotNum = i;
            }
        }
        public Town_ShopSlot AddSlot(ItemData item)
		{
            var slotItem = (Town_ShopSlot)objectPool.RequestObject(PrefabID.ShopSlot);
            slotItem.transform.SetParent(slotHolder);
            slotItem.slotNum = slots.Count;
            slotItem.Initiate();
            slotItem.SetItem(item);
            slots.Add(slotItem);
            return slotItem;
        }
        public void RemoveSlot(int i)
		{
            objectPool.ReturnObject(slots[i]);
            slots.RemoveAt(i);
            ResetNumIndex();
            Debug.Log($"{i}");
		}

        public void RemoveSlotAll()
		{
            for (var i = 0; i < slots.Count; ++i)
			{
                objectPool.ReturnObject(slots[i]);
            }
            slots.Clear();
        }

    }
}