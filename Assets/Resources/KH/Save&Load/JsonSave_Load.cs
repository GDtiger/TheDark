using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;

[System.Serializable]
public class JsonData
{
    public List<PlayerUnitStatus> unitStatus;
    public List<ItemData> inventory;

}

public class JsonSave_Load : MonoBehaviour
{
    //PlayerUnitStatus    playerUnitStatus;
    JsonData jData = new JsonData();

    public void SaveDataToJson()
    {
        
        //var playerindex = FindObjectOfType<PlayerController>().playerID;
        //playerUnitStatus = GameManager.Instance.GetUnitStatus(playerindex);
        jData.unitStatus = GameManager.Instance.unitStatus;
        jData.inventory = GameManager.Instance.inventory;

        string jSaveData = ObjectToJson(jData);
        //string jsonPlayerData = ObjectToJson(playerUnitStatus);
        CreateJsonFile(Application.dataPath + "/Resources/DataJson", "jSaveData", jSaveData);


    }

    ////status
    //public PlayerUnitStatus LoadPlayerStatusData()
    //{
    //   return LoadJsonFile<PlayerUnitStatus>(Application.dataPath + "/Resources/DataJson", "jSaveData");
    //}
    public void LoadJsonData()
    {
        var a = LoadJsonFile<JsonData>(Application.dataPath + "/Resources/DataJson", "jSaveData");
        var playerindex = FindObjectOfType<PlayerController>().playerID;
        Debug.Log(jData.unitStatus[playerindex].hp);
        Debug.Log(a.unitStatus[playerindex].hp);
        Debug.Log(a.unitStatus[playerindex].maxHP);

    }

    string ObjectToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    T JsonToOject<T>(string jsonData)
    {
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    void CreateJsonFile(string createPath, string fileName, string jsonData)
    {
        //system.io
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", createPath, fileName), FileMode.Create);

        //encodeing system.text
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    T LoadJsonFile<T>(string loadPath, string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();

        string jsonData = Encoding.UTF8.GetString(data);
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

}
