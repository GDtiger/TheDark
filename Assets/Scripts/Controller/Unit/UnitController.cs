

using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitController : MonoBehaviour
{
    [TabGroup("Ref", "Auto")] [SerializeField] public GameManager gm;
    [TabGroup("Ref", "Auto")] [SerializeField] public Animator animator;
    [TabGroup("Ref", "Auto")] [SerializeField] public GridManager gridManager;
    [TabGroup("Ref", "Auto")] [SerializeField] public TurnManager turnManager;
    [TabGroup("Ref", "Auto")] [SerializeField] public GraphicManager graphicManager;
    [SerializeField] public Path paths;
    [SerializeField] public Node onTile;
    [TabGroup("Parameter", "Parameter")] public int unitID;

    [TabGroup("Parameter", "Parameter")] public UnitTurnState state;
    [TabGroup("Parameter", "Parameter")] public UnitPositionState posState;
    [TabGroup("Parameter", "Parameter")] public OrderType orderType;
    [TabGroup("Parameter", "Parameter")] public UnitType unitType;
    [TabGroup("Parameter", "Parameter")] public bool isBoss;
    [TabGroup("Parameter", "Parameter")] public DirEight lookAt;
    [TabGroup("Parameter", "Parameter")] public bool ownTurn;
    [TabGroup("Parameter", "Parameter")] public bool activeUnit;
    //[TabGroup("Parameter", "Parameter")] public bool actionEnd;


    [Header("이동가능한 남은 거리")]
    [TabGroup("Parameter", "Parameter")] public int remainAbletoMoveRange = 0;


    [Header("행동 가능 여부 (일반공격가능 여부)")]
    [TabGroup("Parameter", "Parameter")] public bool ableToAction;


    [TabGroup("Parameter", "Parameter")] public int actionPoint = 2;
    [TabGroup("Parameter", "Parameter")] public int turnSpeed = 5;
    [TabGroup("Parameter", "Parameter")] public Vector2Int posBeforeAction;
    [TabGroup("Parameter", "Parameter")] public bool actionThisTurn;
    [TabGroup("Parameter", "Parameter")] public bool battleMode;




    [Tooltip("전투 모드")]
    //[TabGroup("Parameter", "BattleSetting")] public bool battleMode;



    [TabGroup("Parameter", "BattleSetting")] public bool useCustomParameter = false;
    [TabGroup("Parameter", "BattleSetting")] public float closeDistance = 0.01f;
    [TabGroup("Parameter", "BattleSetting")] public float moveSpeed = 5;
    [TabGroup("Parameter", "BattleSetting")] public float unitRotateSpeed = 5;
    public virtual void Initiate()
    {

        paths = new Path();
        if (gm == null)
            gm = GameManager.Instance;
        if (animator == null)
            animator = GetComponent<Animator>();
        if (gridManager == null)
            gridManager = GridManager.Instance;
        if (turnManager == null)
            turnManager = TurnManager.Instance;
        if (graphicManager == null)
            graphicManager = GraphicManager.Instance;

 

        if (!useCustomParameter)
        {
            closeDistance = turnManager.closeDistance;
            moveSpeed = turnManager.moveSpeed;
            unitRotateSpeed = turnManager.unitRotateSpeed;
        }

    }

    public void SetDirection()
    {
        float angle = transform.rotation.eulerAngles.y;
        if (angle > 315)
        {
            angle -= 360;
        }
        if (angle > -45 && angle <= 45)
        {
            lookAt = DirEight.T;
        }
        else if (angle > 45 && angle <= 135)
        {
            lookAt = DirEight.R;
        }
        else if (angle > 135 && angle <= 225)
        {
            lookAt = DirEight.B;
        }
        else if (angle > 225 && angle <= 315)
        {
            lookAt = DirEight.L;
        }
        else
        {

            Debug.LogError(transform.rotation.eulerAngles.y);

        }
    }
    public virtual void Unit_EndProcessing() {

    }

    public virtual void StartTurn() {

        state = UnitTurnState.OrderInitiate;
        SetPosition(UnitPositionState.Nomal);
    }



    public abstract Vector3 GetTargetPosition();

    public abstract bool CheckTargetInAttackRange();

    public virtual void Unit_EndTurn() {
        //Debug.Log($"{gameObject.name}의 턴을 종료합니다");
        ownTurn = false;
        state = UnitTurnState.WaitTurn;
    }

    public virtual void OnEnable()
    {
       
    }

    public virtual void Unit_EndOrder() {

        state = UnitTurnState.Processing;
    }
    public virtual void Unit_SetOrder()
    {

    }

    public abstract void UnitUpdate();
    protected void Processing_Idle()
    {
        state = UnitTurnState.EndProcessing;
    }

    //public abstract void ProcessAfterMovingToTargetNode();

    public void EndProcessing() {
        Unit_EndProcessing();
    }


    public virtual void Unit_OrderInitiate() {

    }

    public bool CheckMiss(UnitController unit, float percent = 0)
    {
        var ranVar = Random.Range(0, 100);
		//후방인경우
		if (unit.lookAt == lookAt)
		{
			return ranVar > 90 + percent ? false : true;
		}
		//서로 바라보는 경우
		else if (Mathf.Abs((int)unit.lookAt - (int)lookAt) == 2)
		{
			return ranVar > 45 + percent ? false : true;
		}
		//측면인 경우
		else
		{
			return ranVar > 65 + percent ? false : true;
		}

		return true;
    }
    public virtual void ChangePositionAnimation(UnitPositionState unitPositionState, bool mustDo = false) {


    }

    internal void SetPosition(UnitPositionState unitPositionState)
    {
        posState = unitPositionState;
        animator.SetInteger("Position", (int)unitPositionState);
    }

    public abstract void Damaged(int damage);
    public virtual void Heal(int damage) { }


    public abstract void Dead();

    public abstract int GetMaxMoveRange();

    public virtual void ReduceActionPoint() {
        actionPoint--;
    }
}
