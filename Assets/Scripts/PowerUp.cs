using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


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
    PlayerScript m_playerScript;



    WaitForSeconds disableDelay;

     Vector3 m_rotation = new Vector3(0, 360f, 0);
    MeshRenderer m_mesh;
    private void Start()
    {
        m_mesh = GetComponentInChildren<MeshRenderer>();
        disableDelay = new WaitForSeconds(0.5f);
        transform.DOLocalRotate(m_rotation, 4f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }

    private void ApplyEffect()
    {
        switch (m_powerUpType)
        {
            case PowerUpType.SuperJump:
                m_playerScript.PowerUpJumpBoost = m_powerUpData.JumpAddedValue;
                m_playerScript.FallingDisabled = true;
                Managers.Instance.UIManager.PowerUpCountdown(m_powerUpData.JumpBoostDuration, this)
                ; break;
            case PowerUpType.Shield:
                m_playerScript.Shielded = true;
                Managers.Instance.UIManager.PowerUpCountdown(m_powerUpData.ShieldDuration, this)
                ; break;
            case PowerUpType.LightLock:
                m_playerScript.LightLock = true;
                Managers.Instance.UIManager.PowerUpCountdown(m_powerUpData.LightLockDuration, this)
                ; break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_mesh.enabled = false;
            Managers.Instance.AudioManager.PlayHere("collect",other.gameObject,false,true);
            transform.GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(Disabling());
            if (other.TryGetComponent(out m_playerScript))
                ApplyEffect();
        }
    }

    public void ResetValues()
    {
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


    private void OnDisable()
    {
        m_mesh.enabled = true;
    }

    IEnumerator Disabling()
    {
        yield return disableDelay;
        this.gameObject.SetActive(false);
    }
}
