using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class PlayerScript : MonoBehaviour

{   //Components
    [SerializeField] LightController m_lightController;

    //Life
    [Range(0, 1f)] public float LifePercentage = 1;
    public float DimMultiplier = 0;
    public bool isDead = false;


    private void Update()
    {
        ClosedCircuit();
        if (!isDead && !m_charging)
            ConsumeHealth();

    }


    [Button]
    private void Initializer()
    {
        m_lightController = GetComponent<LightController>();
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
            LifePercentage -= Time.deltaTime * DimMultiplier * 0.01f;
        }
        else
        {
            isDead = true;
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
        m_charging = true;
        m_energySource = m_hot.Source.GetComponent<EnergySource>();
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
