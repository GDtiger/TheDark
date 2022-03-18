using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;


//자료구조 및 데이터형 보관용 클래스
[System.Serializable]
public class testScript
{
    
    public int i;
    public float f;
    public bool b;
    public string s;
    public int[] iArray;
    public List<int> iList = new List<int>();
    public Dictionary<string, float> fDictionary = new Dictionary<string, float>();

    public testScript() { }

    public testScript(bool isSet)
    {
        if (isSet)
        {
            i = 10;
            f = 99.9f;
            b = true;
            s = "Json Test string";
            iArray = new int[] { 1, 3, 5, 7, 9, 12, 33, 55, 8, 99 };

            for (int idx = 0; idx < 5; idx++)
            {
                iList.Add(2 * idx);
            }
            fDictionary.Add("Pie", Mathf.PI);
            fDictionary.Add("Epsilon", Mathf.Epsilon);
            fDictionary.Add("sqrt(2)", Mathf.Sqrt(2));

        }
    }

    public void print()
    {
        Debug.Log("i = " + i);
        Debug.Log("f = " + f);
        Debug.Log("b = " + b);
        Debug.Log("str = " + s);

        for (int idx = 0; idx < iArray.Length; idx++)
        {
            Debug.Log(string.Format("iArray[{0}] = {1}", idx, iArray[idx]));
        }

        for (int idx = 0; idx < iList.Count; idx++)
        {
            Debug.Log(string.Format("iList[{0}] = {1}", idx, iList[idx]));
        }

        foreach (var data in fDictionary)
        {
            Debug.Log(string.Format("iDictionary[{0}] = {1}", data.Key, data.Value));
        }

    }
}

//변환 스크립트
public class JsonExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        testScript ts = new testScript(true);
        string jsonData = ObjectToJson(ts);
        CreateJsonFile(Application.dataPath+"/Resources/DataJson", "JTestClass", jsonData);

        
        // Debug.Log(jsonData);
        // var ts2 = JsonToOject<testScript>(jsonData);
        // ts2.print();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            
            
            var ts1 = LoadJsonFile<testScript>(Application.dataPath + "/Resources/DataJson", "JTestClass");
            ts1.print();

            var playerindex = FindObjectOfType<PlayerController>().playerID;
            //GameManager.Instance.GetUnitStatus(playerindex);
            Debug.Log(playerindex);
            PlayerUnitStatus playerUnitStatus = GameManager.Instance.GetUnitStatus(playerindex);
            string jsonPlayerData = ObjectToJson(playerUnitStatus);
            CreateJsonFile(Application.dataPath + "/Resources/DataJson", "JPlayerData", jsonPlayerData);
  
        }
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
