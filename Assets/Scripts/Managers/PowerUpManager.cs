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

    float m_powerUpDuration;


    public void PowerUpCountdown(float duration, PowerUp powerUpScript, GameObject powerUpFX = null)
    {

        if (powerUpFX != null)
            powerUpFX.SetActive(true);
        float endValue = 0;
        float startValue = 1;
        float remainingTime = duration;
        float lastRemainingTime = 0;
        float minTime = duration * 0.0001f;

        // Move the value from startValue to endValue over a duration
        DOTween.To(() => startValue, x => startValue = x, endValue, duration)
            .OnUpdate(() =>
            {
                if (startValue < 0.25f)
                {
                    if (duration * startValue <= remainingTime / 2)
                    {
                        if (lastRemainingTime != remainingTime)
                        {
                            Debug.Log("blinked");
                            lastRemainingTime = remainingTime;
                            remainingTime = Mathf.Clamp(remainingTime / 2, minTime, Mathf.Infinity);
                            powerUpScript.BlinkOut();
                        }

                    }

                }







            })
            .OnComplete(() =>
            {
                if (powerUpScript.m_blinkBack != null)
                    StopCoroutine(powerUpScript.m_blinkBack);
                powerUpScript.ResetValues();
            });




    }



}
