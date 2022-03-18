using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : UnitController
{
    [SerializeField] ObjectPoolSCO objectPoolSCO;
    public EnemyUnitStatus unitStatus;
    //public Transform[] targetWaypoints;

    //public Dictionary<Vector2Int, KeyValuePair<int, Node>> watchingNode;
    [SerializeReference] public Dictionary<Vector2Int, KeyValuePair<int, Node>> detectionNode;
    //public Dictionary<Vector2Int, KeyValuePair<int, Node>> recogitionNode;
    public bool hasTarget;
    public bool cautiousMode;
    public PlayerController target;
    //public Transform targetMark;
    public Node targetTile;
    public LayerMask checkDir;
    [TabGroup("Ref", "Need")] public Transform sightPos;

    public List<EnemyController> friendEnemy;
    [TabGroup("Ref", "Auto")] public WaypointManager waypointManager;
    [TabGroup("Ref", "Need")] public EmotionController emotionController;
    [TabGroup("Ref", "Need")] public TargetMarkController targetMarkController;
    [TabGroup("Ref", "Need")] public HPbarController hPbarController;

    [TabGroup("Ref", "Need")] public FSM_Enemy_Move_State moveState;
    [TabGroup("Ref", "Need")] public FSM_Enemy_Attack_State attackState;
    [TabGroup("Parameter", "Parameter")] public EnemyState currentState;
    [TabGroup("Parameter", "Parameter")] public bool actingStopImidiate;
    [TabGroup("Setting", "Setting")] public List<Transform> remainWaypoints;

    public List<string> itemDropRates;
    public override void Initiate()
    {

        base.Initiate();
        Debug.Log(gameObject.name);
        if (objectPoolSCO == null)
            objectPoolSCO = GameManager.Instance.objectPool;
        if (waypointManager == null)
            waypointManager = WaypointManager.Instance;
        //watchingNode = new Dictionary<Vector2Int, KeyValuePair<int, Node>>();
        detectionNode = new Dictionary<Vector2Int, KeyValuePair<int, Node>>();
        unitStatus.SetFullHP();
        Unit_EndProcessing();
        transform.position = onTile.pos;
        AddEnemyOnTile();
        unitStatus.originNode = onTile;
        unitStatus.originLookAt = lookAt;
        unitStatus.originAiState = unitStatus.aiStateType;
        remainWaypoints = new List<Transform>();

    }

    public void AddEnemyOnTile()
    {
        if (onTile != null)
        {
            onTile.RemoveEnemy();
        }

        //Debug.Log($"{gameObject.name} SetEnemyTile");

        onTile = gridManager.GetNode(transform.position);
        if (onTile == null)
        {

            Debug.LogError($"{gameObject.name} is not on Tile");

            ErrorDead();
        }
        else
        {
            onTile.AddEnemy(this);
        }

    }

    public override void StartTurn()
    {
        base.StartTurn();
        Debug.Log($"적 턴 시작");
        unitStatus.aiStateType = unitStatus.originAiState;
        if (battleMode)
        {
            var nodes = gridManager.GetNeighborNodesInRange(transform.position, unitStatus.enemyBaseInformation.callRange);
            Debug.Log($"적 전투모드이므로 주변 적콜하기 {nodes.Count}");
            foreach (var item in nodes)
            {
                if (item.Key == onTile.index) continue;
                if (item.Value.Value.tileWhoStay == TileWhoStay.Enemy)
                {
                    item.Value.Value.whoStandEnemy.FoundPlayer(target);
                    Debug.Log($"적{gameObject.name}은 주변의 {item.Value.Value.pos}위치의 적{item.Value.Value.whoStandEnemy.gameObject.name}을 콜함");
                }
            }
        }
    }
    public override void Unit_OrderInitiate()
    {
        state = UnitTurnState.SetOrder;
        actingStopImidiate = false;
    }
    public override void Unit_SetOrder()
    {
        //전투모드
        if (battleMode)
        {
            if (target != null)
            {
                if (ableToAction && gridManager.GetRange(target.transform.position, transform.position) <= unitStatus.enemyBaseInformation.attackRange)
                {
                    unitStatus.aiStateType = AIStateType.Attack;
                }
                //타겟이 공격범위 밖에 있는경우 추격
                else
                {
                    unitStatus.aiStateType = AIStateType.TargetUnitTrack;
                }
            }
            else
            {
                if (ableToAction && gridManager.GetRange(targetTile.pos, transform.position) <= unitStatus.enemyBaseInformation.attackRange)
                {
                    unitStatus.aiStateType = AIStateType.Patrol;
                    ReturnToNormalMode();

                }
                else
                {
                    unitStatus.aiStateType = AIStateType.TrackMarkWatch;
                }
            }
            //ClearPreviousWatchNode();
            //타겟이 공격범위 안에 있는경우 공격

        }

        SetAiStateFSM();

    }

    public void ReturnToNormalMode()
    {
        battleMode = false;
        targetTile = null;
        cautiousMode = false;
        emotionController.HideImage();
        if (targetMarkController != null)
        {
            targetMarkController.GobackToNormal();
        }
        targetMarkController = null;
        unitStatus.aiStateType = unitStatus.originAiState;
    }

    public void ReturnToNormalAllLinkTargetMark()
    {
        battleMode = false;
        targetTile = null;
        cautiousMode = false;
        emotionController.HideImage();
        targetMarkController = null;

        switch (unitStatus.originAiState)
        {
            case AIStateType.Idle:
                unitStatus.aiStateType = AIStateType.BackToOriginPos;
                break;
            case AIStateType.Patrol:
                unitStatus.aiStateType = unitStatus.originAiState;
                break;
            case AIStateType.TargetUnitTrack:
                break;
            case AIStateType.TrackMarkWatch:
                break;
            case AIStateType.BackToOriginPos:
                break;
            case AIStateType.Attack:
                break;
            case AIStateType.Skill:
                break;
            case AIStateType.Dead:
                break;
            default:
                break;
        }
    }

    private void SetAiStateFSM()
    {
        switch (unitStatus.aiStateType)
        {
            case AIStateType.Idle:
                Debug.Log($"{gameObject.name} Idle");

                //Do Nothing
                orderType = OrderType.None;
                state = UnitTurnState.EndOrder;
                actionPoint = 0;
                break;
            case AIStateType.Patrol:
                {
                    Debug.Log($"{gameObject.name} Patrol");
                    remainAbletoMoveRange = 1000;
                    //Move To Waypoint
                    paths = null;
                    currentState = moveState;
                    //var nodeIndex = gridManager.GetNode(transform.position).index;
                    var waypointDic = waypointManager.waypointDic;


                    if (waypointDic.ContainsKey(this))
                    {
                        Debug.Log($"{remainWaypoints.Count}");
                        if (remainWaypoints.Count == 0)
                        {
                            remainWaypoints = waypointDic[this].waypoints.ToList();
                        }

                        paths = gridManager.pathfindingSystem.FindPath(transform.position, remainWaypoints[0].position, remainAbletoMoveRange, false, true);
                        remainWaypoints.RemoveAt(0);

                        if (paths != null && paths.Count > 0)
                        {
                            Debug.Log($"{gameObject.name} {paths.Count} have Paths");
                            orderType = OrderType.Move;
                            state = UnitTurnState.EndOrder;
                        }
                        else
                        {
                            Debug.Log($"{gameObject.name} haven't Paths");
                            orderType = OrderType.None;
                            state = UnitTurnState.EndOrder;
                        }

                    }
                    actionPoint = 0;
                }
                break;
            case AIStateType.TargetUnitTrack:
                //TrackTarget
                {

                    Debug.Log($"{gameObject.name} Track player");

                    currentState = moveState;
                    paths = gridManager.pathfindingSystem.FindPath(transform.position, target.transform.position, 10000, false, true);
    
                    Debug.Log($"{gameObject.name} + {paths}");

                    remainAbletoMoveRange = unitStatus.enemyBaseInformation.maxMoveRange;
                    if (paths != null && paths.Count > 0)
                    {
                        Debug.Log($"{gameObject.name} {paths.Count} have Paths");
                        orderType = OrderType.Move;
                        state = UnitTurnState.EndOrder;
                    }
                    else
                    {
                        Debug.Log($"{gameObject.name} haven't Paths");

                        orderType = OrderType.None;
                        state = UnitTurnState.EndOrder;
                    }

                }
                break;
            case AIStateType.BackToOriginPos:
                {
                    paths = gridManager.pathfindingSystem.FindPath(transform.position, unitStatus.originNode.pos, 10000, false, true);
                    remainAbletoMoveRange = unitStatus.enemyBaseInformation.maxMoveRange;
                    state = UnitTurnState.EndOrder;
                    orderType = OrderType.Move;
                }
                break;
            case AIStateType.TrackMarkSound:
                {
                    Debug.Log($"{gameObject.name} Track last node");

                    currentState = moveState;
                    paths = gridManager.pathfindingSystem.FindPath(transform.position, targetTile.pos, 10000, false, true);

                    Debug.Log($"{gameObject.name} + {paths}");

                    remainAbletoMoveRange = unitStatus.enemyBaseInformation.maxMoveRange;
                    state = UnitTurnState.EndOrder;
                    orderType = OrderType.Move;
                    actionPoint--;
                }
                break;
            case AIStateType.TrackMarkWatch:
                //TrackTarget
                {
                    Debug.Log($"{gameObject.name} Track last node");

                    currentState = moveState;
                    paths = gridManager.pathfindingSystem.FindPath(transform.position, targetTile.pos, 10000, false, true);

                    Debug.Log($"{gameObject.name} + {paths}");

                    remainAbletoMoveRange = unitStatus.enemyBaseInformation.maxMoveRange;
                    state = UnitTurnState.EndOrder;
                    orderType = OrderType.Move;
                    actionPoint--;
                }
                break;
            case AIStateType.Attack:
                //Attack Target
                {
					if (ableToAction)
					{
                        Debug.Log($"{gameObject.name} Attack");
                        currentState = attackState;
                        state = UnitTurnState.EndOrder;
                        orderType = OrderType.Attack;
                        ableToAction = false;
                        actionThisTurn = true;
                        posBeforeAction = gridManager.GetNode(transform.position).index;
                        actionPoint--;
					}
					else
					{
                        state = UnitTurnState.EndOrder;
                        currentState = null;
                        actionPoint--;
                        orderType = OrderType.None;
                    }
                 
                }
                break;
            case AIStateType.Skill:
                //Use Skill
                state = UnitTurnState.EndOrder;
                actionThisTurn = true;
                posBeforeAction = gridManager.GetNode(transform.position).index;
                break;
            default:
                state = UnitTurnState.EndOrder;
                break;
        }
    }

    public override void Unit_EndOrder()
    {
        base.Unit_EndOrder();
        actionPoint--;
		if (currentState != null)
		{
            currentState.FSMEnter(this);
		}


    }


    public override void Unit_EndProcessing()
    {
        Debug.Log($"End Processing : {gameObject.name}");

        AddEnemyOnTile();

        if (actionPoint > 0)
        {

            Debug.Log($"턴 다시 세팅 {actionPoint}");

            state = UnitTurnState.OrderInitiate;
        }
        else
        {
            Debug.Log("턴 종료");
            state = UnitTurnState.TurnEnd;
        }

        Debug.Log($"End Processing : {gameObject.name}");

        //타겟이 없고 서있는 상태가 아닌경우
        if (target == null && !cautiousMode)
        {
            Debug.Log($"End Processing : {gameObject.name}");
            SetDirection();
            SetWatchNode();
        }

        if (actionThisTurn)
        {
            transform.position = gridManager.GetNode(posBeforeAction).pos;
            actionThisTurn = false;
        }

    }

    public void SetTargetAndTrack(PlayerController playerController)
    {
        if (target == null)
        {
            activeUnit = true;
            battleMode = true;
            target = playerController;
            playerController.AddEnemyWhoWatchingThisPlayerUnit(this);
        }
    }

    public void SetNodeToTrack(Node node)
    {
        if (targetTile == null)
        {
            activeUnit = true;
            targetTile = node;
            cautiousMode = true;
        }
    }

    public void RemoveTarget()
    {
        target = null;
        cautiousMode = false;
        ///TODO 다른 아군을 탐색해서 타겟을 찾기 
        //없으면 평상시 모드로 돌아감
        //다대다용
    }

    public void HearSound(Node node)
    {
        emotionController.ShowWhenHearSound();
        //targetMark = graphicManager.targetMarks[target];
        cautiousMode = true;
        activeUnit = true;
        targetTile = node;
        target = null;
        unitStatus.aiStateType = AIStateType.TrackMarkWatch;
    }
    public void MissingTarget(Node lastPos)
    {
        emotionController.ShowWhenMissingTarget();
        battleMode = true;
        activeUnit = true;
        target = null;
        targetTile = lastPos;
        unitStatus.aiStateType = AIStateType.TrackMarkWatch;
    }
    public void FoundPlayer(PlayerController playerController, bool thisUnitFoundTarget = false)
    {
        if (targetMarkController != null)
        {
            targetMarkController.FindTargetAndTrackTarget(playerController);
        }
        else
        {
            FoundPlayerAndSet(playerController);
        }
    }

    public void FoundPlayerAndSet(PlayerController playerController)
    {
        playerController.AddEnemyWhoWatchingThisPlayerUnit(this);
        target = playerController;
        emotionController.ShowWhenFoundTarget();
        targetMarkController = null;

        activeUnit = true;
        if (!battleMode)
        {
            battleMode = true;
            actingStopImidiate = true;
        }

    }

    public void AddDetectionNode(int x, int y, int range, Node node)
    {
        if (node != null)
        {

            detectionNode.Add(new Vector2Int(x, y), new KeyValuePair<int, Node>(range, node));
        }
    }

    public void AddDetectionNode(Vector2Int pos, int range, Node node)
    {
        if (node != null)
        {

            detectionNode.Add(pos, new KeyValuePair<int, Node>(range, node));
        }
    }



    public void LookDirection(DirEight dirEight)
    {
        switch (dirEight)
        {
            case DirEight.T:
                transform.DOLookAt(transform.position + Vector3.back, 0.5f);
                break;
            case DirEight.L:
                transform.DOLookAt(transform.position + Vector3.left, 0.5f);
                break;
            case DirEight.B:
                transform.DOLookAt(transform.position + Vector3.forward, 0.5f);
                break;
            case DirEight.R:
                transform.DOLookAt(transform.position + Vector3.right, 0.5f);
                break;
            default:
                break;
        }

    }

    public Vector2Int RotateAxis(Vector2Int point, DirEight angle)
    {
        switch (angle)
        {
            case DirEight.T:

                return new Vector2Int(-point.y, point.x);
            case DirEight.L:
                return new Vector2Int(-point.x, -point.y);

            case DirEight.B:
                return new Vector2Int(point.y, -point.x);


            case DirEight.R:
                return new Vector2Int(point.x, point.y);
            default:
                return new Vector2Int(0, 0);
        }


    }
    public void LookTarget()
    {
        var temp = target.transform.position;
        temp.y = transform.position.y;
        transform.DOLookAt(temp, 0.5f).OnComplete(
        () => {
            SetDirection();
            SetWatchNode();
        });
    }
    public bool CheckTargetInSight(Node targetNode)
    {
        if (targetNode != null && battleMode)
        {
            Node startNode = gridManager.GetNode(transform.position);

            int x = startNode.index.x;
            int y = startNode.index.y;

            int sightRange = battleMode ? 8 : 6;

            for (int sight = 0; sight < 4; sight++)
            {
                for (int i = 1; i < sightRange; i++)
                {
                    int jVal = 0;
                    if (i < 4)
                    {
                        jVal = i;
                    }
                    else
                    {
                        jVal = sightRange - i;
                    }
                    for (int j = -jVal; j <= jVal; j++)
                    {
                        var pos = new Vector2Int(x, y) + RotateAxis(new Vector2Int(i, j), (DirEight)sight);
                        //var pos = new Vector2Int(x + i, y + j);

                        //Debug.Log($"{startNode.index}, {pos}");
                        var node = gridManager.GetNode(pos);

                        if (node != null && node == targetNode)
                        {
                            Debug.DrawLine(transform.position, node.pos, Color.red, 10);

                            Debug.Log($"플레이어는 적({gameObject.name}) {targetNode.index} 시야 아래에 있습니다.");

                            return true;
                        }
                    }
                }
            }
        }
        Debug.Log($"플레이어는 적({gameObject.name}) {targetNode.index} 시야 아래에 없습니다.");
        return false;
    }

    public void SetWatchNode()
    {
        Debug.Log($"{gameObject.name} : SearchEnemy hasTarget : {hasTarget}");
        ClearPreviousWatchNode();

        Node startNode = gridManager.GetNode(transform.position);

        int x = startNode.index.x;
        int y = startNode.index.y;

        int sightRange = battleMode ? 8 : 6;
        int limitUnderShadow = battleMode ? 6 : 5;

        for (int i = 1; i < sightRange; i++)
        {
            int jVal = 0;
            if (i < 4)
            {
                jVal = i;
            }
            else
            {
                jVal = sightRange - i;
            }
            for (int j = -jVal; j <= jVal; j++)
            {
                var pos = new Vector2Int(x, y) + RotateAxis(new Vector2Int(i, j), lookAt);
                //var pos = new Vector2Int(x + i, y + j);

  ;             //Debug.Log($"{startNode.index}, {pos} {i} {j} {jVal}");
                var node = gridManager.GetNode(pos);
  
                if (node != null)
                {
                    //Debug.Log($"{node} {node.nodeInShadow}");
                    if (node.nodeInShadow)
                    {
                        if (limitUnderShadow <= Mathf.Abs(i) + Mathf.Abs(j) || Mathf.Abs(i) == limitUnderShadow - 1)
                        {
                           // Debug.DrawLine(transform.position + Vector3.up, gridManager.GetNode(pos).pos + Vector3.up, Color.green, 10);
                        }
                        else
                        {
                            AddDetectionNode(pos, Mathf.Abs(i) + Mathf.Abs(j), node);
                            graphicManager.AddDetectionNode(this, pos);
                            //Debug.DrawLine(transform.position + Vector3.up, gridManager.GetNode(pos).pos + Vector3.up, Color.red, 10);
            
                        }
                       
                    }
                    else
                    {
                        AddDetectionNode(pos, Mathf.Abs(i) + Mathf.Abs(j), node);
                        graphicManager.AddDetectionNode(this, pos);
                        //Debug.DrawLine(transform.position + Vector3.up, gridManager.GetNode(pos).pos + Vector3.up, Color.blue, 10);
                    }
     
                    //Debug.DrawLine(transform.position, startNode.pos, Color.green, 10);
         
                }
            }
        }

        foreach (var item in detectionNode)
        {


            if (item.Value.Value.tileWhoStay == TileWhoStay.Player)
            {
                FoundPlayer(item.Value.Value.whoStandPlayer, true);
                break;
            }
        }

		
    }



    private void ClearPreviousWatchNode()
    {
        if (detectionNode != null)
        {
            foreach (var item in detectionNode)
            {
                graphicManager.RemoveDetectionNode(this, item.Value.Value);
            }
            detectionNode.Clear();
        }
        else
        {
            detectionNode = new Dictionary<Vector2Int, KeyValuePair<int, Node>>();
        }
    }

    public override Vector3 GetTargetPosition()
    {
        return target.transform.position;
    }
    public override int GetMaxMoveRange()
    {
        return unitStatus.enemyBaseInformation.maxMoveRange;
    }
    public override bool CheckTargetInAttackRange()
    {
        if (target != null)
        {
            //Debug.Log($"{gameObject.name}{gridManager.GetNode(transform.position).index} + {gridManager.GetNode(target.transform.position).index}");
            return battleMode && GridManager.Instance.GetRange(transform.position, target.transform.position) <= unitStatus.enemyBaseInformation.attackRange;
        }

        if (targetTile != null)
        {
            if (battleMode && GridManager.Instance.GetRange(transform.position, targetTile.pos) <= unitStatus.enemyBaseInformation.attackRange)
            {
                ReturnToNormalMode();


                return true;
            }
            else
            {
                return false;
            }
            //Debug.Log($"{gameObject.name}{gridManager.GetNode(transform.position).index} + {gridManager.GetNode(target.transform.position).index}");

        }


        return true;
    }

    public override void UnitUpdate()
    {
        switch (orderType)
        {
            case OrderType.None:
                Processing_Idle();
                break;
            case OrderType.Move:
                moveState.FSMUpdate(this);
                break;
            case OrderType.Attack:
                attackState.FSMUpdate(this);
                break;
            case OrderType.Skill:
                break;
            default:
                break;
        }
    }



    public override void Damaged(int damage)
    {
        if (unitStatus.hp - damage > 0)
        {
            unitStatus.hp -= damage;
            hPbarController.ShowHpBar();
            hPbarController.SetHp(unitStatus.hp / (float)unitStatus.enemyBaseInformation.maxHP);
            var damageDisplay = gm.objectPool.RequestObject(PrefabID.DamageTexture) as DamageDisplayController;
            damageDisplay.ShowText(damage, transform.position);
            gm.GetMana(1);
            UIManager.Instance.SetEnemyHP(this, unitStatus.hp, unitStatus.enemyBaseInformation.maxHP);
            animator.SetInteger("TransitionIndex", (int)AnimeID.Hit);
        }
        else
        {
            if (battleMode)
            {
                gm.GetMana(2);
            }
            else
            {
                gm.GetMana(1);
            }

            Dead();
        }
    }

    public void ErrorDead() {
        turnManager.EnemyDead(this);
    }

    public override void Dead()
    {
        turnManager.EnemyDead(this);
        onTile.RemoveEnemy();
        ClearPreviousWatchNode();

        TurnManager.Instance.curPlayerUnits[0].RemoveEnemyWhoWatchingThisPlayerUnit(this);

        Debug.Log($"{gameObject.name}은 죽음");
       
        gm.GetItemsToPlayer(itemDropRates, transform);
        UIManager.Instance.OnOffEnemyInfo(this, false);
        ////itemDropRates
    }

    public void MeleeAttackTrigger()
    {
        if (CheckMiss(target))
        {
            target.Damaged(unitStatus.enemyBaseInformation.damage);
            SoundManager.Instance.PlaySFXSound("BaseAttack", 1);      //쳐맞을때소리(기본공격소리)
        }
        else
        {
            SoundManager.Instance.PlaySFXSound("MissAttack", 1);      //쳐맞을때소리(기본공격소리)
            var damageDisplay = gm.objectPool.RequestObject(PrefabID.MissTexture) as MissTextureController;
            damageDisplay.ShowText(transform.position);
        }

    }

}