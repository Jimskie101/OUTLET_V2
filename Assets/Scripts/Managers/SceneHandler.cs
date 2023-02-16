using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyButtons;

public class SceneHandler : MonoBehaviour
{
    [Button]
    public void LoadStage(int stage)
    {
        SceneManager.LoadScene(stage);
    }
    public int GetCurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    

    //Splash Intro Scene
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            IntroScene();
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
}