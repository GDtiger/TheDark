
//using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "Database/Image", order = 1)]
public class ImageDataBase : ScriptableObject
{
    [SerializeField] List<ImageData> image;

    Dictionary<string, ImageData> imageDic;

    public void Initiate() {
        imageDic = new Dictionary<string, ImageData>();
        for (int i = 0; i < image.Count; i++)
        {
            if (!imageDic.ContainsKey(image[i].name))
            {
                imageDic.Add(image[i].name, image[i]);
                image[i].id = i;
            }
            else
            {

                Debug.LogError($"{i}번째의 이미지는 이미 존재하는 이름입니다 ({image[i].name})");

            }
        }
    }

    public ImageData GetImage(int i) {
        return image[i];
    }

    public ImageData GetImage(string name)
    {
        return imageDic[name];
    }
}

[System.Serializable]
public class ImageData {
    public string name;
    public int id;
    public Sprite image;

    public ImageData(string name, Sprite image)
    {
        this.name = name;
        this.image = image;
    }
}