using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "Sudden Attack", menuName = "Data/Skill/Sudden Attack", order = 1)]
public class Skill_SuddenAttack_DataSCO : SkillData
{
    

    public override void SkillEnter(PlayerController player, EnemyController enemyController, Node node)
    {
        base.SkillEnter(player, enemyController, node);

        var newPos = enemyController.transform.position;
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
        var unitStatus = GameManager.Instance.GetUnitStatus(player.playerID);
        if (enemyController.CheckMiss(player, 10))
        {
            SoundManager.Instance.PlaySFXSound("BaseAttack", 1);      //쳐맞을때소리(기본공격소리)
            enemyController.Damaged(unitStatus.damage * 2);
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

    }
}

