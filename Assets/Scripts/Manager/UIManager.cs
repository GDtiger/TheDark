using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    GameManager gm;

    #region instance
    static UIManager instance = null;



    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    #endregion

    public SkillButtonController[] skillButtonControllers;
    public PocketSlotController[] itemButtonControllers;


    [TabGroup("Ref", "Need")] public List<Image> waitQueueImage;


    public GameObject waitQueueUIObject;
    TurnManager tm;
    public List<Image> go = new List<Image>();


    [TabGroup("Ref", "Need_UI")] public Image playerHPBar;
    [TabGroup("Ref", "Need_UI")] public Image posImage;
    [TabGroup("Ref", "Need_UI")] public Image playerSkillGaugeBar;
    [TabGroup("Ref", "Need_UI")] public Image enemyHPBar;
    [TabGroup("Ref", "Need_UI")] public TMP_Text accText;
    [TabGroup("Ref", "Need_UI")] public TMP_Text playerSkillGaugeTxet;
    [TabGroup("Ref", "Need_UI")] public TMP_Text playerHpBarTxet;
    [TabGroup("Ref", "Need_UI")] public TMP_Text enemyHpBarTxet;
    [TabGroup("Ref", "Need_UI")] public GameObject positionDetail;
    [TabGroup("Ref", "Need_UI")] public GameObject enemyInfoUI;
    [TabGroup("Ref", "Need_UI")] public List<GameObject> actionPointObjects;

    EnemyController curShowInfoEnemy;
    public TextMeshProUGUI gold;

    [Header("사망시 나오는 GUI")]
    [TabGroup("Ref", "Need")] public GameObject DeadWindow;

    [Header("승리시 나오는 GUI")]
    [TabGroup("Ref", "Need")] public GameObject winWindow;

    public void Initiate()
    {
        gm = GameManager.Instance;
        DrawSkillIcon(0, true);
        DrawItemIcon();
        DeadWindow.SetActive(false);
        gold.text = $"{gm.gold}";    
        tm = TurnManager.Instance;
        ShowHidePositionButton();
        OnOffEnemyInfo(null, false);
    }

    public void OnOffEnemyInfo(EnemyController enemyController, bool enemyInfoOn) {
		if (!enemyInfoOn && enemyController == curShowInfoEnemy)
		{
            enemyInfoUI.SetActive(false);
		}
		else
		{
            enemyInfoUI.SetActive(true);
        }

    }
    public void ShowHidePositionButton() {
     
        if (positionDetail.activeSelf)
        {
            positionDetail.SetActive(false);
        }
        else
        {
            positionDetail.transform.localScale = Vector3.zero;
            positionDetail.transform.DOScale(Vector3.one, 0.5f);
            positionDetail.SetActive(true);
        }
    }
    public void DrawSkillIcon(int index, bool InitiateSkillButton = false)
    {
        for (int i = 0; i < skillButtonControllers.Length; i++)
        {
            var unitStatus = gm.unitStatus[index];
            if (unitStatus.skills.Count > i)
            {
                var skillInfo = gm.GetSkillDataFromDB(unitStatus.skills[i]);
                if (InitiateSkillButton)
                {
                    skillButtonControllers[i].InitiateButton(skillInfo.sprite);
                }
                skillButtonControllers[i].DrawButton(unitStatus.skillCoolTimeRemain[i]);
            }
            else
            {
                skillButtonControllers[i].gameObject.SetActive(false);
            }
        }
    }

    public void DrawItemIcon() {
        for (int i = 0; i < itemButtonControllers.Length; i++)
        {
            var unitStatus = gm.GetUnitStatus();
            if (unitStatus.pocket.Count > i && unitStatus.pocket[i] != null)
            {
                var tempItem = unitStatus.pocket[i];
                itemButtonControllers[i].DrawButton(gm.GetImage(tempItem.imageID), gm.GetUnitStatus().itemCoolTimeRemain[i], tempItem.count);
            }
            else
            {
                itemButtonControllers[i].DrawButton();
            }
        }
    }



    public void DrawAccPoint(bool battleMode, PosDirection posDirection)
    {
        if (battleMode)
        {
            switch (posDirection)
            {
                case PosDirection.Forward:
                    accText.text = "45%";
                    break;
                case PosDirection.Side:
                    accText.text = "75%";
                    break;
                case PosDirection.Back:
                    accText.text = "90%";
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (posDirection)
            {
                case PosDirection.Forward:
                    accText.text = "30%";
                    break;
                case PosDirection.Side:
                    accText.text = "65%";
                    break;
                case PosDirection.Back:
                    accText.text = "80%";
                    break;
                default:
                    break;
            }
        }

    }


    public void ActiveItemButton(int i)
    {
        DeactiveAllButton();
        itemButtonControllers[i].AcctiveButton();
    }

    public void ActiveSkillButton(int i)
    {
        DeactiveAllButton();
        skillButtonControllers[i].AcctiveButton();
    }

    public void DeactiveAllButton()
    {
        foreach (var button in skillButtonControllers)
        {
            button.DeactiveButton();
        }

        foreach (var button in itemButtonControllers)
        {
            button.DeactiveButton();
        }
    }

    public void SetHP(float ratio)
    {
        playerHPBar.fillAmount = ratio;
    }
    public void receiveStatusHP(int curVar, int maxVar)
    {
        playerHpBarTxet.text = $"{curVar}/{maxVar}";
    }

    public void SetActionPoint(int curVar, int maxVar)
    {
        float ratio = curVar / (float)maxVar;
        playerSkillGaugeBar.fillAmount = ratio;
        playerSkillGaugeTxet.text = $"{curVar}/{maxVar}";
    }

    public void SetEnemyHP(EnemyController enemy, float curvar, float maxvar)
    {
        float ratio = curvar / maxvar;
        curShowInfoEnemy = enemy;
        enemyHPBar.fillAmount = ratio;
        enemyHpBarTxet.text = $"{curvar}/{maxvar}";
        OnOffEnemyInfo(enemy, true);
    }

    public void SetAccText()
    {
        var unitStatus = gm.GetUnitStatus(gm.currentPlayerID);
        accText.text = $"{unitStatus.criticalChance}";
    }

    //에너미 공격자세 ui 변경용 함수
    public void ChangePosImage()
    {
        
    }

    public void ShowYouDiedWindow()
    {
        DeadWindow.SetActive(true);
    }

    public void ShowYouWinWindow()
    {
        winWindow.SetActive(true);
    }

    public void ReturnToTown()
    {
        gm.ReturnToTown();
    }


    //waitimga - > image 저장 리스트
    //go -> 게임오브젝트 리스트
    public void WaitQueueAddUI()
    {

        //자식오브젝트 전부삭제
        var child = waitQueueUIObject.GetComponentsInChildren<Transform>();
        foreach (var item in child)
        {
            if (item != waitQueueUIObject.transform)
            {
                Destroy(item.gameObject);
            }
        }

        //리스트 클리어
        go.Clear();
        //조건 체크 

        //tm.unitsWhoWaitTurnInThisPhase   -> 대기열에 있는 유닛들을 싹 모아둔 리스트
        for (int i = 0; i < tm.unitsWhoWaitTurnInThisPhase.Count; i++)
        {
            if (tm.unitsWhoWaitTurnInThisPhase[i].activeUnit)
            {

                if (tm.unitsWhoWaitTurnInThisPhase[i].unitType == UnitType.Ally)
                {
                    go.Add(Instantiate(waitQueueImage[1], waitQueueUIObject.transform));
                }
                else if (tm.unitsWhoWaitTurnInThisPhase[i].isBoss)
                {
                    go.Add(Instantiate(waitQueueImage[2], waitQueueUIObject.transform));
                }
                else if (tm.unitsWhoWaitTurnInThisPhase[i].unitType == UnitType.Enemy)
                {
                    go.Add(Instantiate(waitQueueImage[0], waitQueueUIObject.transform));
                }
               

            }
        }
    }

    public void ReduceActionPoint() {
        for (int i = actionPointObjects.Count -1; i >= 0; i--)
        {
            if (actionPointObjects[i].activeSelf)
            {
                actionPointObjects[i].SetActive(false);
                if (gm.tutorialMode && TutorialManager.Instance.checkonce)
                {
                    TutorialManager.Instance.OnclickPopupCheck4();
                }
                return;
            }
        }
    }

    public void SetFullActionPoint() {
        for (int i = 0; i < actionPointObjects.Count; i++)
        {
            actionPointObjects[i].SetActive(true);
        }
    }

    public void WaitQueueDeleteUI()
    {

        if (go != null)
        {
            Destroy(go[0].gameObject);
            go.RemoveAt(0);
        }

    }
}