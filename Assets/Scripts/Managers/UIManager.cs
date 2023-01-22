using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class UIManager : MonoBehaviour
{
    //Game Status

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

    //Button Functions
    [Header("Menu Screens")]
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

    //Settings
    [Header("Settings")]
    [SerializeField] Dropdown m_resolutionDropdown;
    Resolution[] m_resolutions;
    [SerializeField] AudioMixer m_audioMixer;

    public void SetResolution(int i_resolutionIndex)
    {
        Resolution resolution = m_resolutions[i_resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void Start()
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
        if(!m_pauseScreen.activeSelf)
        {
            Time.timeScale = 0f;
            m_pauseScreen.SetActive(true);

        }
        else if(m_pauseScreen.activeSelf)
        {
            Time.timeScale = 1f;
            m_pauseScreen.SetActive(false);
            
        }
    }
}
