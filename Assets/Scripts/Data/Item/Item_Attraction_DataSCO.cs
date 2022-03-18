using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Attraction", menuName = "Data/Item/Attraction", order = 1)]
public class Item_Attraction_DataSCO : SkillData
{
    
    public override void SkillEnter(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillEnter(player, enemyController, node);
        var newPos = node.pos;
        newPos.y = player.transform.position.y;

        player.transform.DOLookAt(newPos, 0.5f).OnComplete(
    () => { player.animator.SetInteger("TransitionIndex", (int)animationStart); }
    );
    }
    public override void SkillUpdate(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillUpdate(player, enemyController, node);
    }
    public override void SkillTrigger(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillTrigger(player, enemyController, node);
        effectPool.RequestEffect(effectIDTrigger, node.pos, Quaternion.identity);
        var markController = GraphicManager.Instance.LeaveTargetMark(node);

        foreach (var enemy in GridManager.Instance.GetAllNodesInRange(node, distance))
        {
            if (enemy.Value.Value.tileWhoStay == TileWhoStay.Enemy && enemy.Value.Value.whoStandEnemy.battleMode == false)
            {
                markController.AddEnemy(enemy.Value.Value.whoStandEnemy);
                enemy.Value.Value.whoStandEnemy.MissingTarget(node);
            }
        }
        SkillExit(player, enemyController, node);
    }
    public override void SkillExit(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillExit(player, enemyController, node);
        player.animator.SetInteger("TransitionIndex", (int)animationEnd);
    }
}