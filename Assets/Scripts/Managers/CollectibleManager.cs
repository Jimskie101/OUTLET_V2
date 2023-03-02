using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class CollectibleManager : MonoBehaviour
{
    UIManager m_uiManager;
    public int CollectedItems = 0;
    [SerializeField] Transform m_collectibleParent;
    [SerializeField] Collectible [] m_collectibles;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        m_uiManager = Managers.Instance.UIManager;
        if(m_collectibleParent != null )Invoke("CollectibleEnabler",0.1f);
    }
    [Button]
    private void GetCollectibles()
    {
        m_collectibles = m_collectibleParent.GetComponentsInChildren<Collectible>();
    }

    private void CollectibleEnabler()
    {
        for(int i = 9; i > CollectedItems -1 ; i--)
        {
            m_collectibles[i].gameObject.SetActive(true);
        }
         m_uiManager.UpdateCollectibleCount(CollectedItems);
    }

    public void CollectedSomething()
    {
        CollectedItems++;
        m_uiManager.UpdateCollectibleCount(CollectedItems);
        Managers.Instance.GameManager.CollectibleCount = CollectedItems;
    }
}
