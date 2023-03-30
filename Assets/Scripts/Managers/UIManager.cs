using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Audio;
using DG.Tweening;
using TMPro;
using System.Linq;
using EasyButtons;

public class UIManager : MonoBehaviour
{
    [Header("Publicized Objects")]
    public Image WaypointIcon;

    [Space]
    //Player Movement
    [SerializeField] PlayerMovement m_playerMoveScript;
    //Game Status
    SceneHandler m_sceneHandler;
    CheckpointManager m_checkpointManager;

    public Image LoadingBar;

    
    private void Start()
    {   


        if(Managers.Instance.GameManager != null)
        Managers.Instance.GameManager.Player.TryGetComponent(out m_playerMoveScript);
        
        m_sceneHandler = Managers.Instance.SceneHandler;
        m_checkpointManager = Managers.Instance.CheckpointManager;
        m_timeForScreen = new WaitForSeconds(2f);
        FadeInFromBlack();
        if (m_resolutionDropdown != null)
            GetResolutionData();
        if (m_sceneHandler.GetCurrentScene() == 1)
        {
            Debug.Log("Checking Saves");
            m_load.interactable = Managers.Instance.SaveAndLoadManager.CheckJson();
        }


    }

    public void LoadStage(int index)
    {
        Managers.Instance.SceneHandler.LoadStage(index);
    }






    [Header("Damage Flash")]
    [SerializeField] Image m_damageFlash;
    Sequence m_damageFlashSequence;
    [Button]
    public void DamageFX()
    {  
        
         m_damageFlash.gameObject.SetActive(true);
         m_damageFlashSequence = DOTween.Sequence()
            .Append(m_damageFlash.DOFade(1f, 0.1f))
            .Append(m_damageFlash.DOFade(0f, 0.2f))
            .Append(m_damageFlash.DOFade(1f, 0.15f))
            .Append(m_damageFlash.DOFade(0f, 0.3f)).SetEase(Ease.InOutFlash)
            .OnComplete(() => m_damageFlash.gameObject.SetActive(false));
    }

    [SerializeField] TMP_Text m_collectibleText;
    public void UpdateCollectibleCount(int amount)
    {

        m_collectibleText.text = amount < 10 ? "Postcards Collected:  " + amount + "/10" : "Postcards Collected: " + amount + "/10";

    }

    [Header("Info/Hint UI")]
    [SerializeField] Image[] m_cinematicsSkipperCircles;
    [SerializeField] UnityEvent m_skipperEvents;

    //Cinematic Skipper
    bool m_holdingSkip = false;
    float m_holdingSkipTime = 3f;
    float m_holdingTimer = 0;
    bool m_skipped = false;
    public void SkipperHold()
    {
        m_holdingSkip = true;
    }
    public void ResetSkipperHold()
    {
        m_holdingSkip = false;

    }

    private void UpdateCircle()
    {
        if (m_holdingTimer >= m_holdingSkipTime && !m_skipped)
        {
            m_skipped = true;
            foreach (Image i in m_cinematicsSkipperCircles)
            {
                if (i.gameObject.activeSelf)
                {
                    
                    i.fillAmount = 0;

                }
            }
            m_skipperEvents.Invoke();
        }
        if (m_holdingTimer <= m_holdingSkipTime)
        {
            m_holdingTimer += Time.unscaledDeltaTime;
            foreach (Image i in m_cinematicsSkipperCircles)
            {
                if (i.gameObject.activeSelf)
                {
                    i.fillAmount = Mathf.Clamp(m_holdingTimer / m_holdingSkipTime, 0, 1f);
                }

            }
        }


    }
    private void Update()
    {
        if (m_holdingSkip)
            UpdateCircle();
        else if (!m_holdingSkip && m_holdingTimer > 0 && !m_skipped)
        {
            m_holdingTimer -= Time.unscaledDeltaTime * 2;
            foreach (Image i in m_cinematicsSkipperCircles)
            {
                if (i.gameObject.activeSelf)
                {
                    i.fillAmount = Mathf.Clamp(m_holdingTimer / m_holdingSkipTime, 0, 1f); ;
                }

            }
        }
    }





    [Header("Info/Hint UI")]
    [SerializeField] TMP_Text m_titleText;
    [SerializeField] TMP_Text m_infoText;
    [SerializeField] GameObject m_uiObjectWithHint;
    public void ShowInfo(string title = "", string info = "", bool isAControl = false, GameObject uiObj = null)
    {
        Time.timeScale = 0f;
        m_skipped = false;
        m_holdingTimer = 0;
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
        if (m_triviaObject != null)
            HideTrivia();
        if (m_posterObject != null)
            HidePoster();
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

    [Header("Trivia/Collectible UI")]
    [SerializeField] Image m_triviaImage;
    [SerializeField] GameObject m_triviaObject;

    public void ShowTrivia(Sprite image)
    {
        if (!m_triviaObject.activeSelf)
        {
            Time.timeScale = 0f;
            m_skipped = false;
            m_holdingTimer = 0;
            Managers.Instance.InputHandler.ShowingHint();
            m_triviaImage.sprite = image;
            m_triviaObject.SetActive(true);
        }


    }
    public void HideTrivia()
    {
        if (m_triviaObject.activeSelf)
        {
            Time.timeScale = 1f;
            Managers.Instance.InputHandler.UnShowingHint();
            m_triviaObject.SetActive(false);
        }

    }


    [Header("Posters UI")]
    [SerializeField] Image m_posterImage;
    [SerializeField] GameObject m_posterObject;

    public void ShowPoster(Sprite image)
    {
        if (!m_posterObject.activeSelf)
        {
            Time.timeScale = 0f;
            m_skipped = false;
            m_holdingTimer = 0;
            Managers.Instance.InputHandler.ShowingHint();
            m_posterImage.sprite = image;
            m_posterObject.SetActive(true);
        }


    }
    public void HidePoster()
    {
        if (m_posterObject.activeSelf)
        {
            Time.timeScale = 1f;
            Managers.Instance.InputHandler.UnShowingHint();
            m_posterObject.SetActive(false);
        }

    }





    [SerializeField] TMP_Text m_gameStatusText;
    RectTransform m_gameStatusRect;
    public void ShowGameUpdate(string statusText)
    {
        m_gameStatusText.gameObject.SetActive(true);
        m_gameStatusText.text = statusText;
        m_gameStatusRect = m_gameStatusText.rectTransform;

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

    [System.Serializable]
    public class PowerUpBar
    {
        public GameObject powerUpParent;
        public Image powerUpLeft;
        public Image powerUpRight;
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
    Resolution[] m_filteredResolutions;
    [SerializeField] AudioMixer m_audioMixer;



    private void GetResolutionData()
    {
        m_resolutions = Screen.resolutions;
        m_filteredResolutions = m_resolutions.Where(resolution =>
        {
            float aspectRatio = (float)resolution.width / (float)resolution.height;
            return Mathf.Approximately(aspectRatio, 16f / 9f);
        }).ToArray();
        m_resolutionDropdown.ClearOptions();

        List<string> resolutionList = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < m_filteredResolutions.Length; i++)
        {
            string resolution = m_filteredResolutions[i].width + "x" + m_filteredResolutions[i].height;
            resolutionList.Add(resolution);
            if (m_filteredResolutions[i].width == Screen.currentResolution.width &&
            m_filteredResolutions[i].height == Screen.currentResolution.height)
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
        Resolution resolution = m_filteredResolutions[i_resolutionIndex];
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
    Tween m_faderTween = null;
    //Fade IN
    public void FadeInFromBlack()
    {
        if (m_fadeImage != null)
        {
            m_endColor = new Color32(0, 0, 0, 0);

            if (m_faderTween != null) m_faderTween.Kill();

            m_fadeImage.gameObject.SetActive(true);
            m_faderTween = m_fadeImage.DOColor(m_endColor, m_fadeTime).SetUpdate(true).
            OnComplete(() => m_fadeImage.gameObject.SetActive(false));
        }

    }
    public void FadeToBlack(bool stopTime = false, System.Action callback = null)
    {
        if (m_fadeImage != null)
        {
            if (stopTime) Managers.Instance.InputHandler.EndStage();
            m_endColor = new Color32(0, 0, 0, 255);

            if (m_faderTween != null) m_faderTween.Kill();

            m_fadeImage.gameObject.SetActive(true);

            m_faderTween = m_fadeImage.DOColor(m_endColor, m_fadeTime).SetUpdate(true)
            .OnComplete(() => { if (callback != null) callback(); });
        }

    }




}
