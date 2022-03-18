using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class HerosEditorWindow : InformationEditorWindow<HeroInformation>
{


    [MenuItem("Project/Hero Editor")]
    protected static void ShowWindow()
    {
        GetWindow<HerosEditorWindow>("Heros");
    }





    
    public override void DrawSliderBar(SortedDictionary<int, HeroInformation> prop)
    {
        base.DrawSliderBar(prop);
        if (GUILayout.Button("new hero"))
        {
            HeroInformation newHero = CreateInstance<HeroInformation>();
            CreateNewHeroWindow newHeroWindow = GetWindow<CreateNewHeroWindow>("New Hero Maker");
            newHeroWindow.Initiate(newHero, "Hero", "Assets/SCO/Data/Heroes");



        }
    }
}