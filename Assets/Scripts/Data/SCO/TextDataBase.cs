using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "TextDataBase", menuName = "Database/Text", order = 1)]
public class TextDataBase : OriginDataBase
{
	[SerializeField]public List<TextData> texts;
	[SerializeReference] Dictionary<string, TextData> textDic;

	public void Initiate() {
		textDic = new Dictionary<string, TextData>();
		for (int i = 0; i < texts.Count; i++)
		{
			if (!textDic.ContainsKey(texts[i].ID))
			{
				textDic.Add(texts[i].ID, texts[i]);
			}
			else
			{

				Debug.LogError($"{i}번째의 이미지는 이미 존재하는 이름입니다 ({texts[i].ID})");

			}
		}
	}

	public int GetDataSize() { return textDic.Count; }
	public TextData GetTextData(string id)
	{
		return textDic[id];
	}
}
