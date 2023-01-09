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


    private void Update()
    {
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
        m_lightController.EmissionUpdate();

    }

    public void ConsumeHealth()
    {
        LifePercentage -= Time.deltaTime * DimMultiplier * 0.01f;
        UpdateLife();
    }


}
