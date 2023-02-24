using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;
using TMPro;


public class UIManager : MonoBehaviour
{
    //Player Movement
    [SerializeField] PlayerMovement m_playerMoveScript;
    //Game Status
    SceneHandler m_sceneHandler;
    CheckpointManager m_checkpointManager;




    private void Start()
    {
        m_sceneHandler = Managers.Instance.SceneHandler;
        m_checkpointManager = Managers.Instance.CheckpointManager;
        m_timeForScreen = new WaitForSeconds(2f);
        FadeInFromBlack();
        GetResolutionData();
        if (m_sceneHandler.GetCurrentScene() == 1)
        {
            Debug.Log("Checking Saves");
            m_load.interactable = Managers.Instance.SaveAndLoadManager.CheckJson();
        }


    }

    public void UpdateCollectibleCount(int amount)
    {

    }

    [Header("Info/Hint UI")]
    [SerializeField] TMP_Text m_titleText;
    [SerializeField] TMP_Text m_infoText;
    [SerializeField] GameObject m_uiObjectWithHint;
    public void ShowInfo(string title = "", string info = "", bool isAControl = false, GameObject uiObj = null)
    {
        Time.timeScale = 0f;
        if (isAControl)
        {
            if (!uiObj.activeSelf)
            {
                Managers.Instance.InputHandler.ShowingHint();
                uiObj.transform.parent.gameObject.SetActive(true);
                uiObj.SetActive(true);
                m_uiObjectWithHint = uiObj;
            }
        }
        else
        {
            if (!m_titleText.transform.parent.gameObject.activeSelf)
            {
                Managers.Instance.InputHandler.ShowingHint();
                m_titleText.text = title;
                m_infoText.text = info;
                m_titleText.transform.parent.gameObject.SetActive(true);

            }
        }

    }
    public void HideInfo()
    {
        if (m_titleText.transform.parent.gameObject.activeSelf)
        {
            Managers.Instance.InputHandler.UnShowingHint();
            m_titleText.transform.parent.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

        else if (m_uiObjectWithHint.activeSelf)
        {
            Managers.Instance.InputHandler.UnShowingHint();
            m_uiObjectWithHint.transform.parent.gameObject.SetActive(false);
            m_uiObjectWithHint.SetActive(false);
            Time.timeScale = 1f;
            m_uiObjectWithHint = null;
        }

    }


    [SerializeField] TMP_Text m_gameStatusText;
    RectTransform m_gameStatusRect;
    public void ShowGameUpdate(string statusText)
    {
        m_gameStatusText.gameObject.SetActive(true);
        m_gameStatusText.text = statusText;
        m_gameStatusRect = m_gameStatusText.GetComponent<RectTransform>();

        // Fade out the text over the specified duration
        Sequence sequence = DOTween.Sequence()
            .Append(m_gameStatusText.DOFade(0f, 1f))
            .Append(m_gameStatusText.DOFade(1f, 1f))
            .Append(m_gameStatusText.DOFade(0f, 1f))
            .Append(m_gameStatusText.DOFade(1f, 1f))
            .Append(m_gameStatusText.DOFade(0f, 1f))
            .SetUpdate(true)
            .OnComplete(() => m_gameStatusText.gameObject.SetActive(false));

        // Start the sequence
        sequence.Play();


    }


    //PowerUp
    [Header("Power Up")]
    [SerializeField] Image powerUpLeft;
    [SerializeField] Image powerUpRight;
    float m_powerUpDuration;

    public void PowerUpCountdown(float duration, PowerUp powerUpScript)
    {
         powerUpLeft.transform.parent.gameObject.SetActive(true);
        float endValue = 0;
        float startValue = 1;
        // Move the value from startValue to endValue over a duration
        DOTween.To(() => startValue, x => startValue = x, endValue, duration)
            .OnUpdate(() =>
            {
                // Update any UI elements or variables that need to reflect the current value
                // For example, if you are moving a UI slider:
                // mySlider.value = startValue;
                powerUpLeft.fillAmount = startValue;
                powerUpRight.fillAmount = startValue;
                
            })
            .OnComplete(() =>
            {
                powerUpLeft.transform.parent.gameObject.SetActive(false);
                powerUpScript.ResetValues();
            });
    }



    //Main Menu

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }
    public void StartGame()
    {
        Managers.Instance.SceneHandler.LoadStage(2);
    }
    public void RestartLevel()
    {
        Managers.Instance.SaveAndLoadManager.ClearJson();
        m_sceneHandler.LoadStage(m_sceneHandler.GetCurrentScene());

    }
    public void MainMenu()
    {
        Managers.Instance.SceneHandler.LoadStage(1);
    }
    public void ReturnToCheckpoint()
    {
        Managers.Instance.GameData.LoadingFromSave = true;
        m_sceneHandler.LoadStage(m_sceneHandler.GetCurrentScene());
    }
    public void LoadGame()
    {
        Managers.Instance.GameData.LoadingFromSave = true;
        m_sceneHandler.LoadStage(Managers.Instance.SaveAndLoadManager.CheckGameData.stageData.SceneNumber);
    }

    //Button Functions
    [Header("Menu Screens")]
    [SerializeField] Button m_load;
    [SerializeField] GameObject m_levelsScreen;
    [SerializeField] GameObject m_settingsScreen;
    [SerializeField] GameObject m_creditsScreen;
    [SerializeField] GameObject m_pauseScreen;
    GameObject m_screenPlaceholder;
    public void OpenLevelScreen()
    {
        m_screenPlaceholder = m_levelsScreen;
        m_levelsScreen.SetActive(true);
    }
    public void OpenSettingsScreen()
    {
        m_screenPlaceholder = m_settingsScreen;
        m_settingsScreen.SetActive(true);
    }
    public void OpenCreditsScreen()
    {
        m_screenPlaceholder = m_creditsScreen;
        m_creditsScreen.SetActive(true);
    }
    public void BacktoMain()
    {
        m_screenPlaceholder.SetActive(false);
        m_screenPlaceholder = null;
    }
    [SerializeField] GameObject m_deathScreen;
    [SerializeField] GameObject m_winScreen;

    WaitForSeconds m_timeForScreen;
    public IEnumerator DeathScreen()
    {
        yield return m_timeForScreen;
        m_deathScreen.SetActive(true);
    }
    public IEnumerator WinScreen()
    {
        yield return m_timeForScreen;
        m_winScreen.SetActive(true);
    }



    //Settings
    [Header("Settings")]
    [SerializeField] Dropdown m_resolutionDropdown;
    Resolution[] m_resolutions;
    [SerializeField] AudioMixer m_audioMixer;



    private void GetResolutionData()
    {
        m_resolutions = Screen.resolutions;
        m_resolutionDropdown.ClearOptions();

        List<string> resolutionList = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < m_resolutions.Length; i++)
        {
            string resolution = m_resolutions[i].width + "x" + m_resolutions[i].height;
            resolutionList.Add(resolution);
            if (m_resolutions[i].width == Screen.currentResolution.width &&
            m_resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        m_resolutionDropdown.AddOptions(resolutionList);
        m_resolutionDropdown.value = currentResolutionIndex;
        m_resolutionDropdown.RefreshShownValue();

    }
    public void SetResolution(int i_resolutionIndex)
    {
        Resolution resolution = m_resolutions[i_resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }



    public void SetVolume(float f_volume)
    {
        m_audioMixer.SetFloat("Volume", f_volume);
    }

    public void SetQuality(int i_qualityIndex)
    {
        QualitySettings.SetQualityLevel(i_qualityIndex);
    }

    public void SetFullscreen(bool b_isFullscreen)
    {
        Screen.fullScreen = b_isFullscreen;
    }

    //Pause
    public void PauseGame()
    {
        if (!m_pauseScreen.activeSelf)
        {
            Managers.Instance.CameraHandler.enabled = false;
            Time.timeScale = 0f;
            Managers.Instance.InputHandler.Pause();
            m_pauseScreen.SetActive(true);

        }
        else if (m_pauseScreen.activeSelf)
        {
            Managers.Instance.CameraHandler.enabled = true;
            Time.timeScale = 1f;
            Managers.Instance.InputHandler.UnPause();
            m_pauseScreen.SetActive(false);

        }
    }

    [Header("Hp Bar")]
    [SerializeField] Image m_hpBar1;
    [SerializeField] Image m_hpBar2;
    //Update HP Bar in UI
    public void UpdateHPBar(float amount)
    {
        m_hpBar1.fillAmount = amount;
        m_hpBar2.fillAmount = amount;
    }

    [Header("Fader")]
    [SerializeField] Image m_fadeImage;
    [SerializeField] float m_fadeTime = 2.0f;
    Color32 m_endColor;
    //Fade IN
    public void FadeInFromBlack()
    {
        m_endColor = new Color32(0, 0, 0, 0);
        m_fadeImage.gameObject.SetActive(true);
        m_fadeImage.DOColor(m_endColor, m_fadeTime).SetUpdate(true).
        OnComplete(() => m_fadeImage.gameObject.SetActive(false));
    }
    public void FadeToBlack(bool stopTime = false)
    {
        if (stopTime) Managers.Instance.InputHandler.EndStage();
        m_endColor = new Color32(0, 0, 0, 255);
        m_fadeImage.gameObject.SetActive(true);
        m_fadeImage.DOColor(m_endColor, m_fadeTime).SetUpdate(true);
    }




}
