using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class SaveData : MonoBehaviour
{
    JsonSave_Load sv = new JsonSave_Load();
    public PlayerUnitStatus unitStatus=null;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {   
            sv.SaveDataToJson();
            Debug.Log("데이터 저장!");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            sv.LoadJsonData();
            Debug.Log("데이터 로드!!");
        }

        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //        var s = unitStatus.hp;
        //        Debug.Log(s);
        //        Debug.Log(unitStatus.mana);
            
        //}
    }
}
