using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using System;

public class TurnManager : MonoBehaviour
{

    [TabGroup("Ref", "Need")] public CinemachineFreeLook cinemachine;
    [TabGroup("Ref", "Auto")] public UIManager uIManager;
    [TabGroup("Ref", "Auto")] public GameManager gm;



    [TabGroup("Parameter", "BattleStatus")] public int phase = 0;
    [TabGroup("Parameter", "BattleStatus")] public int turn = 1;
    [TabGroup("Parameter", "BattleStatus")] public BattleState battleState;

    [TabGroup("Parameter", "BattleSetting")] public float closeDistance = 0.01f;
    [TabGroup("Parameter", "BattleSetting")] public float moveSpeed = 5;
    [TabGroup("Parameter", "BattleSetting")] public float unitRotateSpeed = 5;

    [TabGroup("Parameter", "BattleSetting")] public float delayAfterTurnStart = 1;
    [TabGroup("Parameter", "BattleSetting")] public float delayAfterTurnEnd = 1;
    [TabGroup("Parameter", "BattleSetting")] public float activityUnitInDistance = 10;

    [TabGroup("Parameter", "BattleStatus")]

    [Header("현재 행동하는 유닛들의 집합")]
    [TabGroup("Parameter", "BattleStatus")]
    public List<PlayerController> curPlayerUnits;

    [TabGroup("Parameter", "BattleStatus")]
    public List<EnemyController> curEnemyUnits;

    [TabGroup("Parameter", "BattleStatus")]
    public BossController bossUnits;


    [TabGroup("Parameter", "BattleStatus")]
    public UnitController actingUnit;


    [TabGroup("Parameter", "BattleStatus")]
    public List<UnitController> unitsWhoWaitTurnInThisPhase;
    [TabGroup("Parameter", "BattleStatus")]
    public List<UnitController> unitsWhoDead;

    #region Delay
    [TabGroup("Top", "Delay")] public bool activeDelay;
    [TabGroup("Top", "Delay")] public float delayEndTime = 0;


    public void SetDelay(float time)
    {
        activeDelay = true;
        delayEndTime = time + Time.time;
    }


    public void Delay()
    {
        if (delayEndTime < Time.time)
        {
            activeDelay = false;
        }

    }
    #endregion


    #region instance
    static TurnManager instance = null;

    public static TurnManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TurnManager>();
            }
            return instance;
        }
    }
    #endregion

    public void Initiate()
    {
        turn = 1;
        phase = 0;
        unitsWhoWaitTurnInThisPhase = new List<UnitController>();
        if (uIManager == null)
        {
            uIManager = UIManager.Instance;
        }
        if (gm == null)
        {
            gm = GameManager.Instance;
        }

        for (int i = 0; i < curPlayerUnits.Count; i++)
        {
            curPlayerUnits[i].Initiate();
            curPlayerUnits[i].unitID = i + 1;
        }

        for (int i = 0; i < curEnemyUnits.Count; i++)
        {
            curEnemyUnits[i].Initiate();
            curEnemyUnits[i].unitID = -i - 1;
            curEnemyUnits[i].activeUnit = false;
        }

        bossUnits.Initiate();
        bossUnits.unitID = 0;

        for (int i = 0; i < curPlayerUnits.Count; i++)
        {
            for (int j = 0; j < curEnemyUnits.Count; j++)
            {
                if (Vector3.Distance(curEnemyUnits[i].transform.position, curPlayerUnits[i].transform.position) < activityUnitInDistance)
                {
                    curEnemyUnits[i].activeUnit = true;
                }
            }

            if (Vector3.Distance(bossUnits.transform.position, curPlayerUnits[i].transform.position) < activityUnitInDistance)
            {
                bossUnits.activeUnit = true;
            }
        }


        enabled = true;
    }


    public void ChangeUnitPositionState(int text) {
        if (actingUnit.unitType == UnitType.Ally && actingUnit.state == UnitTurnState.SetOrder)
        {
            actingUnit.ChangePositionAnimation((UnitPositionState)text);
        }
    }

    public void Update()
    {
        if (gm.isPaused) return;
        ////if (actingUnit != null)
        ////{
        ////    uIManager.displayCurrentTurn.text = $"BattleState : {battleState.ToString()}\nUnitState : {actingUnit.state.ToString()}\nOrderType : {actingUnit.orderType.ToString()}";

        ////}
        ////else
        ////{
        ////    uIManager.displayCurrentTurn.text = $"BattleState : {battleState.ToString()}";
        ////}

        Delay();
        if (activeDelay) return;



        if (unitsWhoWaitTurnInThisPhase.Count == 0)
        {
            EndPhase();
            return;
        }
        switch (battleState)
        {
            case BattleState.Turn_Start:
               
                BattleTurnStart();
                break;
            case BattleState.Turn_Processing:
                Processing(actingUnit);
                break;
            case BattleState.Turn_End:
                BattleTurnEnd();
                break;
        }
    }

    #region Battle Turn Logic
    private void BattleTurnStart()
    {
        //acc 갱신
       // UIManager.Instance.SetAccText();
        //현재 턴의 유닛을 설정
        actingUnit = unitsWhoWaitTurnInThisPhase[unitsWhoWaitTurnInThisPhase.Count - 1];

        //턴 순서에서 해당유닛 제거
        unitsWhoWaitTurnInThisPhase.RemoveAt(unitsWhoWaitTurnInThisPhase.Count - 1);
        turn++;
        if (!actingUnit.activeUnit)
        {
            battleState = BattleState.Turn_End;
            return;
        };



        if (actingUnit.unitType == UnitType.Ally)
        {
  
            cinemachine.Follow = actingUnit.transform;
            cinemachine.LookAt = actingUnit.transform;
            for (int i = 0; i < curEnemyUnits.Count; i++)
            {
                if (!curEnemyUnits[i].activeUnit && Vector3.Distance(curEnemyUnits[i].transform.position, actingUnit.transform.position) < activityUnitInDistance)
                {
                    curEnemyUnits[i].activeUnit = true;
                }

            }
            if (!bossUnits.activeUnit && Vector3.Distance(bossUnits.transform.position, actingUnit.transform.position) < activityUnitInDistance)
            {
                bossUnits.activeUnit = true;
            }
        }

        battleState = BattleState.Turn_Processing;
        actingUnit.state = UnitTurnState.TurnStart;
        actingUnit.ownTurn = true;
        SetDelay(delayAfterTurnStart);
    }

    private void Processing(UnitController curActingUnit)
    {
        if (curActingUnit.ownTurn)
        {
            switch (curActingUnit.state)
            {
                case UnitTurnState.WaitTurn:
                    break;
                case UnitTurnState.TurnStart:
                    Unit_StartTurn(curActingUnit);
                    break;
                case UnitTurnState.OrderInitiate:
                    Unit_OrderInitiate(curActingUnit);
                    break;
                case UnitTurnState.SetOrder:
                    Unit_SetOrder(curActingUnit);
                    break;
                case UnitTurnState.EndOrder:
                    Unit_EndOrder(curActingUnit);
                    break;
                case UnitTurnState.Processing:
                    Unit_Processing(curActingUnit);
                    break;
                case UnitTurnState.EndProcessing:
                    Unit_EndProcessing(curActingUnit);
                    break;
                case UnitTurnState.TurnEnd:
                    Unit_TurnEnd(curActingUnit);
                    break;
            }
        }
    }


    /// <summary>
    /// <para>해당턴의 마지막 업데이트시 동작</para> 
    /// <br>- battle 상태를 스타트로 변경</br> 
    /// <br>- 턴 추가</br> 
    /// </summary>
    private void BattleTurnEnd()
    {
        battleState = BattleState.Turn_Start;
        actingUnit.Unit_EndTurn();
       
        //A B = 3 C = 5 D

    }
    #endregion


    private void EndPhase()
    {
        phase++;
        for (int i = 0; i < curPlayerUnits.Count; i++)
        {
            unitsWhoWaitTurnInThisPhase.Add(curPlayerUnits[i]);
        }
        for (int i = 0; i < curEnemyUnits.Count; i++)
        {
            unitsWhoWaitTurnInThisPhase.Add(curEnemyUnits[i]);
        }
        unitsWhoWaitTurnInThisPhase = unitsWhoWaitTurnInThisPhase.OrderBy(unit => unit.turnSpeed).ToList();
        uIManager.WaitQueueAddUI();
    }






    [Button]
    public void Order_CurUnitEndTurn()
    {
        actingUnit.actionPoint = 0;
        actingUnit.state = UnitTurnState.TurnEnd;
    }







    /// <summary>
    /// <para>해당턴이 시작된후 최초의 업데이트시 동작</para> 
    /// <br>- ActionPoint 초기화</br> 
    /// <br>- ActionPoint 디스플레이</br> 
    /// </summary>
    void Unit_StartTurn(UnitController curActingUnit)
    {
        Debug.Log($"Turn Start : {curActingUnit.gameObject.name}");
        curActingUnit.StartTurn();
        curActingUnit.actionPoint = 2;
        curActingUnit.ableToAction = true;
        //uIManager.displayActionPoint.text = "Action Point : 2";
    }

    /// <summary>
    /// <para> 현재 턴의 유닛의 명령을 설정하기전에 동작</para> 
    /// <br>플레이어캐릭터일경우</br> 
    /// <br>- 행동반경 설정</br> 
    /// </summary>
    void Unit_OrderInitiate(UnitController curActingUnit)
    {
        Debug.Log($"ProcessingSetup : {curActingUnit.gameObject.name}");

        curActingUnit.Unit_OrderInitiate();
    }

    /// <summary>
    /// <para> 현재 턴의 유닛의 명령을 설정</para> 
    /// </summary>
    void Unit_SetOrder(UnitController curActingUnit)
    {
        //Debug.Log($"Set Order : {curActingUnit.gameObject.name}");
        curActingUnit.Unit_SetOrder();

    }

    /// <summary>
    /// <para> 현재 턴의 유닛의 명령 설정이 종료 되었을때 동작</para> 
    /// <para>- ActionPoint의 감소</para> 
    /// </summary>
    void Unit_EndOrder(UnitController curActingUnit)
    {
        Debug.Log($"End Order : {curActingUnit.gameObject.name}");
        curActingUnit.Unit_EndOrder();
    }



    /// <summary>
    /// 현재 턴의 유닛이 행동할때 동작하는 함수
    /// </summary>
    void Unit_Processing(UnitController curActingUnit)
    {
        //Debug.Log($"Unit_Processing : {curActingUnit.gameObject.name} {curActingUnit.transform.position}");
        curActingUnit.UnitUpdate();
        //Debug.Log($"Unit_Processing : {curActingUnit.gameObject.name} {curActingUnit.transform.position}");
    }


    /// <summary>
    /// 현재 턴의 유닛의 행동을 종료했을때 동작하는 함수
    /// </summary>
    static void Unit_EndProcessing(UnitController curActingUnit)
    {
        Debug.Log($"End Processing : {curActingUnit.gameObject.name}");
        curActingUnit.Unit_EndProcessing();

    }




    /// <summary>
    /// 현재 턴의 마지막 업데이트
    /// </summary>
    void Unit_TurnEnd(UnitController curActingUnit)
    {
        Debug.Log($"Turn End Processing : {curActingUnit.gameObject.name}");
        battleState = BattleState.Turn_End;
        //curActingUnit.orderType = OrderType.None;
        //curActingUnit.scheduledAction = OrderPlayerType.None;
        uIManager.WaitQueueDeleteUI();
    }

    public void EnemyDead(EnemyController enemyController)
    {
        for (int i = 0; i < curEnemyUnits.Count; i++)
        {
            if (enemyController.unitID == curEnemyUnits[i].unitID)
            {
                curEnemyUnits.RemoveAt(i);
                break;
            }
        }

        for (int i = 0; i < unitsWhoWaitTurnInThisPhase.Count; i++)
        {
            if (enemyController.unitID == unitsWhoWaitTurnInThisPhase[i].unitID)
            {
                unitsWhoWaitTurnInThisPhase.RemoveAt(i);
                break;
            }
        }

        Destroy(enemyController.gameObject);

        //적이 죽을때 다시 검색해서 갱신
        uIManager.WaitQueueAddUI();
    }
}