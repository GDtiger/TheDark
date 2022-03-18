using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
public class FSM_Enemy_Attack_State : EnemyState
{
    public float dealTimeWhenStartAnimation = 0.5f;
    //public bool startAction = false;
    public float angle;
    public float rotateSpeed;
    public int transitionIndex;

    public float durationTime = 2;
    public float endTime;
    public Vector3 previousPos;
    public override void FSMEnter(EnemyController enemyController)
    {
        var temp = enemyController.target.transform.position;
        temp.y = enemyController.transform.position.y;
        previousPos = enemyController.transform.position;
        enemyController.transform.DOLookAt(temp, 0.5f).OnComplete(
            () => {
                enemyController.SetDirection();
                enemyController.SetWatchNode();
              //  SoundManager.Instance.PlaySFXSound("BaseAttack", 1);      //쳐맞을때소리(기본공격소리)
                enemyController.animator.SetInteger("TransitionIndex", transitionIndex); 
            });
        endTime = Time.time + durationTime;
    }

    public override void FSMExit(EnemyController enemyController)
    {
        var temp = enemyController.target.transform.position;
        temp.y = enemyController.transform.position.y;
        enemyController.transform
            .DOLookAt(temp, 0.5f)
            .OnStart(() => enemyController.transform.DOMove(previousPos, 0.5f))
            .OnComplete(() => enemyController.state = UnitTurnState.EndProcessing);
       
    }

    public override void FSMUpdate(EnemyController enemyController)
    {
        if (endTime < Time.time)
        {
            FSMExit(enemyController);
        }
    }


}