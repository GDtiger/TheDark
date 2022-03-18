using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

public class EditorWindow_ItemDataEditor : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty serializedProperty;

    protected string selectedPropertyPach;
    protected string selectedProperty;

    Vector2 scrollPos;
    public bool showSideBar;
    public int category1;
    public bool specificCategory1;

    public ItemDataBase itemDataBase;
    protected SortedDictionary<string, ItemData> data;
    public ItemType itemSearchType;
    public OrderItemMethod orderItemMethod;
    protected string selectedPropertyID = "";

    public string testString;
   //public ItemData testString;

    [MenuItem("Project/New Type/Item Editor")]
    protected static void ShowWindow()
    {
        GetWindow<EditorWindow_ItemDataEditor>("Item Data Manager");
    }

    private void OnGUI()
    {
        //데이터베이스가 없을경우 서치에서 넣어줌
        if (itemDataBase == null)
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(ItemDataBase).Name);
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            var temp = AssetDatabase.LoadAssetAtPath<ItemDataBase>(path);
            itemDataBase = temp;
        }

        
        if (itemDataBase != null)
        {
            itemSearchType = (ItemType)EditorGUILayout.EnumFlagsField("아이템 종류:", itemSearchType);
            orderItemMethod = (OrderItemMethod)EditorGUILayout.EnumPopup("아이템 정렬 방법:", orderItemMethod);
            if (GUILayout.Button("new Item"))
            {
                EditorWindow_CreateNewItem newHeroWindow = GetWindow<EditorWindow_CreateNewItem>("New Item Maker");
                newHeroWindow.Initiate(this, itemDataBase);
            }



            showSideBar = EditorGUILayout.Toggle("Show Side Bar", showSideBar);
            data = GetSpecificInstances();
            if (showSideBar)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(200), GUILayout.ExpandHeight(true));

                //좌측 사이드바
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MaxWidth(200), GUILayout.ExpandHeight(true));
                DrawSliderBar(data);

                //EditorGUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            }




            if (selectedPropertyID != "")
            {
                DrawInformation();
            }
            else
            {
                EditorGUILayout.LabelField("select an item from the list");
            }
            if (showSideBar)
            {
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

        }
        else
        {

        }
    }

    protected virtual float DrawOnLeftSideBar(ref int category1) {

        return 0;
    }

    void DrawInformation() {
        foreach (var item in data)
        {
            if (item.Value.ID == selectedPropertyID)
            {
                serializedObject = new SerializedObject(itemDataBase);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("tempItemData"));

                if (serializedObject.ApplyModifiedProperties())
                {
                    Debug.Log($"{item.Value.name} {item.Value.ID} // Edit");

                    var temp = itemDataBase.GetItemsFromDataBase();
                    for (int i = 0; i < temp.Count; i++)
                    {
                        if (temp[i].ID == itemDataBase.tempItemData.ID)
                        {
                            temp[i] = itemDataBase.tempItemData;
                        }
                    }
                }
            }
        }
    }

    protected void DrawProperties(SerializedProperty p)
    {

        while (p.NextVisible(false))
        {
            EditorGUILayout.PropertyField(p, true);
        }
    }



    public virtual void DrawSliderBar(SortedDictionary<string, ItemData> prop)
    {
        string tempID = "";
        //Debug.Log($"{selectedPropertyID} {tempID}");
        foreach (var p in prop)
        {
            if (GUILayout.Button(p.Value.name))
            {
                itemDataBase.tempItemData = p.Value;
                selectedPropertyID = p.Value.ID;
            }
        }
        if (tempID != "")
        {
            selectedPropertyID = tempID;
        }
    }



    public SortedDictionary<string, ItemData> GetAllInstances()
    {
        SortedDictionary<string, ItemData> a = new SortedDictionary<string, ItemData>();

        List<ItemData> productList = new List<ItemData>();
        if (orderItemMethod == OrderItemMethod.ID)
        {
            productList = itemDataBase.GetItemsFromDataBase().OrderByDescending(kp => Convert.ToInt32(kp.ID))
                                      .Select(kp => kp)
                                      .ToList();
        }
        else
        {
            productList = itemDataBase.GetItemsFromDataBase().OrderByDescending(kp => kp.name)
                          .Select(kp => kp)
                          .ToList();
        }
        for (int i = 0; i < productList.Count; i++)
        {
            a.Add(productList[i].ID, productList[i]);
        }
        return a;
    }



    public SortedDictionary<string, ItemData> GetSpecificInstances()
    {
        SortedDictionary<string, ItemData> a = new SortedDictionary<string, ItemData>();

        List<ItemData> productList = new List<ItemData>();
        if (orderItemMethod == OrderItemMethod.ID)
        {
            productList = itemDataBase.GetItemsFromDataBase().OrderByDescending(kp => kp.ID)
                                      .Select(kp => kp)
                                      .ToList();
        }
        else
        {
            productList = itemDataBase.GetItemsFromDataBase().OrderByDescending(kp => kp.name)
                          .Select(kp => kp)
                          .ToList();
        }
      

        for (int i = 0; i < productList.Count; i++)
        {

            if (itemSearchType == ItemType.Null)
            {
                if (productList[i].itemType == ItemType.Null)
                {
                    a.Add(productList[i].ID, productList[i]);
                }
            }
            else
            {
                if ((productList[i].itemType & itemSearchType) != 0 && (productList[i].itemType != ItemType.Null))
                {
                    a.Add(productList[i].ID, productList[i]);
                }
            }
        }
        return a;
    }
}

