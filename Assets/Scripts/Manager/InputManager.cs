using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public GraphicManager graphicManager;
    public GameManager gm;
    public CameraController cameraController;
    public SKillDescriptionController descriptionController;

    #region instance
    static InputManager instance = null;
    public bool activeInput;

    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InputManager>();
            }
            return instance;
        }
    }
    #endregion

    public Action ClickButtonA;
    public Action ClickButtonB;
    public Action ClickNextTurn;

    public int curActiveSkill = -1;
    public int curActiveItem = -1;


    public bool[] skillIsActivated;
    public int skillCount = 3;

    public LayerMask layerMask;
    RaycastHit hit;



    public float zoomSpeed = 0.5f;

    public bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrentPosition
            = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = touchPos;

        List<RaycastResult> results = new List<RaycastResult>();


        EventSystem.current
        .RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }



    void Update()
    {
        if (activeInput)
        {
            if (Input.GetMouseButtonDown(0))
            {

                //if (!IsPointerOverUIObject(Input.mousePosition))
                //{
                //    ClickButtonA?.Invoke();
                //    CheckEnemy();
                //}

                ClickButtonA?.Invoke();
                CheckEnemy();
            }
        }

        if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0); //ù��° �հ��� ��ǥ
            Touch touchOne = Input.GetTouch(1); //�ι�° �հ��� ��ǥ

            // deltaposition�� deltatime�� �����ϰ� delta��ŭ �ð����� ������ �Ÿ��� ���Ѵ�.

            // ���� position���� ���� delta���� ���ָ� �����̱� ���� �հ��� ��ġ�� �ȴ�.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // ����� ���Ű��� �������� ũ�⸦ ���Ѵ�.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // �� ���� ���̴� �� Ȯ��/����Ҷ� ��ŭ ���� Ȯ��/��Ұ� ����Ǿ�� �ϴ����� �����Ѵ�.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            cameraController.ZoomCamera(deltaMagnitudeDiff * zoomSpeed);
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            cameraController.ZoomCamera(Input.mouseScrollDelta.y * zoomSpeed);
        }
    }

    //public bool IsPointerOverUIObject(Vector2 touchPos)
    //{
    //    PointerEventData eventDataCurrentPosition
    //        = new PointerEventData(EventSystem.current);

    //    eventDataCurrentPosition.position = touchPos;

    //    List<RaycastResult> results = new List<RaycastResult>();


    //    EventSystem.current
    //    .RaycastAll(eventDataCurrentPosition, results);

    //    return results.Count > 0;
    //}

    public void Initiate()
    {
        skillIsActivated = new bool[skillCount];
        graphicManager = GraphicManager.Instance;
        gm = GameManager.Instance;
        enabled = true;
    }
    public void CheckEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
        {

            Debug.Log(hit.transform.gameObject.name);
            var enemy = hit.transform.GetComponent<EnemyController>();
            var enemyHp = enemy.unitStatus.hp;
            var enemyMaxHp = enemy.unitStatus.enemyBaseInformation.maxHP;

            UIManager.Instance.SetEnemyHP(enemy, enemyHp, enemyMaxHp);
            PosDirection posDirection = PosDirection.Back;



            var playerPos = TurnManager.Instance.curPlayerUnits[0].transform.position;
            playerPos.y = 0;

            var enemyPos = enemy.transform.position;
            enemyPos.y = 0;

            var angle = (enemyPos - playerPos);
            var angle2 = Vector3.Angle(enemy.transform.forward, angle);

            if (angle2 <= 45 && angle2 > -45)
            {
                posDirection = PosDirection.Back;
            }
            else if (angle2 <= 135 && angle2 > -135)
            {
                posDirection = PosDirection.Side;
            }
            else
            {
                posDirection = PosDirection.Forward;
            }

   //         if (enemy.lookAt == TurnManager.Instance.curPlayerUnits[0].lookAt)
			//{
   //             posDirection = PosDirection.Back;
			//}
			//else 
			//{
   //             posDirection = PosDirection.Forward;
   //         }
            UIManager.Instance.DrawAccPoint(enemy.battleMode, posDirection);
        }

    }
    public void ActiveInput()
    {
        activeInput = true;
        curActiveSkill = -1;
        
    }

 

    public void DeactiveInput()
    {
        activeInput = false;

    }

    public void ActiveSkill(int index)
    {
        if (!activeInput) return;
        var actionUnit = TurnManager.Instance.actingUnit;
        graphicManager.ClearPath();
        //�Ʊ��϶� ����
        if (actionUnit.unitType == UnitType.Ally)
        {
            PlayerController playerUnit = (PlayerController)actionUnit;
            //��ų��ư�� �ȴ��� �����̰ų� �ٸ���ư�� ������
            //if (!playerUnit.ableToAction) return;

            descriptionController.SetSkillTooltip(index);
            if (!gm.GetUnitStatus(playerUnit.playerID).AbleToUseSkill(index)) return;

            Debug.Log("Show Skill Active Icon");

            if (curActiveSkill == -1 || curActiveSkill != index)
            {
                if (playerUnit.ShowSkillRangeTagetTile(index))
                {
                    Debug.Log("Show Skill Active Icon1");
                    curActiveSkill = index;
                    UIManager.Instance.ActiveSkillButton(index);
                }
            }

            //��ų��ư Ȱ��ȭ���¿��� �ٽ� ��������
            else if (curActiveSkill == index)
            {
                Debug.Log("Off Skill Active Icon1");
                curActiveSkill = -1;
                playerUnit.ShowMoveAbleTile();
                UIManager.Instance.DeactiveAllButton();
                descriptionController.OffSkillTooptip();
            }
        }
    }

    public void ActiveItem(int index)
    {
        if (!activeInput) return;
        var actionUnit = TurnManager.Instance.actingUnit;
        graphicManager.ClearPath();
        //�Ʊ��϶� ����
        if (actionUnit.unitType == UnitType.Ally)
        {
            PlayerController playerUnit = (PlayerController)actionUnit;
            //��ų��ư�� �ȴ��� �����̰ų� �ٸ���ư�� ������
            //if (!playerUnit.ableToAction) return;
            if (!gm.GetUnitStatus(playerUnit.playerID).AbleToUseItem(index)) return;
            if (curActiveItem == -1 || curActiveItem != index)
            {
                if (playerUnit.ShowItemRangeTagetTile(index))
                {
                    curActiveItem = index;
                    UIManager.Instance.ActiveItemButton(index);
                }
            }

            //��ų��ư Ȱ��ȭ���¿��� �ٽ� ��������
            else if (curActiveItem == index)
            {
                curActiveItem = -1;
                playerUnit.ShowMoveAbleTile();
                UIManager.Instance.DeactiveAllButton();
            }



        }

    }

    public void DeactiveSkill()
    {

    }
}

