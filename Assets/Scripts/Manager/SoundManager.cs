using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class SoundManager : MonoBehaviour
{
    #region instance
    private static SoundManager instace;
    public static SoundManager Instance
    {
        get
        {
            if (instace == null)
            {
                instace = FindObjectOfType<SoundManager>();
            }
            return instace;
        }
    }
    #endregion

    public AudioMixer mixer;

    [SerializeField] private AudioSource bgmPlayer;
    [SerializeField] private AudioSource sfxPlayer;

    public float masterVolBGM = 1f;
    public float masterVolSFX = 1f;


    [SerializeField]
    private AudioClip mainBGMClip;
    [SerializeField]
    private AudioClip fieldBGMClip;
    [SerializeField]
    private AudioClip bossBGMClip;

    [SerializeField]
    private SoundData[] sfxAudioClips;
    [SerializeField]
    private Dictionary<string, AudioClip> sfxClipsDic = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        //DontDestroyOnLoad(this.gameObject);
        bgmPlayer = GameObject.Find("BGMSoundPlayer").GetComponent<AudioSource>();
        sfxPlayer = GameObject.Find("SFXSoundPlayer").GetComponent<AudioSource>();

        foreach (var audioClip in sfxAudioClips)
        {
            sfxClipsDic.Add(audioClip.id, audioClip.audio);
        }
    }

    public void PlaySFXSound(string name, float volume = 1f)
    {
        if (!sfxClipsDic.ContainsKey(name))
        {
            Debug.Log(name + "는 없는 사운드 입니다 ");
            return;
        }
       // Debug.Log($"a");
        sfxPlayer.PlayOneShot(sfxClipsDic[name], volume * masterVolSFX);
        sfxPlayer.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
    }

    public void PlayBGMSound(string name, float volum = 1f)
    {
        //Debug.Log("PlayBGMSound");
        bgmPlayer.loop = true;
        bgmPlayer.volume = volum * masterVolBGM;
        bgmPlayer.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];

        if (bgmPlayer.isPlaying)
        {
            bgmPlayer.Stop();
        }

        if (name == "MainMenu")
        {
            Debug.Log("PlayBGMSound");
            bgmPlayer.clip = mainBGMClip;
            bgmPlayer.Play();
        }
        if (name == "Town Tutorial")
        {
            Debug.Log("PlayBGMSound");
            bgmPlayer.clip = fieldBGMClip;
            bgmPlayer.Play();
        }
        if (name == "Stage1")
        {
            Debug.Log("PlayBGMSound");
            bgmPlayer.clip = bossBGMClip;
            bgmPlayer.Play();
        }
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

[System.Serializable]
public class SoundData
{
    public string id;
    public AudioClip audio;
}