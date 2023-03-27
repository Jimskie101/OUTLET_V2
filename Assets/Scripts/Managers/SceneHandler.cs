using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using EasyButtons;
using DG.Tweening;

public class SceneHandler : MonoBehaviour
{

    int m_sceneIndex = 0;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    VideoPlayer m_videoPlayer;

    //Splash Intro Scene
    private void Start()
    {
        m_sceneIndex = GetCurrentScene();
        m_videoPlayer = FindObjectOfType<VideoPlayer>();

        if (m_videoPlayer != null && GetCurrentScene() != 0)
            m_videoPlayer.loopPointReached += EndReached;
        if (m_sceneIndex == 0) IntroScene();

        if (GetCurrentSceneName() != "Loading" && SceneManager.GetSceneByName("Loading").isLoaded)
            SceneManager.UnloadSceneAsync("Loading");

    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
    }


    [Button]
    public void LoadStage(int stage)
    {
        DOTween.KillAll();

        Managers.Instance.GameData.NextSceneIndex = stage;
        Managers.Instance.UIManager.FadeToBlack(false, () => StartCoroutine(LoadSceneAsync()));

    }
    UIManager m_uiManager;
    private IEnumerator LoadSceneAsync()
    {
        // Load the LoadingScreen scene
        yield return SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Single);

        // // Load the target scene asynchronously
        // AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(Managers.Instance.GameData.NextSceneIndex, LoadSceneMode.Additive);
        // asyncOperation.allowSceneActivation = false;

        // // Update the progress bar while loading
        // while (!asyncOperation.isDone)
        // {
        //     m_uiManager.LoadingBar.fillAmount  = Mathf.Clamp01(asyncOperation.progress / 0.9f);;

        //     // Wait until the target scene is fully loaded
        //     if (asyncOperation.progress >= 0.9f)
        //     {
        //         // Wait a short time to ensure the progress bar shows 100%
        //         yield return new WaitForSeconds(1f);

        //         // Activate the target scene
        //         asyncOperation.allowSceneActivation = true;
        //     }

        //     yield return null;
        // }

        // // Unload the LoadingScreen scene
        // SceneManager.UnloadSceneAsync("Loading");
    }



    public int GetCurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    public Scene GetSceneValueByName(string sceneName)
    {
        return SceneManager.GetSceneByName(sceneName);
    }




    float secondsLeft = 0;
    WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    public void IntroScene()
    {
        StartCoroutine(DelayLoadLevel(10));
        IEnumerator DelayLoadLevel(float seconds)
        {
            secondsLeft = seconds;
            do
            {
                yield return waitForSeconds;
            } while (--secondsLeft > 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        LoadStage(GetCurrentScene() + 1);

    }


    // public void Cinematics1()
    // {
    //     StartCoroutine(DelayLoadLevel(90));
    //     IEnumerator DelayLoadLevel(float seconds)
    //     {
    //         secondsLeft = seconds;
    //         do
    //         {
    //             yield return waitForSeconds;
    //         } while (--secondsLeft > 0);
    //         LoadStage(SceneManager.GetActiveScene().buildIndex + 1);
    //     }
    // }
    bool m_working = false;
    public void Skip()
    {
        if (m_videoPlayer != null && m_videoPlayer.isPlaying && !m_working)
        {
            m_working = true;
            EndReached(m_videoPlayer);
        }
    }




}