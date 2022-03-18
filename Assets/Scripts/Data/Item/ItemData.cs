using System;
using UnityEngine;

//[CustomEditor(typeof(/*ClassYouAreBuildingAnEditorFor*/))]
[System.Serializable]
public class ItemData : OriginData
{

    public int star = 1;
    //public Sprite image;
    //itemID =                           //A     아이템 고유번호
    public string imageID;               //B     아이템 이미지 번호
    public ItemType itemType;            //C     아이템 타입
    //ItemName                           //D     아이템 이름
    public int price = 0;                //E     아이템 가격
    public int pocketMaxCount;           //F     아이템 퀵슬롯 맥스카운트
    public int dropRate;                 //G     아이템 드롭 확률
    public int distanceTile;             //H     아이템 사거리
    public int effectRangeTile;          //I     아이템 이펙트 사거리
    public int DelayTurn;                //J     아이템 쿨타임
    public int activePoint;              //K     아이템 액티브 포인트
    public EffectiveType effectiveType;  //L     아이템 효과 타입
    public int number;                   //M     아이템 효과 양(회복타입 아이템 옅은 그림자 HP 3회복//효과타입 아이템 확실한 그림자 명중률 10 증가)
    public int durationTurn;             //N     아이템 효과 지속시간
    public int inventoryMaxCount;        //O     아이템 인벤토리 맥스 카운트
    public TileWhoStay targetTile;       //P     아이템 타겟
    public string description;           //Q     아이템 설명   
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

