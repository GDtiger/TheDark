using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUISystem : MonoBehaviour
{
	public GameObject optionPanel;
	public SoundManager soundManager;
	void Start()
	{
		soundManager = SoundManager.Instance;
		//soundManager.PlayBGMSound("MainScene", 1);
	}

	public void OnClickStart()
	{
		SoundManager.Instance.PlaySFXSound("Confirm");
		gameObject.SetActive(false);
		LoadingSceneController.Instance.LoadScene("Town Tutorial");
	}
	public void OnClickExit()
	{
		SoundManager.Instance.PlaySFXSound("Confirm");
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		System.Diagnostics.Process.GetCurrentProcess().Kill();
		Application.Quit();

#endif
		//UnityEditor.EditorApplication.isPlaying = false;
	}
}
