using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
    }


    [Button]
    public void LoadStage(int stage)
    {
        DOTween.KillAll();
        SceneManager.LoadScene(stage);
    }
    public int GetCurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }



    //Splash Intro Scene
    private void Start()
    {   
        m_sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (m_sceneIndex == 0)
            IntroScene();
        if (m_sceneIndex == 2)
            Cinematics1();
        
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

    public void Cinematics1()
    {
        StartCoroutine(DelayLoadLevel(90));
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
    bool m_working = false;
    private void Update()
    {
        if(m_sceneIndex == 2 && !m_working)
        {
            if(Input.GetButtonDown("Jump"))
            {
                m_working = true;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }




}