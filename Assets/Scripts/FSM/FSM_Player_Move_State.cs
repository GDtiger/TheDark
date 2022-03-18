using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class FSM_Player_Move_State : PlayerState
{
    public float curTime;
    public float soundCoolTime;
    public override void FSMEnter(PlayerController unitController)
    {
        Debug.Log($"FSM_Move_Enter {unitController.gameObject.name}");
        //throw new NotImplementedException();
        unitController.animator.SetFloat("Move", 1);
    }

    public override void FSMExit(PlayerController unitController)
    {
        Debug.Log($"FSM_Move_Exit {unitController.gameObject.name}");
        unitController.animator.SetFloat("Move", 0);
        unitController.state = UnitTurnState.EndProcessing;
    }

    public override void FSMUpdate(PlayerController unitController)
    {
        if (curTime <= Time.time)
        {
            curTime = Time.time + soundCoolTime;
            SoundManager.Instance.PlaySFXSound("Running", 1);
        }
        //var curUnitPath = paths
        //더이상 이동할수없을경우 행동 종료
        var curNode = unitController.paths[0];
        if (!(unitController.paths.Count > 0) || 
            //마지막이동할려는 위치가 적일경우 이동종료
            unitController.paths. Count == 1 && curNode.tileWhoStay == TileWhoStay.Enemy)

        {
            FSMExit(unitController);
            return;
        }

        //Debug.Log($"{transform.position}, {paths[0].position}, {moveSpeed * Time.deltaTime}");
        //이동할위치가 현재위치와의 거리가 일정수치이하일때 
        if ((curNode.pos - transform.position).sqrMagnitude < unitController.closeDistance)
        {
            unitController.transform.position = curNode.pos;
            unitController.AddPlayerOnTile();

            // 적이 감지하는곳일경우 발각 트리거발동
            if (curNode.enemyWatch)
            {
                unitController.SetEnemyWhoWatchPlayerTile();
            }
            else
            {
                unitController.HideFromEnemyWhoWatchThisPlayer();
            }


      

            unitController.paths.RemoveAt(0);

        }
        else
        {
            unitController.transform.position = Vector3.MoveTowards(unitController.transform.position, curNode.pos, unitController.moveSpeed * Time.deltaTime);
            Vector3 relativePos = curNode.pos - unitController.transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            unitController.transform.rotation = Quaternion.Lerp(unitController.transform.rotation, toRotation, unitController.unitRotateSpeed * Time.deltaTime);

        }
    }
}