


using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerUnitStatus
{
    public int playerID;
    public int hp;
    public int maxHP;
    public int mana;
    public int damage;
    public float criticalChance;
    public int maxMoveRange;
    public int attackRange;
    public int attackNoiseRange;
    public int skillPoint;
    public int curSkillPoint;
    public bool ableToUseItem;

    //public SkillData normalAttack;
    public string normalAttackID;

    //public List<int> skillIndex;
    public List<ItemData> pocket;
    //public List<SkillData> skillInfo;
    public List<string> skills;
    public List<int> skillCoolTimeRemain;
    public List<int> itemCoolTimeRemain;

    public bool AbleToUseSkill(int skillIndex)
    {
        var skillID = skills[skillIndex];
        Debug.Log($"{skillCoolTimeRemain[skillIndex]} + {GameManager.Instance.GetSkillDataFromDB(skillID).cost} + {mana} + {TurnManager.Instance.curPlayerUnits[GameManager.Instance.currentPlayerID].actionPoint}");
        return skillCoolTimeRemain[skillIndex] == 0
            && GameManager.Instance.GetSkillDataFromDB(skillID).cost <= mana
            && TurnManager.Instance.curPlayerUnits[GameManager.Instance.currentPlayerID].actionPoint > 0
            ? true : false;
    }

    public bool AbleToUseItem(int itemIndex)
    {
        return itemCoolTimeRemain[itemIndex] == 0
            && ableToUseItem
            && TurnManager.Instance.curPlayerUnits[GameManager.Instance.currentPlayerID].actionPoint > 0
            ? true : false;
    }

    public void SetFullHP()
    {
        hp = maxHP;
        UIManager.Instance.receiveStatusHP(hp, maxHP);
    }

    public void GetMana(int _mana)
    {
        Debug.Log($"{GameManager.Instance.maxMana} < {mana} + {_mana}");

        if (GameManager.Instance.maxMana < mana + _mana)
        {

            mana = GameManager.Instance.maxMana;
        }
        else
        {
            mana += _mana;
        }
        Debug.Log($"{mana}");
    }

    public void UseSkill(int skillIndex)
    {
        var skillInfo = GameManager.Instance.GetSkillDataFromDB(skills[skillIndex]);
        skillCoolTimeRemain[skillIndex] = skillInfo.delay;
        mana -= skillInfo.cost;

        UIManager.Instance.SetActionPoint(mana, GameManager.Instance.maxMana);

        Debug.Log($"쿨타임 {skillCoolTimeRemain[skillIndex]}");
    }

    public void UseItem(int skillIndex)
    {
        var skillInfo = pocket[skillIndex];
        ableToUseItem = false;
        itemCoolTimeRemain[skillIndex] = skillInfo.DelayTurn;
        if (pocket[skillIndex].count > 1)
        {
            pocket[skillIndex].count--;
        }
        else
        {
            pocket.RemoveAt(skillIndex);

        }
        Debug.Log($"쿨타임 {itemCoolTimeRemain[skillIndex]}");
    }


    public void TriggerAfterLoad(SkillDataBase skillDataBase)
    {
        //pocket = new List<ItemData>();
        //skillInfo = new List<SkillData>();
    }

    public void InitiateGame()
    {

        //스킬 쿨타임 셋팅
        skillCoolTimeRemain = new List<int>();
        SetFullHP();
        mana = 5;
        for (int i = 0; i < GameManager.Instance.maxQuickSlotSkillSize; i++)
        {
            skillCoolTimeRemain.Add(0);
        }

        for (int i = 0; i < GameManager.Instance.maxItemPocketSize; i++)
        {
            itemCoolTimeRemain.Add(0);
        }
    }

    public void InitiateTurn()
    {

        for (int i = 0; i < GameManager.Instance.maxQuickSlotSkillSize; i++)
        {
            if (skillCoolTimeRemain[i] > 0)
            {
                skillCoolTimeRemain[i]--;
            }
            else if (skillCoolTimeRemain[i] == 0)
            {
                //아무것도 하지않음
            }
        }

        for (int i = 0; i < GameManager.Instance.maxItemPocketSize; i++)
        {
            if (itemCoolTimeRemain[i] > 0)
            {
                itemCoolTimeRemain[i]--;
            }
            else if (itemCoolTimeRemain[i] == 0)
            {
                //아무것도 하지않음
            }
        }
        ableToUseItem = true;
    }
}