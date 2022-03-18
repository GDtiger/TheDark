using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "RestoreHP", menuName = "Data/Item/RestoreHP", order = 1)]
public class Item_RestoreHP_DataSCO : SkillData
{
    
    public override void SkillEnter(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillEnter(player, enemyController, node);
        player.animator.SetInteger("TransitionIndex", (int)animationStart);
    }
    public override void SkillUpdate(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillUpdate(player, enemyController, node);
    }
    public override void SkillTrigger(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillTrigger(player, enemyController, node);
        effectPool.RequestEffect(effectIDTrigger, player.transform.position, Quaternion.identity);
        player.Heal(itemData.number);
        SkillExit(player, enemyController, node);
    }
    public override void SkillExit(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillExit(player, enemyController, node);
        player.animator.SetInteger("TransitionIndex", (int)animationEnd);
    }
}