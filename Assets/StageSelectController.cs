using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Town
{
	public class StageSelectController : MonoBehaviour
	{
		//public GameObject stageSelcetUIPannel;
		public SceneID sceneID = SceneID.Stage1;

		public void OpenPannel(SceneID _sceneID)
		{
			gameObject.SetActive(true);
			sceneID = _sceneID;
		}
		public void SelectStage()
		{
			//GameManager.Instance.uiLoadScreen.FadeWindow(true);
			//SceneManager.LoadScene((int)sceneID, LoadSceneMode.Single);
			Town.Town_UIManager.Instance.CloseAllUI();
			if(sceneID == SceneID.Stage1)
			LoadingSceneController.Instance.LoadScene("The Dark Map");
			if (sceneID == SceneID.Stage2)
				LoadingSceneController.Instance.LoadScene("The Dark Map");


		}

		public void CloseWindow()
		{
			gameObject.SetActive(false);
			Town_UIManager.Instance.CloseWindow();

		}

	}
}

