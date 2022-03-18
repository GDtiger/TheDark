using UnityEngine;

[CreateAssetMenu(fileName = "AssassinAttack", menuName = "Data/Skill/AssassinAttack", order = 1)]
public class Skill_AssassinAttack_DataSCO : SkillData
{
    public override void SkillEnter(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillEnter(player, enemyController, node);
        player.animator.SetInteger("TransitionIndex", (int)SKillID.Assassin_Attack);
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