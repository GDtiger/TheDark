using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class OptionController : MonoBehaviour
{
    public GameObject panel;
    public GameObject helpBtn;
    public GameObject returnToTownBtn;
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Awake()
    {
        mixer = SoundManager.Instance.mixer;
    }

    public void OnClickOpenOption()
    {
        Debug.Log("Open to Option");
        SoundManager.Instance.PlaySFXSound("Confirm");
        panel.SetActive(true);
        if (SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene().name == "Town Tutorial")
		{
            Debug.Log("Town");
            helpBtn.SetActive(true);
            returnToTownBtn.SetActive(false);
        }
        else
		{
            Debug.Log("Ingame");
            helpBtn.SetActive(false);
            returnToTownBtn.SetActive(true);
        }
    }
    public void OnClickCloseOption()
    {
        SoundManager.Instance.PlaySFXSound("Cancle");
        panel.SetActive(false);
    }
    public void OnClickReturnToTown()
	{
        panel.SetActive(false);
        LoadingSceneController.Instance.LoadScene("Town Tutorial");
    }
    public void BGSoundVolume(float val)
    {
        mixer.SetFloat("BGM", Mathf.Log10(val) * 20);
    }
    public void SFXSoundVolume(float val)
    {
        mixer.SetFloat("SFX", Mathf.Log10(val) * 20);
    }
    public void MasterSoundVolume(float val)
    {
        mixer.SetFloat("Master", Mathf.Log10(val) * 20);
    }
}
