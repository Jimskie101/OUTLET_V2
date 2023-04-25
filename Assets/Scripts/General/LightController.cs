using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LightController : MonoBehaviour
{


    //Reference Fields
    [SerializeField] Light m_pointLight;
    Material [] m_bulbMaterial;
    public SkinnedMeshRenderer Bulb;


    //Private Variables
    

    //Serialized Fields
    [SerializeField] float m_initialLightValue;
    [SerializeField] float m_lightValue;

    [HideInInspector]
    public float initialValue;

    Color m_bulbColor ;
    public float BulbIntensity = 10;


    
    private void OnEnable()
    {
        m_bulbMaterial = Bulb.materials;
        m_bulbColor = m_bulbMaterial[1].GetColor("_EmissionColor");
        EmissionUpdate();
    }


    
    

    public void EmissionUpdate()
    {
        m_bulbMaterial[1].SetColor("_EmissionColor", m_bulbColor * BulbIntensity);
    }




    private void Awake()
    {
        LightValue = m_initialLightValue;
        initialValue = m_initialLightValue;
    }

    public float LightValue
    {
        get
        {
            return m_lightValue;
        }

        set
        {
            m_lightValue = value;
            ChangeLightValue(m_lightValue);
        }
    }


    public void ChangeLightValue(float f_lightValue)
    {
        m_pointLight.range = Mathf.Clamp(f_lightValue * 5f, 2f, 200f);
        m_pointLight.intensity = f_lightValue * 0.01f;
    }



    private void Update()
    {
        //if(m_LightValue % 2 < 1)
        //Managers.Instance.UIManager.UpdateUIBars(m_lightValue / m_initialLightValue);

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            LightValue -= 5;
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            LightValue += 5;
        }
    }
}
