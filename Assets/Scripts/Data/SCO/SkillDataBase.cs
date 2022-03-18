using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "SkillDataBase", menuName = "Database/Skill", order = 1)]
public class SkillDataBase : OriginDataBase
{
	[SerializeField] List<SkillData> skills;
	[SerializeField] List<SkillData> items;
	[SerializeReference] Dictionary<string, SkillData> skillDic;
	[SerializeReference] Dictionary<string, SkillData> itemDic;

	public void Initiate() {
		skillDic = new Dictionary<string, SkillData>();
		for (int i = 0; i < skills.Count; i++)
		{
			if (!skillDic.ContainsKey(skills[i].ID))
			{
				skillDic.Add(skills[i].ID, skills[i]);
			}
			else
			{

				Debug.LogError($"{i}번째의 이미지는 이미 존재하는 이름입니다 ({skills[i].ID})");

			}
		}
		itemDic = new Dictionary<string, SkillData>();
		for (int i = 0; i < items.Count; i++)
		{
			if (!itemDic.ContainsKey(items[i].ID))
			{
				itemDic.Add(items[i].ID, items[i]);
			}
			else
			{

				Debug.LogError($"{i}번째의 이미지는 이미 존재하는 이름입니다 ({items[i].ID})");

			}
		}
	}


	public SkillData GetSkillDataFromDataBase(int index)
	{
		return skills[index];
	}

	public SkillData GetSkillDataFromDataBase(string id) {
		return skillDic[id];
	}

	public SkillData GetItemDataFromDataBase(string id)
	{
		return itemDic[id];
	}

	[Button]
	public void ImportCSV()
	{

		List<Dictionary<string, string>> data = CSVReader.Read("TestCSV");

 

		for (var i = 0; i < data.Count; i++)
		{
			for (int j = 0; j < skills.Count; j++)
			{
                if (skills[j].ID == data[i]["ID"])
                {
					skills[j].damage = int.Parse( data[i]["damage"]);











					break;
				}

			}
		}
	}
}
