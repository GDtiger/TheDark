using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTutorialManager : MonoBehaviour
{

    public GameManager gm;
    public GameObject popUp;
    public Image image;
    public TMPro.TMP_Text titleText;
    public TMPro.TMP_Text descriptText;
    public int tutorialIndex;
    public Sprite[] images;

    #region instance
    static GameTutorialManager instance = null;

    public static GameTutorialManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameTutorialManager>();
            }
            return instance;
        }
    }
    #endregion


    public void Initiate() {
        gm = GameManager.Instance;
        popUp.SetActive(false);
        if (!gm.tutorialInagameMode)
        {
            CallTutorial();
        }
    }

    public void CallTutorial() {
        gm.isPaused = true;
        titleText.text = gm.textDataBaseBattleTitle.GetTextData(tutorialIndex.ToString()).text;
        descriptText.text = gm.textDataBaseBattleDesc.GetTextData(tutorialIndex.ToString()).text;
		if (images[tutorialIndex] == null)
		{
            image.enabled = false;

		}
		else
		{
            image.enabled = true;
            image.sprite = images[tutorialIndex];
        }
      
        popUp.SetActive(true);
        tutorialIndex++;
    }

    public void ClosePopUp() {
        Debug.Log("Ã¢´Ý±â");
        popUp.SetActive(false);
        gm.isPaused = false;
        if (tutorialIndex < gm.textDataBaseBattleTitle.GetDataSize())
        {
            CallTutorial();
        }
        else
        {
            gm.tutorialInagameMode = false;
        }
    }

    
}
