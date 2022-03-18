using UnityEngine;

[CreateAssetMenu(fileName = "Destruction", menuName = "Data/Skill/Destruction", order = 1)]
public class Skill_Destruction_DataSCO : SkillData
{
    public override void SkillEnter(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillEnter(player, enemyController, node);
        player.animator.SetInteger("TransitionIndex", (int)SKillID.Destruction);
    }
    public override void SkillUpdate(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillUpdate(player, enemyController, node);
    }
    public override void SkillTrigger(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillTrigger(player, enemyController, node);
    }
    public override void SkillExit(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillExit(player, enemyController, node);
    }
}