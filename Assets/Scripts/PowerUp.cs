using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasyButtons;


enum PowerUpType
{
    SuperJump,
    Shield,
    LightLock,
}

public class PowerUp : MonoBehaviour
{
    [SerializeField] PowerUpData m_powerUpData;
    [SerializeField] PowerUpType m_powerUpType;
    [SerializeField] GameObject m_powerUpFX;
    PlayerScript m_playerScript;



    WaitForSeconds disableDelay;
    WaitForSeconds m_reEnablerDelay;

    Vector3 m_rotation = new Vector3(0, 360f, 0);
    MeshRenderer m_mesh;
    [SerializeField] MeshFilter m_meshFilter;
    private void Start()
    {
        m_particles = transform.GetComponentInChildren<ParticleSystem>();
        if (m_mesh == null)
            m_mesh = GetComponentInChildren<MeshRenderer>();
        disableDelay = new WaitForSeconds(0.5f);
        m_reEnablerDelay = new WaitForSeconds(m_powerUpData.RespawnTime);
        transform.DOLocalRotate(m_rotation, 4f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        m_blinkTimeSlow = new WaitForSeconds(0.20f);
        m_blinkTimeFast = new WaitForSeconds(0.10f);
    }

    private void ApplyEffect()
    {
        switch (m_powerUpType)
        {
            case PowerUpType.SuperJump:
                m_playerScript.PowerUpJumpBoost = m_powerUpData.JumpAddedValue;
                m_playerScript.FallingDisabled = true;
                Managers.Instance.PowerUpManager.PowerUpCountdown(m_powerUpData.JumpBoostDuration, this, m_powerUpFX)
                ; break;
            case PowerUpType.Shield:
                m_playerScript.Shielded = true;
                Managers.Instance.PowerUpManager.PowerUpCountdown(m_powerUpData.ShieldDuration, this, m_powerUpFX)
                ; break;
            case PowerUpType.LightLock:
                m_playerScript.LightLock = true;
                Managers.Instance.PowerUpManager.PowerUpCountdown(m_powerUpData.LightLockDuration, this, m_powerUpFX)
                ; break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_mesh.enabled = false;
            Managers.Instance.AudioManager.PlayHere("collect", this.gameObject, false, true);
            m_particles.Play();
            if (other.TryGetComponent(out m_playerScript))
                ApplyEffect();
            StartCoroutine(Disabling());
        }
    }

    public void ResetValues()
    {
        if(m_blinker != null)
        StopCoroutine(m_blinker);
        m_powerUpFX.SetActive(false);
        switch (m_powerUpType)
        {
            case PowerUpType.SuperJump:
                m_playerScript.PowerUpJumpBoost = 0f;
                m_playerScript.ResetFallingDisabledValue();
                ; break;
            case PowerUpType.Shield:
                m_playerScript.Shielded = false;
                ; break;
            case PowerUpType.LightLock:
                m_playerScript.LightLock = false;
                ; break;
        }
        
    }

    WaitForSeconds m_blinkTimeSlow;
    WaitForSeconds m_blinkTimeFast;
    Coroutine m_blinker = null;
    float m_blinkInterval = 0;
    [Button]
    public void BlinkOut(float interval)
    {
        if (m_blinkInterval <= 0)
        {
            if (m_blinker == null)
            {
                m_blinkInterval = interval;
                
                m_powerUpFX.SetActive(false);
                if(interval < 0.30)
                m_blinker = StartCoroutine(BlinkBack(m_blinkTimeFast));
                else
                m_blinker = StartCoroutine(BlinkBack(m_blinkTimeSlow));

            }
        }
        else
        m_blinkInterval -= Time.deltaTime;


    }

    IEnumerator BlinkBack(WaitForSeconds time)
    {
        yield return time;
        m_powerUpFX.SetActive(true);
        m_blinker = null;
        yield return null;
    }

    private void OnDisable()
    {
        m_mesh.enabled = true;
    }

    [SerializeField] SphereCollider m_sphereCollider;
    IEnumerator Disabling()
    {
        yield return disableDelay;
        if (transform.GetChild(1).TryGetComponent<MeshRenderer>(out m_mesh))
            m_mesh.enabled = false;
        if (TryGetComponent<SphereCollider>(out m_sphereCollider))
            m_sphereCollider.enabled = false;
        StartCoroutine(ReEnable());
    }
    ParticleSystem m_particles = null;
    IEnumerator ReEnable()
    {
        transform.GetComponentInChildren<ParticleSystem>();
        if (m_particles != null)
            m_particles.Play();
        yield return m_reEnablerDelay;
        if (transform.GetChild(1).TryGetComponent<MeshRenderer>(out m_mesh))
            m_mesh.enabled = true;
        if (TryGetComponent<SphereCollider>(out m_sphereCollider))
            m_sphereCollider.enabled = true;

    }

    [SerializeField] Mesh[] m_powerupMesh;

    [Button]
    private void ChangeMesh()
    {
        m_meshFilter = GetComponentInChildren<MeshFilter>();
        switch (m_powerUpType)
        {
            case PowerUpType.SuperJump:
                m_meshFilter.mesh = m_powerupMesh[0];
                ; break;
            case PowerUpType.Shield:
                m_meshFilter.mesh = m_powerupMesh[1];
                ; break;
            case PowerUpType.LightLock:
                m_meshFilter.mesh = m_powerupMesh[2];
                ; break;
        }

    }
}
