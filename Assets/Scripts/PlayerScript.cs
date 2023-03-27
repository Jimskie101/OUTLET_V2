using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class PlayerScript : MonoBehaviour
{
    GameManager m_gameManager;
    //Components
    LightController m_lightController;

    [SerializeField] PlayerData m_playerData;


    //For PowerUps
    //For Super Jump
    [HideInInspector] public float PowerUpJumpBoost = 0;
    [HideInInspector] public bool LightLock = false;
    [HideInInspector] public bool Shielded = false;
    public bool FallingDisabled = false;

    public bool JumpBoosted = false;
    public void ResetFallingDisabledValue()
    {
        JumpBoosted = true;
    }

    //Life
    [SerializeField][Range(0, 1f)] private float m_lifePercentage = 1;
    public float LifePercentage
    {
        get { return m_lifePercentage; }
        set { m_lifePercentage = m_gameManager.UnliLight ? 1 : value; }
    }
    public float ChangeStateMultiplier = 1;

    private bool m_isDead = false;
    public bool IsDead
    {
        get { return m_isDead; }
        set { m_isDead = m_gameManager.NoDeathMode ? false : value; }
    }


    private void Start()
    {
        Initializer();
    }

    private void Update()
    {



        ClosedCircuit();
        if (!IsDead && !m_charging && !LightLock)
            ConsumeHealth();

    }


    [Button]
    private void Initializer()
    {
        m_lightController = GetComponent<LightController>();
        m_gameManager = Managers.Instance.GameManager;
        if (!Managers.Instance.GameData.LoadingFromSave)
            LifePercentage = m_playerData.IntialLifeValue;
    }
    public void UpdateLife()
    {

        m_lightController.LightValue = m_lightController.initialValue * LifePercentage;
        m_lightController.BulbIntensity = 1 * LifePercentage * 10;
        Managers.Instance.UIManager.UpdateHPBar(LifePercentage);
        m_lightController.EmissionUpdate();

    }

    public void ConsumeHealth()
    {
        if (LifePercentage > 0)
        {
            LifePercentage -= Time.deltaTime * (m_playerData.DimMultiplier * ChangeStateMultiplier) * 0.01f;
        }
        else
        {
            IsDead = true;
        }
        UpdateLife();
    }

    private void ClosedCircuit()
    {
        if (m_hot.Connect && m_neutral.Connect)
        {
            //Action();
            Regen();
        }
        else {
            m_charging = false;
            StopSound();
        }







    }
    [Header("Wires")]
    [SerializeField] WireBase m_hot;
    [SerializeField] WireBase m_neutral;
    [SerializeField] EnergySource m_energySource;
    [SerializeField] Keypad m_keypad;
    AudioSource m_audioSrc;
    bool m_charging;
    private void Regen()
    {

        if (m_hot.Source != null)
        {
            m_charging = true;


            if (m_hot.Source.TryGetComponent(out m_energySource))
            {
                if (m_energySource.Charge > 0)
                {
                    if (m_lightController.LightValue < m_lightController.initialValue)
                    {
                        if (m_audioSrc == null)
                        {
                            Managers.Instance.AudioManager.PlayHere("electric", this.gameObject);
                            TryGetComponent(out m_audioSrc);
                        }
                        if (m_audioSrc != null && !m_audioSrc.isPlaying)
                        {
                           Managers.Instance.AudioManager.PlayHere("electric", this.gameObject);
                        }


                        LifePercentage += Time.deltaTime * 15f * 0.01f;
                        UpdateLife();
                        m_energySource.Charge -= Time.deltaTime * 15f;
                        m_energySource.Outline.OutlineWidth = 2f * (m_energySource.Charge / m_energySource.m_initialCharge);
                    }
                    else StopSound();
                }
                else StopSound();
            }
            else if (m_hot.Source.TryGetComponent(out m_keypad))
            {
                m_keypad.Activate();
            }

        }
        else
        {
            m_charging = false;
            StopSound();

        }

    }

    public void TakeDamage(float damageAmount)
    {
        
        if (!Shielded)
            LifePercentage -= damageAmount;
            Managers.Instance.UIManager.DamageFX();
    }

    private void StopSound()
    {
        if (m_audioSrc != null && m_audioSrc.isPlaying)
        {
            m_audioSrc.Stop();
        }
    }

}
