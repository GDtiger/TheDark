
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class ItemEditorWindow : InformationEditorWindow<ItemInformation>
{

    public ItemType itemLargeCategory;

    [MenuItem("Project/Item Editor")]
    protected static void ShowWindow()
    {
        GetWindow<ItemEditorWindow>("Items");
    }

    public override bool CorrectWithCategory(ItemInformation type)
    {
        if (itemLargeCategory == ItemType.Null)
        {
            return (type.ItemLargeCategory == ItemType.Null)? true : false;
        }
        else
        {
            return (type.ItemLargeCategory & itemLargeCategory) != 0 && (type.ItemLargeCategory != ItemType.Null) ? true : false;
        }
        
       
    }

    public void Awake()
    {
        itemLargeCategory = (ItemType)1;
    }

    protected override float DrawOnLeftSideBar(ref int category1)
    {

        itemLargeCategory = (ItemType)EditorGUILayout.EnumFlagsField("아이템 종류:", itemLargeCategory);
        //itemLargeCategory = (ItemLargeCategory)EditorGUILayout.EnumPopup("아이템 보고싶은 아이템 종류를 선택하세요:", itemLargeCategory);
        //specificCategory1 = itemLargeCategory == ItemLargeCategory.All ? true : false;



        category1 = (int)itemLargeCategory;
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("아이템 리스트");
        return 18;
    }


    public override void DrawSliderBar(SortedDictionary<int, ItemInformation> prop)
    {
        base.DrawSliderBar(prop);
        if (GUILayout.Button("new Item"))
        {
            ItemInformation newHero = CreateInstance<ItemInformation>();
            CreateNewItemWindow newHeroWindow = GetWindow<CreateNewItemWindow>("New Item Maker");
            newHeroWindow.Initiate(newHero, "Item", "Assets/SCO/Data/Items");

        }
    }
}