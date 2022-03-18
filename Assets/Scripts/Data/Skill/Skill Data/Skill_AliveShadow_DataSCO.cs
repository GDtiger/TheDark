using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "AliveShadow", menuName = "Data/Skill/AliveShadow", order = 1)]
public class Skill_AliveShadow_DataSCO : SkillData
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


        Debug.Log(node.index);


        effectPool.RequestEffect(effectIDTrigger, node.pos, Quaternion.identity);
        if (node.enemyWatch)
        {
            var markController = GraphicManager.Instance.LeaveTargetMark(node);
            foreach (var enemy in node.enemyWhoWatchingThisNode)
            {
                //어그로가 안끌린 적들만 해당
                if (enemy.battleMode == false)
                {
                    markController.AddEnemy(enemy);
                    enemy.MissingTarget(node);
                }
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