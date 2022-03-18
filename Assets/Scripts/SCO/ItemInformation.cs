using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "new Item Info", menuName = "Data/Item", order = 1)]
public class ItemInformation : InformationOrigin
{
   
    [SerializeField]
    private int level;
    [SerializeField]
    private Sprite[] herosPic;



    [SerializeField]
    private ItemType itemLargeCategory;

    public ItemType ItemLargeCategory {
        get { return itemLargeCategory; }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }

    public Sprite[] HerosPic
    {
        get
        {
            return herosPic;
        }
    }

}
