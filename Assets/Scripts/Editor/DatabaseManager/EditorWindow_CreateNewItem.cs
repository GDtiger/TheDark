using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class EditorWindow_CreateNewItem : EditorWindow
{
    protected SerializedObject serializedObject;
    protected SerializedProperty serializedProperty;

    //protected T[] data;

    public string path = "Assets/Scripts/hero";
    public string defaultName = "hero";


    public ItemDataBase itemDataBase;
    EditorWindow_ItemDataEditor editorWindow_ItemDataEditor;
    //ItemData newItemData;

    ItemDataBase newDatabase;
    private void OnGUI()
    {
        serializedObject = new SerializedObject(newDatabase);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tempItemData"));
        if (serializedObject.ApplyModifiedProperties())
        {
            //var temp = newDatabase.GetItemDataBase();
            //for (int i = 0; i < temp.Count; i++)
            //{
            //    if (temp[i].ID == newDatabase.itemData.ID)
            //    {
            //        temp[i] = newDatabase.itemData;
            //    }
            //}
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            itemDataBase.CreateNewItem(newDatabase.tempItemData);
            Close();
        }

        if (GUILayout.Button("Cancle"))
        {
            Close();
        }
        EditorGUILayout.EndVertical();
    }

    public void Initiate(EditorWindow_ItemDataEditor _editorWindow_ItemDataEditor,   ItemDataBase _itemDataBase) {
        newDatabase = new ItemDataBase();
        itemDataBase = _itemDataBase;
        editorWindow_ItemDataEditor = _editorWindow_ItemDataEditor;
        newDatabase.tempItemData = new ItemData();
        //defaultName = _defaultName;
        //path = _path;

        int index = 0;
        var temp = _editorWindow_ItemDataEditor.GetAllInstances();

        //Debug.Log("생성");

        //foreach (var item in temp)
        //{

        //    Debug.Log($"{item.Key}");

        //}

        while (temp.ContainsKey(index.ToString()))
        {

            index++;
        }
        newDatabase.tempItemData.ID = index.ToString();
    }




}

