using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Town;

public class TutorialController : MonoBehaviour
{
    #region instance
    static TutorialController instance = null;


    public static TutorialController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TutorialController>();
            }
            return instance;
        }
    }
    #endregion
    public GameManager gm;
    InputManager im;
  
    public GameObject popUp;
    public GameObject TutorialBtnObj;
    public TMPro.TextMeshProUGUI tutorialDescriptionTitle;
    public TMPro.TextMeshProUGUI tutorialDescription;
    public Image image;
    public bool isContinueTutorial = true;
    public bool moveUIBool = false;
    public LayerMask layerMask;
    [SerializeField] public int tutorialNumber;

    public List<Sprite> tutorialSprite; 

    public Transform arrowParent;
    public GameObject arrow;
    public GameObject arrowPrefab;
    public Transform[] arrowPos;
    public Vector3[] arrowPosOffset;
    public MainScenePlayerController mainPlayerController;
    Sequence sequence;
    void Awake()
    {
        popUp.SetActive(false);
        gm = GameManager.Instance;
        im = InputManager.Instance;
        image.gameObject.SetActive(false);
    }
	void Start()
	{
        if (gm.isTutorial)
		{
            StartCoroutine(CoTownTutorialStart());
		}
	}
	public void OnClickTutorialStart()
    {
        popUp.SetActive(true);
        tutorialDescription.text = gm.textDataBase.GetTextData($"{tutorialNumber}").text;
        TutorialBtnObj.SetActive(false);
    }
    public void OnClickPopupCheck()
    {
        gm.soundManager.PlaySFXSound("Confirm");
        mainPlayerController.isMove = true;
        tutorialDescriptionTitle.text = "¾Ë¸²";
        tutorialNumber++;
        popUp.SetActive(false);
        image.gameObject.SetActive(false);
        if (gm.town_UIManager.shopManager.isOpenShop || gm.town_UIManager.inventoryManager.isOpenInventory || gm.town_UIManager.skillManager.isOpenSkillUI
            && tutorialNumber == 0)
        {
            if (isContinueTutorial)
            {
                OpenPopup();
                isContinueTutorial = false;
            }
        }
        if (gm.isTutorial && tutorialNumber == 3)
        {
            InfinityArrow(0, DirEight.B);
        }
        if (gm.isTutorial && tutorialNumber == 4|| tutorialNumber == 10)
		{
            InfinityArrow(1, DirEight.B);
        }
    }
  
    public void OpenPopup()
    {
        mainPlayerController.isMove = false;
        popUp.SetActive(true);
        tutorialDescription.text = gm.textDataBase.GetTextData($"{tutorialNumber}").text;
    }

   


    public void Reward(int i, string s)
    {
        if (s == "Gold")
        {
            gm.gold += i;
        }
        if (s == "Item")
        {
            gm.town_UIManager.inventoryManager.AddItemToInventory
                (gm.town_UIManager.inventoryManager.database.GetItemFromDataBase(i));
        }
        if (s == "Skill")
        {
            gm.unitStatus[0].skillPoint += 1;
            gm.unitStatus[0].curSkillPoint += 1;

        }
    }

    public IEnumerator CoTownTutorialStart()
    {
        yield return new WaitForSeconds(1f);
        mainPlayerController.isMove = false;
        popUp.SetActive(true);
        tutorialDescription.text = gm.textDataBase.GetTextData($"{tutorialNumber}").text;
        TutorialBtnObj.SetActive(false);
    }

    public void InfinityArrow(int index, DirEight dirEight) {
        Debug.Log("InfinityArrow");
        arrow = Instantiate(arrowPrefab, arrowParent);
        float valueSize = 0.8f;
        float valueTime = 0.8f;
        arrow.SetActive(true);
        arrow.GetComponent<RectTransform>().position = arrowPos[index].position + arrowPosOffset[index];
        sequence.Kill();
        switch (dirEight)
        {
            case DirEight.T:
                arrow.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -90), 0);
                sequence = DOTween.Sequence();
                //sequence.AppendCallback(() => arrow.transform.DOMove(pos + Vector3.up * valueSize, 0.5f).SetLoops(-1, LoopType.Yoyo));
                sequence.AppendCallback(() => arrow.transform.DOScale(valueSize, valueTime).SetEase(Ease.Linear).SetLoops(-1));
                break;
            case DirEight.L:
                arrow.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -180), 0);
                sequence = DOTween.Sequence();
                //sequence.AppendCallback(() => arrow.transform.DOMove(pos + Vector3.left * valueSize, 0.5f).SetLoops(-1, LoopType.Yoyo));
                sequence.AppendCallback(() => arrow.transform.DOScale(valueSize, valueTime).SetEase(Ease.Linear).SetLoops(-1));
                break;
            case DirEight.B:
                arrow.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -270), 0);
                sequence = DOTween.Sequence();
                //sequence.AppendCallback(() => arrow.transform.DOMove(pos + Vector3.down * valueSize, 0.5f).SetLoops(-1, LoopType.Yoyo));
                sequence.AppendCallback(() => arrow.transform.DOScale(valueSize, valueTime).SetEase(Ease.Linear).SetLoops(-1));
                break;
            case DirEight.R:
                arrow.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -0), 0);
                sequence = DOTween.Sequence();
                //sequence.AppendCallback(() => arrow.transform.DOMove(pos + Vector3.right * valueSize, 0.5f).SetLoops(-1, LoopType.Yoyo));
                sequence.AppendCallback(() => arrow.transform.DOScale(valueSize, valueTime).SetEase(Ease.Linear).SetLoops(-1));
                break;
            default:
                break;
        }
    }

    public void SetArrow(Vector3 pos, DirEight dirEight, int loopTIme = 10, float killTime = 3)
    {
        Debug.Log("SetArrow");
        arrow = Instantiate(arrowPrefab, arrowParent);
        float valueSize = 100;
        // myTween.OnComplete(null);
        // myTween.SetAutoKill(true);
        arrow.SetActive(true);
        arrow.GetComponent<RectTransform>().position = pos;
        //guid = System.Guid.NewGuid();
        //sequence.id = guid;
        sequence.Kill();
        switch (dirEight)
        {
            case DirEight.T:
                arrow.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -90), 0);
                sequence = DOTween.Sequence();
                sequence.AppendCallback(() => arrow.transform.DOMove(pos + Vector3.up * valueSize, 0.5f).SetLoops(loopTIme, LoopType.Yoyo));
                sequence.InsertCallback(killTime, () => UnArrow());
                break;
            case DirEight.L:
                arrow.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -180), 0);
                sequence = DOTween.Sequence();
                sequence.AppendCallback(() => arrow.transform.DOMove(pos + Vector3.left * valueSize, 0.5f).SetLoops(loopTIme, LoopType.Yoyo));
                sequence.InsertCallback(killTime, () => UnArrow());
                break;
            case DirEight.B:
                arrow.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -270), 0);
                sequence = DOTween.Sequence();
                sequence.AppendCallback(() => arrow.transform.DOMove(pos + Vector3.down * valueSize, 0.5f).SetLoops(loopTIme, LoopType.Yoyo));
                sequence.InsertCallback(killTime, () => UnArrow());
                break;
            case DirEight.R:
                arrow.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -0), 0);
                sequence = DOTween.Sequence();
                sequence.AppendCallback(() => arrow.transform.DOMove(pos + Vector3.right * valueSize, 0.5f).SetLoops(loopTIme, LoopType.Yoyo));
                sequence.InsertCallback(killTime, () => UnArrow());
                break;
            default:
                break;
        }
    }
    public void UnArrow()
    {
        Debug.Log("UnArrow");
        sequence.Kill();
        //DOTween.Kill(guid);
        //arrow.SetActive(false);
        Destroy(arrow);
    }
    public void Arrow()
    {
        arrow.SetActive(true);
        Invoke("unArrow", 3f);
    }
    public void unArrow()
    {
        arrow.SetActive(false);
    }
}
