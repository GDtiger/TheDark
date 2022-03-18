using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class TutorialManager : MonoBehaviour
{

    #region instance
    static TutorialManager instance = null;


    public static TutorialManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TutorialManager>();
            }
            return instance;
        }
    }
    #endregion
    GameManager gm;
    InputManager im;
    public GameObject[] popUp;
    public GameObject[] tutorialObject;
    public GameObject arrow;
    public LayerMask layerMask;
    public Vector3[] posArrow;
    public Guid guid;
    // Start is called before the first frame update
    //Tween myTween;
    Sequence sequence;
    void Start()
    {
        
        gm = GameManager.Instance;
        gm.tutorialMode = true;
        im = InputManager.Instance;
        for (int i = 0; i < tutorialObject.Length; i++)
        {
            tutorialObject[i].SetActive(false);
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(checkonce ==true)
            OnclickPopupCheck3();
        }
    }
    public void OnclickPopupCheck()
    {
        Debug.Log($"1");
        im.activeInput = false;
        popUp[0].SetActive(false);
        popUp[1].SetActive(true);
        SetArrow(posArrow[0], DirEight.T);
    }
    public void OnclickPopupCheck1()
    {
        Debug.Log($"2");
        im.activeInput = false;
        popUp[1].SetActive(false);
        popUp[2].SetActive(true);
        SetArrow(posArrow[1], DirEight.L);

    }
    public void OnclickPopupCheck2()
    {
        Debug.Log($"3");
        im.activeInput = false;
        popUp[2].SetActive(false);
        popUp[3].SetActive(true);
        //SetArrow(posArrow[2], DirEight.B);
    }

    RaycastHit hit;
    public bool checkonce = true;
    public void OnclickPopupCheck3()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
        {
            im.activeInput = true;
            popUp[3].SetActive(false);

            //Invoke("Arrow", 3f);
            
        }

    }


    public void SetArrow(Vector3 pos, DirEight dirEight) {
        Debug.Log("sa");
        float valueSize = 100 ;
        float killTime = 3;
        int loopTIme = 10;
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
                arrow.GetComponent<RectTransform>().DORotate(new Vector3(0, 0,-90), 0);
                sequence = DOTween.Sequence();
                sequence.AppendCallback(() => arrow.transform.DOMove(pos + Vector3.up * valueSize, 0.5f).SetLoops(loopTIme, LoopType.Yoyo));
                sequence.InsertCallback(killTime, () => UnArrow());
				break;
			case DirEight.L:
                arrow.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -180), 0);
                sequence = DOTween.Sequence();
                sequence.AppendCallback(() => arrow.transform.DOMove(pos+ Vector3.left * valueSize, 0.5f).SetLoops(loopTIme, LoopType.Yoyo));
                sequence.InsertCallback(killTime, () => UnArrow());
                break;
            case DirEight.B:
                arrow.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -270), 0);
                sequence = DOTween.Sequence();
                sequence.AppendCallback(() =>  arrow.transform.DOMove(pos+Vector3.down * valueSize, 0.5f).SetLoops(loopTIme, LoopType.Yoyo));
                sequence.InsertCallback(killTime, () => UnArrow());
                break;
            case DirEight.R:
                arrow.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -0), 0);
                sequence = DOTween.Sequence();
                sequence.AppendCallback(() => arrow.transform.DOMove(pos+ Vector3.right * valueSize, 0.5f).SetLoops(loopTIme, LoopType.Yoyo));
                sequence.InsertCallback(killTime, () => UnArrow());
                break;
            default:
				break;
		}
    }

    public void UnArrow() {
        sequence.Kill();
        //DOTween.Kill(guid);
        arrow.SetActive(false);
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

    public void OnclickPopupCheck4()
    {
        Debug.Log($"4");
        //im.activeInput = true;
        popUp[4].SetActive(true);
        //gm.tutorialMode = false;
        checkonce = false;
    }
    public void OnclickPopupCheck5()
    {
        Debug.Log($"5");
        //im.activeInput = false;
        popUp[4].SetActive(false);

    }
    public bool t = true;
    //암살
    public void OnclickPopupCheck6()
    {
        Debug.Log($"6");
        popUp[5].SetActive(true);
        t = false;

    }

    public void OnclickPopupCheck7()
    {
        Debug.Log($"7");
        popUp[5].SetActive(false);
    }

    //시야
    public bool tt = true;
    public void OnclickPopupCheck8()
    {
        Debug.Log($"8");
        popUp[6].SetActive(true);
        tt = false;
    }

    public void OnclickPopupCheck9()
    {
        Debug.Log($"9");
        im.activeInput = false;
        popUp[6].SetActive(false);
        popUp[7].SetActive(true);
        
    }

    public void OnClickPopUp12()
    {
        im.activeInput = false;
        popUp[7].SetActive(false);
        popUp[8].SetActive(true);
    }
    public void OnClickPopUp13()
    {
        im.activeInput = false;
        StartCoroutine(CoClickPopUp13());
    }
    public void OnClickPopUp14()
    {
        im.activeInput = false;
        popUp[9].SetActive(false);
        popUp[10].SetActive(true);
    }
    public void OnClickPopUp15()
    {
        im.activeInput = false;
        popUp[10].SetActive(false);
        popUp[11].SetActive(true);
    }
    public void OnClickPopUp15_2()
    {
        im.activeInput = false;
        popUp[11].SetActive(false);
        popUp[12].SetActive(true);
    }
    public void OnClickPopUp16()
    {
        im.activeInput = false;
        popUp[12].SetActive(false);
        popUp[13].SetActive(true);
    }
    public void OnClickPopUp17()
    {
        im.activeInput = false;
        StartCoroutine(CoClickPopUp17());
        //popupUI[6].SetActive(false);
        //popupUI[7].SetActive(true);
    }
    public void OnClickPopUp18()
    {
      
        popUp[14].SetActive(false);
        popUp[15].SetActive(true);
        popUp[15].SetActive(false);
        im.activeInput = true;
    }
   
    public IEnumerator CoClickPopUp13()
    {
       
        popUp[8].SetActive(false);
        tutorialObject[0].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        tutorialObject[0].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        tutorialObject[0].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        tutorialObject[0].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        tutorialObject[0].SetActive(true);
        yield return new WaitForSeconds(2f);
        tutorialObject[0].SetActive(false);
        popUp[9].SetActive(true);

    }
    public IEnumerator CoClickPopUp17()
    {
       
        UIManager.Instance.positionDetail.SetActive(true);
        popUp[13].SetActive(false);
        tutorialObject[1].SetActive(true);
        tutorialObject[2].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        tutorialObject[1].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        tutorialObject[1].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        tutorialObject[1].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        tutorialObject[1].SetActive(true);
        yield return new WaitForSeconds(3f);
        tutorialObject[1].SetActive(false);
        tutorialObject[2].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        tutorialObject[3].SetActive(true);
        tutorialObject[4].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        tutorialObject[3].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        tutorialObject[3].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        tutorialObject[3].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        tutorialObject[3].SetActive(true);
        yield return new WaitForSeconds(3f);
        tutorialObject[3].SetActive(false);
        tutorialObject[4].SetActive(false);
        popUp[14].SetActive(true);
        UIManager.Instance.positionDetail.SetActive(false);
        
    }
}
