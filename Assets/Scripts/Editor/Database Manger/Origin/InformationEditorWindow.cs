using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class InformationEditorWindow<T> : EditorWindow where T : InformationOrigin
{
    protected SerializedObject serializedObject;
    protected SerializedProperty serializedProperty;

    protected SortedDictionary<int, T> data;
    protected string selectedPropertyPach;
    protected string selectedProperty;
    protected int selectedPropertyID = -1;

    Vector2 scrollPos;
    public bool showSideBar;
    public int category1;
    public bool specificCategory1;

    private void OnGUI()
    {
   
        showSideBar = EditorGUILayout.Toggle("Show Side Bar", showSideBar);
        //if (specificCategory1)
        //{
        //    data = GetAllInstances();
        //}
        //else
        //{
        //    data = GetSpecificInstances();
        //}
        data = GetSpecificInstances();
        if (showSideBar)
        {

  

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(200), GUILayout.ExpandHeight(true));
            var sideBarHeightSize = DrawOnLeftSideBar(ref category1);





            //좌측 사이드바
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MaxWidth(200), GUILayout.ExpandHeight(true));
            //EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
            DrawSliderBar(data);

            //EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        }




        if (selectedPropertyID >= 0)
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

    protected virtual float DrawOnLeftSideBar(ref int category1) {
        return 0;
    }

    void DrawInformation() {


        foreach (var item in data)
        {
            if (data[item.Key].ID == selectedPropertyID)
            {
                if (GUILayout.Button("Delete"))
                {
                    string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
                    SortedDictionary<int, T> a = new SortedDictionary<int, T>();
    
                    for (int i = 0; i < guids.Length; i++)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                        var temp = AssetDatabase.LoadAssetAtPath<T>(path);



                        if (temp.ID == item.Value.ID)
                        {


                            AssetDatabase.DeleteAsset(path);
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                            return;
                        }
                    }
                }

                serializedObject = new SerializedObject(data[item.Key]);
                serializedProperty = serializedObject.GetIterator();
                serializedProperty.NextVisible(true);
                DrawProperties(serializedProperty);
                Apply();
            }
        }
    }


    public virtual bool CorrectWithCategory(T type) {
        return false;
    }

    protected void DrawProperties(SerializedProperty p)
    {

        while (p.NextVisible(false))
        {
            EditorGUILayout.PropertyField(p, true);

        }


    }



    public virtual void DrawSliderBar(SortedDictionary<int, T> prop)
    {
        int tempID = -1;

        foreach (var p in prop)
        {
            if (GUILayout.Button(p.Value.name))
            {
                tempID = p.Value.ID;
            }
        }

        if (tempID >= 0)
        {
            selectedPropertyID = tempID;
        }

        //if (GUILayout.Button("new hero"))
        //{
        //    HeroInfomation newHero = CreateInstance<HeroInfomation>();
        //    CreateNewHero newHeroWindow = GetWindow<CreateNewHero>("New Data");
        //    newHeroWindow.newHero = newHero;

        //}
    }

    protected void Apply()
    {
        serializedObject.ApplyModifiedProperties();
    }

    public static SortedDictionary<int, T> GetAllInstances()
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        SortedDictionary<int, T> a = new SortedDictionary<int, T>();

        

        for (int i = 0; i < guids.Length; i++)
        {

            //Debug.Log(guids[i]);

            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            var temp = AssetDatabase.LoadAssetAtPath<T>(path);
            //var fileName = path.Split("/");


            if (temp.Name != temp.name)
            {
                temp.Name = temp.name;
            }
            if (a.ContainsKey(temp.ID))
            {
                Debug.LogError($"{temp.name}의 id는 이미 존재합니다 다른값으로 변경해주세요 {temp.ID}");
                a.Clear();
                return a;
            }
            else
            {
                a.Add(temp.ID, temp);
            }

        }
        List<T> productList = a.OrderByDescending(kp => kp.Key)
                                      .Select(kp => kp.Value)
                                      .ToList();
        a.Clear();
        for (int i = 0; i < productList.Count; i++)
        {
            a.Add(productList[i].ID, productList[i]);
        }
        return a;
    }

    public SortedDictionary<int, T> GetSpecificInstances()
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        SortedDictionary<int, T> a = new SortedDictionary<int, T>();



        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            var temp = AssetDatabase.LoadAssetAtPath<T>(path);
            if (temp.Name != temp.name)
            {
                temp.Name = temp.name;
            }
            if (a.ContainsKey(temp.ID))
            {
                Debug.LogError($"{temp.name}의 id는 이미 존재합니다 다른값으로 변경해주세요 {temp.ID}");
                a.Clear();
                return a;
            }
            else
            {
                if (CorrectWithCategory(temp))
                {
                    a.Add(temp.ID, temp);
                }
          
            }

        }
        List<T> productList = a.OrderByDescending(kp => kp.Key)
                                      .Select(kp => kp.Value)
                                      .ToList();
        a.Clear();
        for (int i = 0; i < productList.Count; i++)
        {
            a.Add(productList[i].ID, productList[i]);
        }
        return a;
    }
}

