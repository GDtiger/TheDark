using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(fileName = "ShadowStep", menuName = "Data/Skill/Shadow Step", order = 1)]
public class Skill_ShadowStep_DataSCO : SkillData
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
        //if (player.onTile.enemyWatch && )
        //{

        //}

        Debug.Log($"{skillName} - {player.onTile.index} - {node.index}");
        player.transform.DOMove(node.pos, 0);
        effectPool.RequestEffect(effectIDTrigger, node.pos, Quaternion.identity);
        SkillExit(player, enemyController, node);
    }


    public override void SkillExit(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillExit(player, enemyController, node);
        player.animator.SetInteger("TransitionIndex", (int)animationEnd);
    }
}