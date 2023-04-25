using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIHighlighter : MonoBehaviour
{
    EventSystem m_eventSystem;
    [SerializeField] Button m_highlight;
    private void Awake()
    {
        m_eventSystem = FindObjectOfType<EventSystem>();
    }
    private void OnEnable()
    {
        m_eventSystem.SetSelectedGameObject(null);
        
        m_eventSystem.SetSelectedGameObject(m_highlight.gameObject);
        m_highlight.OnSelect(null);
    }
}
