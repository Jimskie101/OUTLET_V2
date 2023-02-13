using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class PlayerScript : MonoBehaviour
{
    GameManager m_gameManager;
    //Components
    LightController m_lightController;

    //Life
    [SerializeField][Range(0, 1f)] private float m_lifePercentage = 1;
    public float LifePercentage 
    {
        get { return m_lifePercentage;}
        set { m_lifePercentage = m_gameManager.UnliLight ? 1 : value; }
    }
    public float DimMultiplier = 0;
    public float PlayerStateDimMultiplier = 1;

    private bool m_isDead = false;
    public bool IsDead
    {
        get { return m_isDead;}
        set { m_isDead = m_gameManager.NoDeathMode ? false : value; }
    }


    private void Start()
    {
        Initializer();
    }

    private void Update()
    {
        ClosedCircuit();
        if (!IsDead && !m_charging)
            ConsumeHealth();

    }


    [Button]
    private void Initializer()
    {
        m_lightController = GetComponent<LightController>();
        m_gameManager = Managers.Instance.GameManager;
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
            LifePercentage -= Time.deltaTime * (DimMultiplier * PlayerStateDimMultiplier) * 0.01f;
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
        else m_charging = false;





    }
    [Header("Wires")]
    [SerializeField] WireBase m_hot;
    [SerializeField] WireBase m_neutral;
    [SerializeField] EnergySource m_energySource;
    bool m_charging;
    private void Regen()
    {

        if (m_hot.Source != null)
        {
            m_charging = true;
            m_energySource = m_hot.Source.GetComponent<EnergySource>();

        }
        else
        {
            m_charging = false;
        }


        if (m_energySource != null)
        {
            if (m_energySource.Charge > 0)
            {
                if (m_lightController.LightValue < m_lightController.initialValue)
                {
                    LifePercentage += Time.deltaTime * 15f * 0.01f;
                    UpdateLife();
                    m_energySource.Charge -= Time.deltaTime * 15f;
                    m_energySource.Outline.OutlineWidth = 2f * (m_energySource.Charge / m_energySource.m_initialCharge);
                }
            }
        }

    }


}
