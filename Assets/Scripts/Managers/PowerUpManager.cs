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
                float interval;

                // Move the value from startValue to endValue over a duration
                DOTween.To(() => startValue, x => startValue = x, endValue, duration)
                    .OnUpdate(() =>
                    {
                        if (startValue < 0.5f && startValue > 0.001f)
                        {
                            interval = Mathf.Clamp(startValue, 0.05f, 1);
                            powerUpScript.BlinkOut(interval);

                        }
                        
                    })
                    .OnComplete(() =>
                    {

                        powerUpScript.ResetValues();
                    });
        



    }



}
