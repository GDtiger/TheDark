
using UnityEngine;

using UnityEditor;

public class ExampleWindow : EditorWindow
{
    string myString = "Hello, World";
    Color color = new Color(1, 1, 1, 1);

    bool showColorizePanel = false;
    bool showHeroMakerPanel = false;


    protected SerializedObject serializedObject;
    protected SerializedProperty serializedProperty;

    protected string selectedPropertyPach;
    protected string selectedProperty;


    protected HeroInformation[] heroes;

    //Material myString = "Hello, World";

    [MenuItem("Window/Example")]
    public static void ShowWindow() {
        //윈도우 열기
        //string은 윈도우창의 이름
        EditorWindow.GetWindow<ExampleWindow>("Example");
    }

    private void OnGUI()
    {
        // Window Code

        //라벨 보이기
        GUILayout.Label("This is Label", EditorStyles.boldLabel);

        //변수의 이름 변경
        myString = EditorGUILayout.TextField("Name", myString);


        showColorizePanel = EditorGUILayout.Toggle("Show Color", showColorizePanel);
        showHeroMakerPanel = EditorGUILayout.Toggle("Show hero", showHeroMakerPanel);


        GUILayout.Label("");


        if (showColorizePanel)
        {
            Colorize();
        }
        if (showHeroMakerPanel)
        {
            HeroEditor();
        }

        //if (GUILayout.Button("Press Me"))
        //{
        //    Debug.Log("Button Press Me");
        //}

    }

    private void HeroEditor()
    {
        heroes = GetAllInstances<HeroInformation>();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));
        DrawSliderBar(heroes);
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

        if (selectedProperty != null)
        {
            for (int i = 0; i < heroes.Length; i++)
            {
                if (heroes[i].Name == selectedProperty)
                {
                    serializedObject = new SerializedObject(heroes[i]);
                    serializedProperty = serializedObject.GetIterator();
                    serializedProperty.NextVisible(true);
                    DrawProperties(serializedProperty);
                    Apply();
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("Select an Item form list");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
       
    }

    protected void DrawSliderBar(HeroInformation[] allHeroes) {
        foreach (HeroInformation hero in allHeroes)
        {
            if (GUILayout.Button(hero.Name))
            {
                selectedPropertyPach = hero.Name;
            }
        }

        if (!string.IsNullOrEmpty(selectedPropertyPach))
        {
            selectedProperty = selectedPropertyPach;
        }
    }

    protected void DrawProperties(SerializedProperty p) {
        while (p.NextVisible(false))
        {
            EditorGUILayout.PropertyField(p, true);
        }
    }


    //모든 스크립트 테이블 가져와서 보여주기
    public static T[] GetAllInstances<T>() where T : HeroInformation
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

    protected void Apply() {
        serializedObject.ApplyModifiedProperties();
    }

    private void Colorize()
    {
        GUILayout.Label("Color the selected objects!", EditorStyles.boldLabel);

        color = EditorGUILayout.ColorField("Color", color);

        if (GUILayout.Button("Colorize"))
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                var temp = obj.GetComponent<Renderer>();
                if (temp != null)
                {
                    temp.sharedMaterial.color = color;
                }


            }
        }
    }
}
