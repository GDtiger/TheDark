using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CreateNewDataWindow<T> : EditorWindow where T : InformationOrigin
{
    protected SerializedObject serializedObject;
    protected SerializedProperty serializedProperty;

    protected T[] data;

    public string path = "Assets/Scripts/hero";
    public string defaultName = "hero";
    public T newData;

    private void OnGUI()
    {

        serializedObject = new SerializedObject(newData);
        serializedProperty = serializedObject.GetIterator();
        serializedProperty.NextVisible(true);
        DrawProperties(serializedProperty);
        if (GUILayout.Button("save"))
        {
            data = GetAllInstances();
            if (string.IsNullOrEmpty(newData.Name))
            {
                newData.Name = defaultName + (data.Length + 1);
            }

            CreateFolderIfNotExisi();

            AssetDatabase.CreateAsset(newData, path + "/" + defaultName  + (data.Length + 1) + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Close();
        }

        Apply();
    }

    private void CreateFolderIfNotExisi()
    {
        if (!AssetDatabase.IsValidFolder($"Assets/SCO"))
        {
            AssetDatabase.CreateFolder("Assets", "SCO");
        }
        if (!AssetDatabase.IsValidFolder($"Assets/SCO/Data"))
        {
            AssetDatabase.CreateFolder("Assets/SCO", "Data");
        }
        if (!AssetDatabase.IsValidFolder(path))
        {
            var temp = path.Split("/");
            AssetDatabase.CreateFolder("Assets/SCO/Data", temp[temp.Length - 1]);
        }
    }

    public void Initiate(T info, string _defaultName, string _path) {
        newData = info;
        defaultName = _defaultName;
        path = _path;

        int index = 0;
        var temp = InformationEditorWindow<T>.GetAllInstances();

        Debug.Log("생성");

        //foreach (var item in temp)
        //{

        //    Debug.Log($"{item.Key}");

        //}

        while (temp.ContainsKey(index))
        {

            index++;
        }
        info.ID = index;
    }

    protected void DrawProperties(SerializedProperty p)
    {

        while (p.NextVisible(false))
        {
            EditorGUILayout.PropertyField(p, true);

        }
    }


    public static T[] GetAllInstances()
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        return a;

    }

    protected void Apply()
    {
        serializedObject.ApplyModifiedProperties();
    }
}

