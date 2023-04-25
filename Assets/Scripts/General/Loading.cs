using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Loading : MonoBehaviour
{
    UIManager m_uiManager;
    
    [SerializeField] private float minimumLoadTime = 3.0f; // The minimum amount of time to show the loading screen in seconds.

    private void Start()
    {   Managers.Instance.AudioManager.PlayHere("loading", this.gameObject, false, true);
        m_uiManager = Managers.Instance.UIManager;
        
        // Load the target scene asynchronously
        if(Managers.Instance.SceneHandler.GetCurrentSceneName() == "Loading")
            StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        

        // Load the target scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(Managers.Instance.GameData.NextSceneIndex, LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = false;

        float startTime = Time.time;

        // Update the progress bar while loading
        while (Time.time - startTime < minimumLoadTime)
        {
            float elapsedTime = Time.time - startTime;
            float progress = Mathf.Clamp01(elapsedTime / minimumLoadTime);
            m_uiManager.LoadingBar.fillAmount = progress;

            yield return null;
        }

        // Wait a short time to ensure the progress bar shows 100%
        yield return new WaitForSeconds(0.5f);

        // Activate the target scene
        asyncOperation.allowSceneActivation = true;
        
        // Unload the LoadingScreen scene
        SceneManager.UnloadSceneAsync("Loading");
    }
}
