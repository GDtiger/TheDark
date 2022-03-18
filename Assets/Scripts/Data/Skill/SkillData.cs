using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillData : ScriptableObject
{
    public bool item;
    public ItemData itemData;
    public ItemDataBase itemDataBase;


    public string ID;
    public string skillName;
    public int skillLevel;
    public Sprite sprite;
    public EffectPoolSCO effectPool;
    public SkillCategory skillCategory;
    public SkillType skillType;
    public string description;
    public SKillID skillAnimationID;
    public float endTime;

    public float damagePercent;
    //public int delay;
    //public int sound;
    //public int distance;
    public bool ignoreObstacle = false;

    //새로운 스킬 데이터
    public SkillFeature feature;        //E-Feature
    public int activePoint;             //F-ActivePoint
    public TileWhoStay targetTile;      //G-Target;
    public int damage;                //H-Damage(Before - DamagePercent)
    public float tickDamage;            //I-TickDamage
    public int distance;                //J-
    public int range;                   //K-
    public int delay;                   //L-
    public int sound;                   //M-
    public int accurate;                //N-
    public int CC;                      //O-


    public int cost;                    //Q-Cost
    public UnitPositionState afterSkill;//R-
    public AdvType advType1;            //S-
    public int advLevel1;               //T-
    public AdvCondition advCondition1;  //U-
    public int advRate1;                //V-
    public AdvType2 advType2;           //W-
    public int advLevel2;               //X-
    public AdvCondition2 advCondition2; //Y-
    public int advRate2;                //Z-


    [TabGroup("Parameter", "STE")] public AnimeID animationStart = AnimeID.None;
    [TabGroup("Parameter", "STE")] public AnimeID animationTrigger = AnimeID.None;
    [TabGroup("Parameter", "STE")] public AnimeID animationEnd = AnimeID.None;

    [TabGroup("Parameter", "STE")] public EffectID effectIDStart = EffectID.None;
    [TabGroup("Parameter", "STE")] public EffectID effectIDTrigger = EffectID.None;
    [TabGroup("Parameter", "STE")] public EffectID effectIDEnd = EffectID.None;

    public string soundID;

    public bool customEndTime;
    public float durationTime = 5;      //P-

    //조건에 맞는 사용여부
    //public bool ableToUseBattleMode;
    public bool ableToUseOnShadowTile;


    public int[] effectRange;



    public bool isNullSkill = true;


   [TabGroup("Parameter", "MultiHitSkill")] public bool multiHitSkill;
   [TabGroup("Parameter", "MultiHitSkill")] public MultiHitSkillData[] multiHit;
   [TabGroup("Refer", "MultiHitSkill")] public int index;
   [TabGroup("Refer", "MultiHitSkill")] public float skillTime;

    public bool CheckAbleToUseSkill(PlayerController playerController) {
        //배틀모드에서만쓸수있는 스킬이지만 배틀모드가 아닐때 리턴
        //if (ableToUseBattleMode && playerController.battleMode == false) return false;


        //그림자타일에서만 쓸수있는 스킬이지만 그림자위가 아닐때 리턴
        if (ableToUseOnShadowTile && GridManager.Instance.GetNode(playerController.transform.position).nodeInShadow == false) return false;
        return true;
    }
    public virtual void SkillEnter(PlayerController player, EnemyController enemyController, Node node) {
        if (item)
        {
            Debug.Log($"{itemData.name} Enter");
        }
        else
        {
            Debug.Log($"{skillName} Enter");
        }
        //SoundManager.Instance.PlaySFXSound(soundID);
        player.ableToAction = false;
        endTime = Time.time + durationTime;

        if (multiHitSkill)
        {
            index = 0;
        }
    }


    public virtual void SkillUpdate(PlayerController player, EnemyController enemyController, Node node)
    {
        //Debug.Log($"{skillName} Update {player.transform.position}");
        
        if (customEndTime && endTime < Time.time)
        {
            SkillExit(player, enemyController, node);
        }
    }

    public virtual void SkillTrigger(PlayerController player, EnemyController enemyController, Node node) {


        if (item)
        {
            Debug.Log($"{itemData.name} Trigger");
        }
        else
        {
            Debug.Log($"{skillName} Trigger");
        }
        if (targetTile == TileWhoStay.Enemy && !enemyController.battleMode)
        {
            enemyController.FoundPlayer(player);
		}
		else
		{
            switch (targetTile)
            {
                case TileWhoStay.None:
                    player.CallInSoundRange(node, sound);
                    break;
                case TileWhoStay.Enemy:
                    player.CallInSoundRange(enemyController.onTile, sound);
                    break;
                case TileWhoStay.Player:
                    player.CallInSoundRange(player.onTile, sound);
                    break;
                case TileWhoStay.Trap:
                    break;
                default:
                    break;
            }
        }
	

    }


    public virtual void SkillExit(PlayerController player, EnemyController enemyController, Node node)
    {
        if (item)
        {
            Debug.Log($"{itemData.name} Exit");
        }
        else
        {
            Debug.Log($"{skillName} Exit");
        }
        player.state = UnitTurnState.EndProcessing;
    }

    [Button]
    public void GetItemDataFromItemDataBase() {
        itemData = itemDataBase.GetItemFromDataBase(ID);
        distance = itemData.distanceTile;
        range = itemData.effectRangeTile;
        targetTile = itemData.targetTile;
    }

    public void MultiSkillEffect(PlayerController player, EnemyController enemyController, Node node) {
        if (multiHit.Length > index)
        {
            index++;
            SoundManager.Instance.PlaySFXSound(multiHit[index].sound, 1);      //쳐맞을때소리(기본공격소리)
            enemyController.animator.SetInteger("TransitionIndex", (int)AnimeID.Hit);
        }
    }
    public SkillData()
    {
        isNullSkill = true;
        ID = "-1";

    }
}
