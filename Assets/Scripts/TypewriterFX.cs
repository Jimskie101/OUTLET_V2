using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterFX : MonoBehaviour
{
    [SerializeField] float m_delay = 0.1f;
    [SerializeField] TMP_Text m_text;
    WaitForSecondsRealtime m_timer;
    string m_tempText;

    
    private void OnEnable()
    {
        if(m_text == null) m_text = GetComponent<TMP_Text>();
        m_tempText = m_text.text;
        m_text.text = " ";
        StartCoroutine(Type());
        m_timer = new WaitForSecondsRealtime(m_delay);
    }

    private IEnumerator Type()
    {
        foreach (char letter in m_tempText)
        {
            m_text.text =  m_text.text + letter;
            m_text.ForceMeshUpdate();
            yield return m_timer;
        }
    }
}
