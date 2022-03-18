using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class FSM_Enemy_Move_State : EnemyState
{
    public float curTime;
    public float soundCoolTime;
    public override void FSMEnter(EnemyController unitController)
    {

        Debug.Log($"FSM_Move_Enter {unitController.gameObject.name}");
        unitController.animator.SetFloat("Move", 1);
    }

    public override void FSMExit(EnemyController unitController)
    {
        Debug.Log($"FSM_Move_Exit {unitController.gameObject.name}");
		if (unitController == null)
		{
            return;
		}
        if (unitController.target != null && unitController.CheckTargetInSight(unitController.target.onTile))
        {
            var temp = unitController.target.transform.position;
            temp.y = unitController.transform.position.y;
            unitController.transform.DOLookAt(temp, 0.5f).OnComplete(
            () => {
                unitController.SetDirection();
                unitController.SetWatchNode();
                unitController.paths.Clear();
                unitController.animator.SetFloat("Move", 0);
                unitController.state = UnitTurnState.EndProcessing;
            });
        }
        else
        {
            unitController.SetDirection();
            unitController.SetWatchNode();
            unitController.paths.Clear();
            unitController.animator.SetFloat("Move", 0);
            unitController.state = UnitTurnState.EndProcessing;
        }
        
    }

    public override void FSMUpdate(EnemyController unitController)
    {
		//var curUnitPath = paths
		if (curTime <= Time.time)
		{
            curTime = Time.time + soundCoolTime;
            SoundManager.Instance.PlaySFXSound("Running", 1);
		}
		if (unitController == null)
		{
            FSMExit(unitController);
            return;
        }
        //경로가 0이거나 이동할수있는 거리가 0이거나 배틀모드상태일때 타겟이 공격범위 안에 들어올때
        if (unitController.remainAbletoMoveRange == 0
            || !(unitController.paths.Count > 0)
            || unitController.actingStopImidiate)
        {

            FSMExit(unitController);
            return;
        }


        //더이상 이동할수없을경우 행동 종료

        if (unitController.battleMode && unitController.CheckTargetInAttackRange())
        {
            unitController.transform.position = unitController.paths[0].pos;

            FSMExit(unitController);
            return;
        }





        //Debug.Log($"{transform.position}, {paths[0].position}, {moveSpeed * Time.deltaTime}");
        //이동할위치가 현재위치와의 거리가 일정수치이하일때 
        if ((unitController.paths[0].pos - transform.position).sqrMagnitude < unitController.closeDistance)
        {
            //Debug.Log($"moved : {GridManager.Instance.GetNode(unitController.transform.position).index}");
            unitController.transform.position = unitController.paths[0].pos;
            unitController.paths.RemoveAt(0);
            unitController.remainAbletoMoveRange--;
            unitController.SetDirection();
            unitController.SetWatchNode();

        }
        else
        {
            //Debug.Log($"moving : {GridManager.Instance.GetNode(unitController.transform.position).index}");
            transform.position = Vector3.MoveTowards(transform.position, unitController.paths[0].pos, unitController.moveSpeed * Time.deltaTime);
            Vector3 relativePos = unitController.paths[0].pos - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, unitController.unitRotateSpeed * Time.deltaTime);
        
        }
    }


    
    

    //private void EndTrigger(EnemyController unitController)
    //{
        
    //}
}