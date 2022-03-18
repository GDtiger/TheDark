using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum OrderPlayerType { Skill1, Skill2, Skill3, Skill4, Attack, Move, None = 9 , Item1 = 10, Item2= 11, Item3= 12};

public class PlayerController : UnitController
{
    //[SerializeField] Rigidbody rb;
    //[TabGroup("Ref", "Auto")] [SerializeField] NavMeshAgent agent;
    [TabGroup("Parameter", "Parameter")] [SerializeField] ControlType controlType;
    [TabGroup("Ref", "Auto")] [SerializeField] ObjectPoolSCO objectPoolSCO;
    public int playerID;

    //public PlayerUnitStatus unitStatus;
    [TabGroup("Ref", "Need")] public FSM_Player_Move_State moveState;
    [TabGroup("Ref", "Need")] public FSM_Player_Skill_State skillState;
    [TabGroup("Ref", "Need")] public TargetMarkController targetMark;


    public bool ableToMove;
    public bool showPath;
    public Vector2Int previousShowPathIndex;

    public OrderPlayerType orderActivateType = OrderPlayerType.Move;

    Dictionary<Vector2Int, KeyValuePair<int, Node>> allowToClickTile;
    bool needToMoveUnderEnemySight;

    [Tooltip("은신이 걸림")]
    //[TabGroup("Parameter", "BattleSetting")] public bool detected;

    [TabGroup("Parameter", "BattleSetting")] public EnemyController target;
    [TabGroup("Parameter", "BattleSetting")] public List<EnemyController> enemyWhoWatchingThisPlayer;
    [TabGroup("Parameter", "BattleSetting")] public bool enemyWachingThisTarget;
    [TabGroup("Parameter", "BattleSetting")] public SkillData curSkillData;
    [TabGroup("Parameter", "BattleSetting")] public Node targetNode;
    [TabGroup("Parameter", "BattleSetting")] public Node lastNodeWitchEnemyWatch;

    [Header("설정된 다음명령이 존재함")]
    [TabGroup("Parameter", "BattleSetting")] public OrderPlayerType scheduledAction;

    [TabGroup("Parameter", "Parameter")] bool hasPath;
    [TabGroup("Parameter", "Parameter")] public PlayerState currentState;
    [TabGroup("Parameter", "Parameter")] public bool ableToChangePosition;


    public override void Initiate()
    {
        base.Initiate();
        if (objectPoolSCO == null)
            objectPoolSCO = GameManager.Instance.objectPool;
        Unit_EndProcessing();
        transform.position = onTile.pos;
        //var unitStatus = gm.GetUnitStatus(playerID);
        //unitStatus.InitiateGame();
        allowToClickTile = new Dictionary<Vector2Int, KeyValuePair<int, Node>>();
        //var tempMark = graphicManager.InitiatePlayer(this);
        // if (tempMark != null) targetMark = tempMark;
    }



    public override void StartTurn()
    {
        base.StartTurn();
        ConnectWithInputManager();
        AddPlayerOnTile();
        UIManager.Instance.SetFullActionPoint();
        gm.InitiatePlayerTurn(playerID);
        ableToChangePosition = true;
    }


    public override void Unit_OrderInitiate()
    {

        Debug.Log($"{gameObject.name} - Unit_OrderInitiate");
        
        switch (scheduledAction)
        {
            case OrderPlayerType.Skill1:
                break;
            case OrderPlayerType.Skill2:
                break;
            case OrderPlayerType.Skill3:
                break;
            case OrderPlayerType.Attack:
                {
                    state = UnitTurnState.EndOrder;
                    ableToAction = false;
                    orderType = OrderType.Attack;
                }
                break;
            case OrderPlayerType.Move:
                break;
            case OrderPlayerType.None:
                {

                    if (actionPoint > 0)
                    {
                        ShowMoveAbleTile();
                    }
                    //targetNode = null;
                    target = null;
                    previousShowPathIndex = new Vector2Int(-1, -1);
                    state = UnitTurnState.SetOrder;
                }
                break;
            default:
                break;
        }
    }

    public override void Unit_EndOrder()
    {
        base.Unit_EndOrder();
        //그래픽매니저에서 현재 타일들 모두 없애기
        graphicManager.CleaAllTileNode();
        //UI매니저에서 버튼들 모두 비활성화시키기
        gm.uIManager.DeactiveAllButton();


    }

    public override void Unit_EndProcessing()
    {
        AddPlayerOnTile();
        SetDirection();

        //아이콘 비활성화
        graphicManager.CleaAllTileNode();
		if (actionPoint > 0)
		{
            state = UnitTurnState.OrderInitiate;
		}
		else
		{
            state = UnitTurnState.TurnEnd;
        }


        if (actionThisTurn)
        {
            actionThisTurn = false;
        }
        orderType = OrderType.None;
        if (onTile.enemyWatch)
        {
            //graphicManager.HideTargetMark(this);
            foreach (var enemy in onTile.enemyWhoWatchingThisNode)
            {
                enemy.FoundPlayer(this);
            }
        }

    }

    public override void Unit_EndTurn()
    {
        base.Unit_EndTurn();
        AddPlayerOnTile();
        gm.uIManager.DeactiveAllButton();
        graphicManager.CleaAllTileNode();
        if (actionThisTurn)
        {
            actionThisTurn = false;
        }
        orderType = OrderType.None;
        scheduledAction = OrderPlayerType.None;
        orderActivateType = OrderPlayerType.Move;
        DisconnectWithInputManager();
    }

    public void CallInSoundRange(Node originNode, int range) {
        var allNode = gridManager.GetAllNodesInRange(originNode, range);
		foreach (var node in allNode)
		{
			if (node.Value.Value.whoStandEnemy != null)
			{
                node.Value.Value.whoStandEnemy.HearSound(originNode);
            }
		}
    }

    //마우스 클릭 또는 터치시 작동
    public void PlayerInput()
    {
        if (state != UnitTurnState.SetOrder) return;

        var unitStatus = gm.GetUnitStatus(playerID);
        Ray cast = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log(gameObject.name);
        if (Physics.Raycast(cast, out RaycastHit hit, Mathf.Infinity))
        {
            Debug.Log(hit.transform.gameObject.name);
            ////Debug.Log($"{hit.point} 어딘가 부디침");

            if (((1 << hit.transform.gameObject.layer) & graphicManager.hitGroundLayer) != 0)
            {
                var index = GridManager.GetVector2IntFromPosition(hit.point);
                var node = gridManager.GetNode(index);
                if (node == null) return;
                if (!allowToClickTile.ContainsKey(node.index)) return;
                switch (orderActivateType)
                {
                    case OrderPlayerType.Move:
                        {
                            if (actionPoint == 0) return;
                            switch (node.tileWhoStay)
                            {
                                case TileWhoStay.None:
                                    //비어있는 위치일경우 
                                    {
                                        //경로안에 있을경우 && 이전좌표와 같지 않을경우
                                        if (previousShowPathIndex != index && allowToClickTile.ContainsKey(index))
                                        {
                                            DrawPathByLine(hit, index);
                                        }
                                        //경로안에 있을경우 && 이전좌표와 같을 경우 이동함
                                        else
                                        {
                                            if (hasPath)
                                            {
                                                moveState.FSMEnter(this);
                                                state = UnitTurnState.EndOrder;
                                                orderType = OrderType.Move;
                                                scheduledAction = OrderPlayerType.None;
                                                ReduceActionPoint();
                                            }

                                            #region MyRegion
                                            ////전투모드가 아닐때는 감지되지 않아야 이동가능
                                            //if (((!battleMode && !detected) ||
                                            //    //전투모드일때는 상관없음
                                            //    (battleMode))
                                            //    //타일을 클릭해야 이동
                                            //    && allowToClickTile.ContainsKey(index))
                                            //{
                                            //    //아무것도 없을때만 이동가능
                                            //    switch (allowToClickTile[index].Value.tileWhoStay)
                                            //    {
                                            //        case TileWhoStay.None:
                                            //            {
                                            //                if (controlType == ControlType.FreeMode)
                                            //                {
                                            //                    Vector3 tilePos = Vector3.zero;
                                            //                    //if (gridManager.GetNodePosition(hit.point, ref tilePos))
                                            //                    //agent.destination = tilePos;
                                            //                }
                                            //                else
                                            //                {
                                            //                    if (hasPath)
                                            //                    {
                                            //                        state = UnitTurnState.EndOrder;
                                            //                        orderType = OrderType.Move;
                                            //                        scheduledAction = OrderPlayerType.None;
                                            //                    }
                                            //                    ////잠행이동 경로 추적
                                            //                    //paths = gridManager.pathfindingSystem.FindPath(transform.position, hit.point, unitStatus.maxMoveRange, true, false);

                                            //                    ////일반 이동 경로 추적
                                            //                    //if (paths == null)
                                            //                    //{
                                            //                    //    paths = gridManager.pathfindingSystem.FindPath(transform.position, hit.point, unitStatus.maxMoveRange, false, false);
                                            //                    //}


                                            //                    //if (paths != null && paths.path.Count > 0)
                                            //                    //{
                                            //                    //    state = UnitTurnState.EndOrder;
                                            //                    //    orderType = OrderType.Move;
                                            //                    //}
                                            //                }
                                            //            }
                                            //            break;
                                            //        case TileWhoStay.Enemy:
                                            //            {

                                            //            }
                                            //            break;
                                            //        case TileWhoStay.Player:
                                            //            {
                                            //                //아무일도 일어나지 않음
                                            //            }
                                            //            break;
                                            //        case TileWhoStay.Trab:
                                            //            break;
                                            //        default:
                                            //            break;
                                            //    }
                                            //}
                                            //return;
                                            #endregion

                                        }
                                    }
                                    break;
                                case TileWhoStay.Enemy:
                                    {
                                        if (!ableToAction) return;
                                        if (previousShowPathIndex != index && allowToClickTile.ContainsKey(index))
                                        {
                                            DrawPathByLine(hit, index);
                                            graphicManager.ShowSoundNode(index, unitStatus.attackNoiseRange);
                                            //graphicManager.skillRangeTile
                                        }
                                        else
                                        {
                                            curSkillData = gm.GetNormalAttack(playerID);
                                            target = node.whoStandEnemy;
                                            state = UnitTurnState.EndOrder;
                                            ableToAction = false;
                                            //orderType = OrderType.Attack;
                                            //적이 스킬을 쓸수있는 위치에 있음
                                            if (gridManager.GetRange(transform.position, node.pos) <= gm.GetUnitStatus(playerID).attackRange)
                                            {
                                                orderType = OrderType.Attack;
                                                curSkillData.SkillEnter(this, target, null);
                                                ReduceActionPoint();

                                            }
                                            //적이 스킬을 쓸수있는 위치에 밖에 있으므로 이동후 공격
                                            else
                                            {
                                                ReduceActionPoint();
                                                ReduceActionPoint();
                                                moveState.FSMEnter(this);
                                                orderType = OrderType.Move;
                                                scheduledAction = OrderPlayerType.Attack;
                                            }

                                        }
                                        //적일 경우 일반 공격하기

                                    }

                                    break;
                                case TileWhoStay.Player:
                                    break;
                                case TileWhoStay.Trap:
                                    break;
                                default:
                                    break;
                            }
                        }

                        break;
                    case OrderPlayerType.Skill1:
                        UseSkill(unitStatus, hit, node, OrderPlayerType.Skill1);
                        break;
                    case OrderPlayerType.Skill2:
                        UseSkill(unitStatus, hit, node, OrderPlayerType.Skill2);
                        break;
                    case OrderPlayerType.Skill3:
                        UseSkill(unitStatus, hit, node, OrderPlayerType.Skill3);
                        break;
                    case OrderPlayerType.Skill4:
                        UseSkill(unitStatus, hit, node, OrderPlayerType.Skill4);
                        break;
                    case OrderPlayerType.Item1:
                        UseItem(unitStatus, hit, node, OrderPlayerType.Item1);
                        break;
                    case OrderPlayerType.Item2:
                        UseItem(unitStatus, hit, node, OrderPlayerType.Item2);
                        break;
                    case OrderPlayerType.Item3:
                        UseItem(unitStatus, hit, node, OrderPlayerType.Item3);
                        break;
                    default:
                        break;
                }
            }



            //땅을 클릭했을 경우
        }

    }

    public void ShowMoveAbleTile()
    {
        if (actionPoint > 0)
        {
            graphicManager.CleaAllTileNode();
            var unitStatus = gm.GetUnitStatus(playerID);
            orderActivateType = OrderPlayerType.Move;
            if (onTile.nodeInShadow)
            {
                allowToClickTile = graphicManager.ShowClickAbleTile(transform.position, unitStatus.maxMoveRange + 1, true);
            }
            else
            {
                allowToClickTile = graphicManager.ShowClickAbleTile(transform.position, unitStatus.maxMoveRange, true);
            }
        }


    }

    void DrawSoundArea(Vector2Int index, OrderPlayerType skill)
    {
        //var tempSkillData = gm.GetUnitStatus(playerID).skillInfo[(int)skill];
        var tempSkillData = gm.GetSkillDataFromPlayer(playerID, (int)skill);
        graphicManager.ShowSoundNode(index, tempSkillData.sound);
    }
    public void AddPlayerOnTile()
    {

        //Debug.Log($"AddPlayerOnTile");

        if (onTile != null)
        {
            onTile.RemovePlayer();
        }
        if (onTile.enemyWatch)
        {
            lastNodeWitchEnemyWatch = onTile;
            graphicManager.DrawTile(onTile);
        }

        onTile = gridManager.GetNode(transform.position);
        onTile.AddPlayer(this);
        graphicManager.DrawTile(onTile);
    }
    private void DrawPathByLine(RaycastHit hit, Vector2Int index)
    {
        graphicManager.TurnOffTargetMark();
        previousShowPathIndex = index;
        //적시야밖에서 경로 검색
        paths = null;
        if (!battleMode)
        {
            paths = gridManager.pathfindingSystem.FindPath(transform.position, hit.point, GetMaxMoveRange(), true, false);
            needToMoveUnderEnemySight = false;
        }


        //적시야밖에서 경로가 존재하지않으므로 적경로안에서 검색
        if (paths == null)
        {
            paths = gridManager.pathfindingSystem.FindPath(transform.position, hit.point, GetMaxMoveRange(), false, false);
            needToMoveUnderEnemySight = true;
        }

        //경로가 존재함
        if (paths != null && paths.path.Count > 0)
        {
            if (needToMoveUnderEnemySight)
            {
                //Debug.Log($"적시야 밖에서의 경로 {paths.Count}");
            }
            else
            {
                //Debug.Log($"적시야 안에서의 경로 {paths.Count}");
            }

            //경로 그려주기

            graphicManager.SetPathLine(paths);
            graphicManager.TurnOnTargetMark(gridManager.GetNode(hit.point).pos);
            hasPath = true;
        }
        //경로가 존재하지않음
        else
        {
            graphicManager.ClearPath();
            hasPath = false;
        }
    }

    private void UseSkill(PlayerUnitStatus unitStatus, RaycastHit ray, Node node, OrderPlayerType skill)
    {
        var tempSkillData = gm.GetSkillDataFromPlayer(playerID, (int)skill);

        Debug.Log($"이전위치: {previousShowPathIndex} 현재 클릭위치: {node.index} 노드타입: {node.tileWhoStay} 스킬타겟타입: {tempSkillData.targetTile}");

        //경로안에 있을경우 && 이전좌표와 같지 않을경우
        if (previousShowPathIndex != node.index)
        {
            //DrawPathByLine(ray, node.index);
            previousShowPathIndex = node.index;
            switch (node.tileWhoStay)
            {
                case TileWhoStay.None:
                    targetNode = node;
                    Debug.Log($"스킬 {(int)skill} {node.index} 발동");
                    break;
                case TileWhoStay.Enemy:
                    target = node.whoStandEnemy;
                    Debug.Log($"스킬 {(int)skill} 발동");
                    break;
                case TileWhoStay.Player:
                    break;
                case TileWhoStay.Trap:
                    break;
                default:
                    break;
            }
            DrawSoundArea(node.index, skill);
        }
        else
        {
            //해당 타일이 타겟 타일일때
            //if (unitStatus.skillInfo[(int)skill].targetTile == node.tileWhoStay)
            if (tempSkillData.targetTile == node.tileWhoStay)
            {

                //var tempSkillData = unitStatus.skillInfo[(int)skill];
                //해당스킬의 사용여부
                switch (node.tileWhoStay)
                {
                    case TileWhoStay.None:
                        targetNode = node;
                        Debug.Log($"스킬 {(int)skill} {node.index} 발동");
                        break;
                    case TileWhoStay.Enemy:
                        target = node.whoStandEnemy;
                        Debug.Log($"스킬 {(int)skill} 발동");
                        break;
                    case TileWhoStay.Player:
                        break;
                    case TileWhoStay.Trap:
                        break;
                    default:
                        break;
                }


                state = UnitTurnState.EndOrder;
                orderType = OrderType.Skill;
                ableToAction = false;
                curSkillData = tempSkillData;
                curSkillData.SkillEnter(this, target, node);
                gm.UseSkill(playerID, unitStatus, skill);
                ReduceActionPoint();
            }
        }

    }
    private void UseItem(PlayerUnitStatus unitStatus, RaycastHit ray, Node node, OrderPlayerType item)
    {
        var tempSkillData = gm.GetSkillDataFromItem(playerID, (int)item - 10);

        Debug.Log($"이전위치: {previousShowPathIndex} 현재 클릭위치: {node.index} 노드타입: {node.tileWhoStay} 스킬타겟타입: {tempSkillData.targetTile}");

        //경로안에 있을경우 && 이전좌표와 같지 않을경우
        if (previousShowPathIndex != node.index)
        {
            //DrawPathByLine(ray, node.index);
            previousShowPathIndex = node.index;
            ////DrawSoundArea(node.index, item);
        }
        else
        {
            //해당 타일이 타겟 타일일때
            //if (unitStatus.skillInfo[(int)skill].targetTile == node.tileWhoStay)
            if (tempSkillData.targetTile == node.tileWhoStay)
            {

                //var tempSkillData = unitStatus.skillInfo[(int)skill];
                //해당스킬의 사용여부
                switch (node.tileWhoStay)
                {
                    case TileWhoStay.None:
                        targetNode = node;
                        Debug.Log($"스킬 {(int)item} {node.index} 발동");
                        break;
                    case TileWhoStay.Enemy:
                        target = node.whoStandEnemy;
                        Debug.Log($"스킬 {(int)item} 발동");
                        break;
                    case TileWhoStay.Player:
                        break;
                    case TileWhoStay.Trap:
                        break;
                    default:
                        break;
                }


                state = UnitTurnState.EndOrder;
                orderType = OrderType.Skill;
                ableToAction = false;
                curSkillData = tempSkillData;
                curSkillData.SkillEnter(this, target, node);
                gm.UseItem(playerID, unitStatus, item);
                ReduceActionPoint();
            }
        }

    }

    public void ConnectWithInputManager()
    {
        InputManager.Instance.ClickButtonA += PlayerInput;
        InputManager.Instance.ActiveInput();
    }

    public void DisconnectWithInputManager()
    {
        InputManager.Instance.ClickButtonA -= PlayerInput;
        InputManager.Instance.DeactiveInput();
    }





    public bool ShowSkillRangeTagetTile(int skillIndex)
    {
        var tempSkillData = gm.GetSkillDataFromPlayer(playerID, skillIndex);

        Debug.Log($"{gm.GetUnitStatus().mana} + { tempSkillData.cost} ");

        if (tempSkillData != null && gm.GetUnitStatus().mana >= tempSkillData.cost)
        {
            graphicManager.CleaAllTileNode();
            orderActivateType = (OrderPlayerType)skillIndex;
            allowToClickTile = graphicManager.ShowClickAbleTile(transform.position, tempSkillData.distance, false, true, tempSkillData.ignoreObstacle);
            graphicManager.ClearPath();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool ShowItemRangeTagetTile(int itemIndex)
    {
        var tempSkillData = gm.GetItemDataFromPlayer(playerID, itemIndex);
        if (tempSkillData != null)
        {
            graphicManager.CleaAllTileNode();
            orderActivateType = (OrderPlayerType)itemIndex + 10;
            allowToClickTile = graphicManager.ShowClickAbleTile(transform.position, tempSkillData.distanceTile, false, true, true);
            graphicManager.ClearPath();
            return true;
        }
        else
        {
            return false;
        }

    }



    public void AddEnemyWhoWatchingThisPlayerUnit(EnemyController enemyController)
    {
        if (!enemyWhoWatchingThisPlayer.Contains(enemyController))
        {
            enemyWhoWatchingThisPlayer.Add(enemyController);
            lastNodeWitchEnemyWatch = onTile;
            battleMode = true;
            animator.SetBool("BattleMode", true);
            enemyWachingThisTarget = true;
        }

    }

    public void RemoveEnemyWhoWatchingThisPlayerUnit(EnemyController enemyController)
    {
        if (enemyWhoWatchingThisPlayer.Contains(enemyController))
        {
            enemyWhoWatchingThisPlayer.Remove(enemyController);
            if (enemyWhoWatchingThisPlayer.Count == 0)
            {
                battleMode = false;
                animator.SetBool("BattleMode", false);
                enemyWachingThisTarget = false;
            }

        }
        else
        {

            Debug.LogError($"{enemyController.gameObject.name}은 {gameObject.name}을 보고있지않습니다.");

        }

    }


    private void OnDrawGizmos()
    {
        if (paths != null)
        {
            for (int i = 1; i < paths.path.Count; i++)
            {
                Gizmos.DrawLine(paths.path[i - 1].pos + Vector3.up, paths.path[i].pos + Vector3.up);
            }
        }

        if (gridManager != null)
        {
            var direction = -gridManager.directionalLight.transform.forward;
            Gizmos.DrawLine(transform.position, transform.position + direction * 10);
        }
    }

    public override Vector3 GetTargetPosition()
    {
        return target.transform.position;
    }

    public override bool CheckTargetInAttackRange()
    {
        return false;
    }
    public override int GetMaxMoveRange()
    {
        if (onTile.nodeInShadow)
        {
            return gm.GetUnitStatus(playerID).maxMoveRange + 1;
        }
        else
        {
            return gm.GetUnitStatus(playerID).maxMoveRange;
        }
     
    }

    public void HideFromEnemyWhoWatchThisPlayer()
    {

        if (enemyWachingThisTarget && !onTile.enemyWatch)
        {
            for (int i = 0; i < enemyWhoWatchingThisPlayer.Count; i++)
            {
				if (enemyWhoWatchingThisPlayer[i] == null) 
				{
                    enemyWhoWatchingThisPlayer.RemoveAt(i);
                    i--;
                    continue;
                }
                if (enemyWhoWatchingThisPlayer[i].CheckTargetInSight(onTile))
                {
                    for (int j = 0; j < enemyWhoWatchingThisPlayer.Count; j++)
                    {
                        enemyWhoWatchingThisPlayer[i].LookTarget();
                    }
                    //HideTargetMark();
                    Debug.Log($"적시야 내에 위치합니다.");
                    return;

                }
            }

            Debug.Log($"적시야 밖으로 나왔습니다.");

            enemyWachingThisTarget = false;

            var markController = graphicManager.LeaveTargetMark(lastNodeWitchEnemyWatch);
            for (int i = 0; i < enemyWhoWatchingThisPlayer.Count; i++)
            {
                enemyWhoWatchingThisPlayer[i].MissingTarget(lastNodeWitchEnemyWatch);
                markController.AddEnemy(enemyWhoWatchingThisPlayer[i]);
            }
            //LeaveTargetMark(lastNodeWitchEnemyWatch);
            enemyWhoWatchingThisPlayer.Clear();
        }
    }

    //public void LeaveTargetMark(Node node)
    //{
    //    targetMark.transform.position = node.pos;
    //    targetMark.gameObject.SetActive(true);
    //}

    public void HideTargetMark()
    {
        targetMark.gameObject.SetActive(false);
    }

    public void SetEnemyWhoWatchPlayerTile()
    {
        if (onTile.enemyWatch)
        {
            onTile.SetEnemyHasTarget(this);
        }
    }

    public void UseSkillEffect()
    {
        curSkillData.SkillTrigger(this, target, targetNode);
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
                skillState.FSMUpdate(this);
                break;
            case OrderType.Skill:
                skillState.FSMUpdate(this);
                break;
            default:
                break;
        }
        //Debug.Log($"{transform.position}");
    }

    public override void Damaged(int damage)
    {
        var unitStatus = gm.GetUnitStatus(playerID);
        if (unitStatus.hp - damage > 0)
        {
            unitStatus.hp -= damage;
            UIManager.Instance.SetHP(unitStatus.hp / (float)unitStatus.maxHP);
            //체력 text 갱신용 함수
            UIManager.Instance.receiveStatusHP(unitStatus.hp, unitStatus.maxHP);
          
            var damageDisplay = gm.objectPool.RequestObject(PrefabID.DamageTexture) as DamageDisplayController;
            damageDisplay.ShowText(damage, transform.position);
            animator.SetInteger("TransitionIndex", (int)AnimeID.Hit);

        }
        else
        {
            Dead();
        }
    }
    public void UseSkillSemiEffect()
    {
        curSkillData.MultiSkillEffect(this, target, targetNode);
    }




    public override void Heal(int damage)
    {
        var unitStatus = gm.GetUnitStatus(playerID);
        var damageDisplay = gm.objectPool.RequestObject(PrefabID.DamageTexture) as DamageDisplayController;

        if (unitStatus.hp + damage < unitStatus.maxHP)
        {
            unitStatus.hp += damage;
            damageDisplay.ShowText(damage, transform.position);

        }
        else
        {
            damageDisplay.ShowText(unitStatus.maxHP - unitStatus.hp, transform.position);
            unitStatus.hp = unitStatus.maxHP;
        }

        UIManager.Instance.SetHP(unitStatus.hp / (float)unitStatus.maxHP);
        //체력 text 갱신용 함수
        UIManager.Instance.receiveStatusHP(unitStatus.hp, unitStatus.maxHP);


    }


    public override void ChangePositionAnimation(UnitPositionState unitPositionState, bool mustDo = false)
    {
        if (mustDo)
        {
            if (ableToChangePosition && posState != unitPositionState)
            {
                SetPosition(unitPositionState);
                ableToChangePosition = false;
            }
        }
        else
        {
            if (actionPoint > 0 && ableToChangePosition && posState != unitPositionState)
            {
                SetPosition(unitPositionState);
                ReduceActionPoint();
                ableToChangePosition = false;
                
            }
        }

    }

public override void Dead()
    {
        gm.PlayableUnitDie(playerID);
    }

    public override void ReduceActionPoint()
    {
        actionPoint--;
        UIManager.Instance.ReduceActionPoint();
    }
}

