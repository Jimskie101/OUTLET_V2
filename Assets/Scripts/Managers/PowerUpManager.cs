using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



public class PowerUpManager : MonoBehaviour
{
    [System.Serializable]
    public class PowerUpBar
    {
        public GameObject powerUpParent;
        public Image powerUpLeft;
        public Image powerUpRight;
    }

    //PowerUp
    [Header("Power Up")]
    [SerializeField] PowerUpBar[] m_powerUpBar;
    Image powerUpLeft;
    Image powerUpRight;
    float m_powerUpDuration;


    public void PowerUpCountdown(float duration, PowerUp powerUpScript, GameObject powerUpFX = null)
    {
        foreach (PowerUpBar p in m_powerUpBar)
        {
            if (!p.powerUpParent.activeSelf)
            {
                if (powerUpFX != null)
                    powerUpFX.SetActive(true);
                p.powerUpParent.SetActive(true);
                float endValue = 0;
                float startValue = 1;
                float interval;

                // Move the value from startValue to endValue over a duration
                DOTween.To(() => startValue, x => startValue = x, endValue, duration)
                    .OnUpdate(() =>
                    {
                        // Update any UI elements or variables that need to reflect the current value
                        // For example, if you are moving a UI slider:
                        // mySlider.value = startValue;
                        p.powerUpLeft.fillAmount = startValue;
                        p.powerUpRight.fillAmount = startValue;
                        if (startValue < 0.5f && startValue > 0.001f)
                        {
                            interval = Mathf.Clamp(startValue, 0.2f, 1);
                            powerUpScript.BlinkOut(interval);

                        }
                        
                    })
                    .OnComplete(() =>
                    {

                        p.powerUpParent.SetActive(false);
                        powerUpScript.ResetValues();
                    });
                break;
            }
        }



    }



}
