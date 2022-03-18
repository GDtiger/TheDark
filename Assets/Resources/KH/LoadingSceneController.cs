using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    private static LoadingSceneController instance;

    public static LoadingSceneController Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<LoadingSceneController>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();

                }
            }
            return instance;
        }
    }

    private static LoadingSceneController Create()
    {
        Debug.Log("프리펩 불러오기");
        return Instantiate(Resources.Load<LoadingSceneController>("LoadingUITest"));
    }

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField]
    private CanvasGroup canvasGroup;
    private string loadSceneName;
    [SerializeField]
    private Image progressBar;
    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnscenLoaded;
        loadSceneName = sceneName;
        StartCoroutine(LoadSceneProcess());
    }
    IEnumerator LoadSceneProcess()
    {
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;
        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
    IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
        {
            gameObject.SetActive(false);
        }

    }
    private void OnscenLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Debug.Log("OnscenLoaded Start");
        if (!GameManager.Instance.debugMode)
        {
            if (arg0.name == "Stage1" || arg0.name == "Stage1 1")
            {
                Debug.Log("Stage");

                StartCoroutine(Fade(false));

                GameManager.Instance.InitiateIngame();
                SceneManager.sceneLoaded -= OnscenLoaded;
            }
            else if (arg0.name == "Town"||arg0.name == "Town Tutorial")
            {
                Debug.Log("Town Tutorial");
                StartCoroutine(Fade(false));
                GameManager.Instance.InitiateInTown();
                SceneManager.sceneLoaded -= OnscenLoaded;
            }
            else if (arg0.name == "The Dark Map")
            {
                Debug.Log("The Dark Map");
                StartCoroutine(Fade(false));
                GameManager.Instance.InitiateIngame();
                SceneManager.sceneLoaded -= OnscenLoaded;
            }

        }
        Debug.Log("OnscenLoaded End");
    }
}

