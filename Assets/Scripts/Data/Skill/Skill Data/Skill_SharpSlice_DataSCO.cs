using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "SharpSlice", menuName = "Data/Skill/SharpSlice", order = 1)]
public class Skill_SharpSlice_DataSCO : SkillData
{
    public Vector3 previousPos;
    public override void SkillEnter(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillEnter(player, enemyController, node);
        player.animator.SetInteger("TransitionIndex", (int)AnimeID.Anime001_SpeedSlash);

        var temp = enemyController.transform.position;
        temp.y = enemyController.transform.position.y;
        previousPos = player.transform.position;
        player.transform.DOLookAt(temp, 0.5f).OnComplete(
    () => { player.animator.SetInteger("TransitionIndex", (int)animationStart); });

    }
    public override void SkillUpdate(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillUpdate(player, enemyController, node);
    }
    public override void SkillTrigger(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillTrigger(player, enemyController, node);
        var unitStatus = GameManager.Instance.GetUnitStatus(player.playerID);
        if (enemyController.CheckMiss(player))
        {
            SoundManager.Instance.PlaySFXSound("BaseAttack", 1);      //쳐맞을때소리(기본공격소리)
            enemyController.Damaged(unitStatus.damage);
        }
        else
        {
            SoundManager.Instance.PlaySFXSound("MissAttack", 1);      //쳐맞을때소리(기본공격소리)
            var damageDisplay = GameManager.Instance.objectPool.RequestObject(PrefabID.MissTexture) as MissTextureController;
            damageDisplay.ShowText(enemyController.transform.position);
        }
        SkillExit(player, enemyController, node);
    }
    public override void SkillExit(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillExit(player, enemyController, node);
        player.ChangePositionAnimation(UnitPositionState.Guard, true);
    }
}