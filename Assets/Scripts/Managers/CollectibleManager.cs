using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    UIManager m_uiManager;
    public int CollectedDevices = 0;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        m_uiManager = Managers.Instance.UIManager;
    }

    public void CollectedSomething()
    {
        CollectedDevices++;
        m_uiManager.UpdateCollectibleCount(CollectedDevices);
    }
}
