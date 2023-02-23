using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    Color m_defaultColor = new Color32(127,127,127,255);
    private void OnEnable()
    {
        this.GetComponent<RectTransform>().localScale = Vector3.one;
        this.GetComponentInChildren<TMPro.TMP_Text>().color = m_defaultColor;
    }
}
