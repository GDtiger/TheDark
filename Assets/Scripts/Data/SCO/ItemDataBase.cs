using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;


[CreateAssetMenu(fileName = "ItemDataBase", menuName = "Database/Item", order = 1)]
public class ItemDataBase : OriginDataBase
{
	[SerializeField] List<ItemData> items;
	public List<Color> backGroundColor;
	public List<Sprite> backGroundSlotImage;
	public ItemData tempItemData;
	public int ItemCount { get { return items.Count; } }
	[Button]
	public void ImportCSV()
	{
		items.Clear();
		List<Dictionary<string, string>> data = CSVReader.Read("ItemDataBase");
		for (var i = 0; i < data.Count; i++)
		{
			ItemData item = new ItemData();

			item.ID = data[i]["ID"];
			item.name = data[i]["ItemName"];
			item.price = int.Parse(data[i]["Price"]);
			item.pocketMaxCount = int.Parse(data[i]["PocketMaxCount"]);
			item.dropRate = int.Parse(data[i]["DropRate_Percent"]);
			item.distanceTile = int.Parse(data[i]["Distance_Tile"]);
			item.effectRangeTile = int.Parse(data[i]["EffectRange_Tile"]);
			item.DelayTurn = int.Parse(data[i]["Delay_Turn"]);
			item.activePoint = int.Parse(data[i]["ActivePoint"]);
			item.number = int.Parse(data[i]["Number"]);
			item.durationTurn = int.Parse(data[i]["Duration_Turn"]);
			item.inventoryMaxCount = int.Parse(data[i]["InventoryMaxCount"]);
			item.imageID = data[i]["ImageID"];
			item.description = data[i]["Description"];
			item.isnullItem = false;
			switch (data[i]["Type"])
			{
				case "Null":
					item.itemType = ItemType.Null;
					break;
				case "Heal":
					item.itemType = ItemType.Heal;
					break;
				case "Attraction":
					item.itemType = ItemType.Attraction;
					break;
				case "Buff":
					item.itemType = ItemType.Buff;
					break;
				case "Trap":
					item.itemType = ItemType.Trap;
					break;
				case "Debuff":
					item.itemType = ItemType.Debuff;
					break;
			};
			switch (data[i]["EffectiveType"])
			{
				case "HP":
					item.effectiveType = EffectiveType.HP;
					break;
				case "EnemyAttraction":
					item.effectiveType = EffectiveType.EnemyAttraction;
					break;
				case "Damage":
					item.effectiveType = EffectiveType.Damage;
					break;
				case "Accurate":
					item.effectiveType = EffectiveType.Accurate;
					break;
				case "Move":
					item.effectiveType = EffectiveType.Move;
					break;
				case "Vision":
					item.effectiveType = EffectiveType.Vision;
					break;
				case "Turn":
					item.effectiveType = EffectiveType.Turn;
					break;
				case "Debuff":
					item.effectiveType = EffectiveType.Debuff;
					break;
			};
			switch (data[i]["Target"])
			{
				case "Player":
					item.targetTile = TileWhoStay.Player;
					break;
				case "Enemy":
					item.targetTile = TileWhoStay.Enemy;
					break;
				case "Tile":
					item.targetTile = TileWhoStay.None;
					break;
			}
			items.Add(item);
			Debug.Log((int)item.itemType);
		}
	}

	//[Button]
	//public void ExportCSV()
	//{
	//	var rowData = new string[items.Count + 1][];
	//	string filePath = @"Assets\Resources\TestCSV2.csv";
	//	File.Delete(filePath);
	//	string[] rowDataTemp = new string[13];
	//	rowDataTemp[0] = "ID";
	//	rowDataTemp[1] = "ImageID";
	//	rowDataTemp[2] = "Name";
	//	rowDataTemp[3] = "HP";
	//	rowDataTemp[4] = "HPLeveling";
	//	rowDataTemp[5] = "Def";
	//	rowDataTemp[6] = "DefLeveling";
	//	rowDataTemp[7] = "ElementMastery";
	//	rowDataTemp[8] = "ElementMasteryLeveling";
	//	rowDataTemp[9] = "ElementResistance";
	//	rowDataTemp[10] = "ElementResistanceLeveling";
	//	rowDataTemp[11] = "ItemLargeType";
	//	rowDataTemp[12] = "ItemSmallType";
	//	rowData[0] = rowDataTemp;

	//	StreamWriter outStream = new StreamWriter(filePath, true, Encoding.UTF8);
	//	outStream.WriteLine(string.Join(",", rowData[0]));

	//	for (int i = 0; i < items.Count; i++)
	//	{
	//		var temp = new string[12];
	//		temp[0] = items[i].ID;
	//		temp[1] = items[i].imageID;
	//		temp[2] = items[i].name;
	//		temp[3] = Convert.ToString(items[i].pocketMaxCount);
	//		temp[4] = Convert.ToString(items[i].distanceTile);
	//		temp[5] = Convert.ToString(items[i].Number);
	//		temp[6] = Convert.ToString(items[i].defLeveling);
	//		temp[7] = Convert.ToString(items[i].elementMastery);
	//		temp[8] = Convert.ToString(items[i].elementMasteryLeveling);
	//		temp[9] = Convert.ToString(items[i].elementResistance);
	//		temp[10] = Convert.ToString(items[i].elementResistanceLeveling);
	//		temp[11] = Convert.ToString(items[i].itemType).Replace(" ","");		//type = Wepon, Two_Hand_Sword
	//		rowData[i + 1] = temp;
	//		outStream.WriteLine(string.Join(",", temp));
	//	};
	//	outStream.Close();
	//}
	public ItemData GetItemFromDataBase(string id)
	{
        for (int i = 0; i < items.Count; i++)
        {
			if (id == items[i].ID)
            {
				return items[i].DeepCopy();
			}
        }
		return null;
	}

	public ItemData GetItemFromDataBase(int index)
	{
		return items[index].DeepCopy();
	}

	public List<ItemData> GetItemsFromDataBase()
	{
		return items;
	}
	public void CreateNewItem(ItemData _itemData)
	{
		items.Add(_itemData);
	}

	//public ItemData GetItem(ItemID itemID)
	//{
	//    return items[itemID.ToString()];
	//}

	//public Color GetItemColor(ItemID itemID)
	//{
	//    return backGroundColor[items[itemID.ToString()].star];
	//}

	//public ItemData DrawItem(ItemType itemType)
	//{
	//    var temp = items.Values.Where(x => x.itemLargeCategory == itemType)
	//        .Select(x => x).ToList();
	//    return temp[Random.Range(0, temp.Count - 1)];
	//}
	//public ItemInfo CreateNewItem(ItemID itemID)
	//{
	//    Debug.Log($"새 아이템 ({itemID})을 만들기 시작했습니다.");
	//    ItemInfo newItem = new ItemInfo();
	//    var itemData = items[itemID];
	//    newItem.star = items[itemID].star;
	//    newItem.level = 1;
	//    newItem.exp = 0;
	//    newItem.itemID = itemID;
	//    newItem.textID = items[itemID].textID;
	//    newItem.itemImage = itemData.imageID;
	//    newItem.itemProtrait = itemData.image_ProfileID;
	//    newItem.itemSmallCategory = itemData.itemSmallCategory;
	//    newItem.itemLargeCategory = itemData.itemLargeCategory;

	//    ItemType itemType = items[itemID].itemSmallCategory;



	//    //아이템 타입이 무기 종류 일때
	//    if (10000 < (int)itemType && (int)itemType < 20000)
	//    {
	//        newItem.itemMainStatus = MainStatusType.Status_Main_Damage;
	//        newItem.mainStatus = items[itemID].damage;
	//    }
	//    //아이템 타입이 장신구 종류 일때
	//    else if (20000 < (int)itemType)
	//    {
	//        //int rand = Random.Range(1, System.Enum.GetValues(typeof(MainStatusType)).Length);

	//        //Debug.Log($"주능력치 랜덤값 : {rand} {2 >> rand} {(StatusType)(2 >> rand)}");

	//        switch (newItem.itemSmallCategory)
	//        {
	//            case ItemType.Item_Type_Flower:
	//                newItem.itemMainStatus = MainStatusType.Status_Main_Health_Point;
	//                newItem.mainStatus = items[itemID].hp;
	//                break;
	//            case ItemType.Item_Type_Goblet:
	//                newItem.itemMainStatus = MainStatusType.Status_Main_Damage;
	//                newItem.mainStatus = items[itemID].damage;
	//                break;
	//            case ItemType.Item_Type_Plume:
	//                newItem.itemMainStatus = MainStatusType.Status_Main_Defence;
	//                newItem.mainStatus = items[itemID].def;
	//                break;
	//            case ItemType.Item_Type_Sands:
	//                newItem.itemMainStatus = MainStatusType.Status_Main_Elemental_Mastery;
	//                newItem.mainStatus = items[itemID].elementMastery;
	//                break;
	//            case ItemType.Item_Type_Circlet:
	//                newItem.itemMainStatus = MainStatusType.Status_Main_Elemental_Resistance;
	//                newItem.mainStatus = items[itemID].elementResistance;
	//                break;
	//            default:
	//                break;
	//        }

	//        //newItem.itemMainStatus = (MainStatusType)(rand);
	//        //switch (newItem.itemMainStatus)
	//        //{

	//        //    case MainStatusType.Status_Main_Elemental_Mastery:
	//        //        newItem.mainStatus = items[itemID].elementMastery;
	//        //        break;
	//        //    case MainStatusType.Status_Main_Health_Point:
	//        //        newItem.mainStatus = items[itemID].hp;
	//        //        break;
	//        //    case MainStatusType.Status_Main_Elemental_Resistance:
	//        //        newItem.mainStatus = items[itemID].elementResistance;
	//        //        break;
	//        //    case MainStatusType.Status_Main_Defence:
	//        //        newItem.mainStatus = items[itemID].def;
	//        //        break;
	//        //}
	//    }







	//    //보조 능력 랜덤 설정
	//    //int var = System.Enum.GetValues(typeof(SubStatusType)).Length;
	//    newItem.itemSubStatus = (SubStatusType)Random.Range(0, System.Enum.GetValues(typeof(SubStatusType)).Length);

	//    switch (newItem.itemSubStatus)
	//    {
	//        case SubStatusType.Status_Sub_Physical_DMG_Bonus_Rate:
	//            newItem.subStatus = Random.Range(subStatDmg.x, subStatDmg.y) * (newItem.star * subStatDmg.z + 1);
	//            break;
	//        case SubStatusType.Status_Sub_HP_Rate:
	//            newItem.subStatus = Random.Range(subStatHP.x, subStatHP.y) * (newItem.star * subStatHP.z + 1);
	//            break;
	//        case SubStatusType.Status_Sub_Elemental_Mastery_Rate:
	//            newItem.subStatus = Random.Range(subStatElementMastery.x, subStatElementMastery.y) * (newItem.star * subStatElementMastery.z + 1);
	//            break;
	//        case SubStatusType.Status_Sub_Crit_DMG_Rate:
	//            newItem.subStatus = Random.Range(subStatCriDmg.x, subStatCriDmg.y) * (newItem.star * subStatCriDmg.z + 1);
	//            break;
	//        case SubStatusType.Status_Sub_Crit_Rate:
	//            newItem.subStatus = Random.Range(subStatCriChance.x, subStatCriChance.y) * (newItem.star * subStatCriChance.z + 1);
	//            break;
	//        case SubStatusType.Status_Sub_Def_Rate:
	//            newItem.subStatus = Random.Range(subStatDef.x, subStatDef.y) * (newItem.star * subStatDef.z + 1);
	//            break;
	//        case SubStatusType.Status_Sub_Energy_Recharge_Rate:
	//            newItem.subStatus = Random.Range(subStatElementRecharge.x, subStatElementRecharge.y) * (newItem.star * subStatElementRecharge.z + 1);
	//            break;
	//        default:
	//            break;
	//    }
	//    Debug.Log($"새 아이템 ({itemID})을 만들어졌습니다.");
	//    return newItem;
	//}

	//public void LevelUpItem(ref ItemInfo itemInfo)
	//{
	//    LevelUpItem(ref itemInfo, 1);
	//}

	//public void LevelUpItem(ref ItemInfo itemInfo, int level)
	//{
	//    var itemData = items[itemInfo.itemID];
	//    itemInfo.level += level;

	//    switch (itemInfo.itemMainStatus)
	//    {
	//        case MainStatusType.Status_Main_Damage:
	//            itemInfo.mainStatus = itemData.damage + itemData.damageLeveling * itemInfo.level;
	//            break;
	//        case MainStatusType.Status_Main_Elemental_Mastery:
	//            itemInfo.mainStatus = itemData.elementMastery + itemData.elementMasteryLeveling * itemInfo.level;
	//            break;
	//        case MainStatusType.Status_Main_Health_Point:
	//            itemInfo.mainStatus = itemData.hp + itemData.hpLeveling * itemInfo.level;
	//            break;
	//        case MainStatusType.Status_Main_Elemental_Resistance:
	//            itemInfo.mainStatus = itemData.elementResistance + itemData.elementResistanceLeveling * itemInfo.level;
	//            break;
	//        case MainStatusType.Status_Main_Defence:
	//            itemInfo.mainStatus = itemData.def + itemData.defLeveling * itemInfo.level;
	//            break;
	//    }
	//}


}