using System;
using UnityEngine;

//[CustomEditor(typeof(/*ClassYouAreBuildingAnEditorFor*/))]
[System.Serializable]
public class ItemData : OriginData
{

    public int star = 1;
    //public Sprite image;
    //itemID =                           //A     ������ ������ȣ
    public string imageID;               //B     ������ �̹��� ��ȣ
    public ItemType itemType;            //C     ������ Ÿ��
    //ItemName                           //D     ������ �̸�
    public int price = 0;                //E     ������ ����
    public int pocketMaxCount;           //F     ������ ������ �ƽ�ī��Ʈ
    public int dropRate;                 //G     ������ ��� Ȯ��
    public int distanceTile;             //H     ������ ��Ÿ�
    public int effectRangeTile;          //I     ������ ����Ʈ ��Ÿ�
    public int DelayTurn;                //J     ������ ��Ÿ��
    public int activePoint;              //K     ������ ��Ƽ�� ����Ʈ
    public EffectiveType effectiveType;  //L     ������ ȿ�� Ÿ��
    public int number;                   //M     ������ ȿ�� ��(ȸ��Ÿ�� ������ ���� �׸��� HP 3ȸ��//ȿ��Ÿ�� ������ Ȯ���� �׸��� ���߷� 10 ����)
    public int durationTurn;             //N     ������ ȿ�� ���ӽð�
    public int inventoryMaxCount;        //O     ������ �κ��丮 �ƽ� ī��Ʈ
    public TileWhoStay targetTile;       //P     ������ Ÿ��
    public string description;           //Q     ������ ����   
    public int count;
    public bool isnullItem = true;



	//public int elementMasteryLeveling;

	//public int elementResistance; //
	//public int elementResistanceLeveling;

	public ItemData()
	{
        isnullItem = true;
        ID = "-1";

    }

	public ItemData DeepCopy()
    {
        ItemData itemData = new ItemData();
        itemData.ID = ID;                           //A
        itemData.itemType = itemType;               //B
        itemData.name = name;                       //C
        itemData.price = price;                     //D
        itemData.pocketMaxCount = pocketMaxCount;               //E
        itemData.dropRate = dropRate;               //F
        itemData.distanceTile = distanceTile;       //G
        itemData.effectRangeTile = effectRangeTile; //H
        itemData.DelayTurn = DelayTurn;             //I
        itemData.activePoint = activePoint;         //J
        itemData.effectiveType = effectiveType;     //K
        itemData.number = number;                   //L
        itemData.durationTurn = durationTurn;   //M
        itemData.inventoryMaxCount = inventoryMaxCount;     //N
        itemData.targetTile = targetTile;                   //O 
        itemData.description = description;

        itemData.imageID = imageID;
        itemData.isnullItem = isnullItem;
        itemData.count = 1;

        return itemData;
    }
}

