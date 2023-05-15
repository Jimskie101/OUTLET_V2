using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class CollectibleManager : MonoBehaviour
{
    UIManager m_uiManager;
    public int CollectedItems = 0;
    public int Posters = 0;
    [SerializeField] Transform m_collectibleParent;
    [SerializeField] Transform m_posterParent;
    [SerializeField] Collectible [] m_collectibles;
    [SerializeField] Collectible [] m_posterObjects;
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        m_uiManager = Managers.Instance.UIManager;
        GetCollectibles();
        if(m_collectibleParent != null )
        Invoke("CollectibleEnabler",0.1f);
        if(m_posterParent != null )
        Invoke("PosterEnabler",0.1f);
    }
    [Button]
    private void GetCollectibles()
    {
        if(m_collectibleParent != null)
        m_collectibles = m_collectibleParent.GetComponentsInChildren<Collectible>(true);
    }
    [Button]
    private void GetPosters()
    {
        m_posterObjects = m_posterParent.GetComponentsInChildren<Collectible>(true);
    }
    
    private void CollectibleEnabler()
    {
        
        if(m_collectibles != null){
        for(int i = 9; i > CollectedItems -1 ; i--)
        {
            m_collectibles[i].gameObject.SetActive(true);
        }
         m_uiManager.UpdateCollectibleCount(CollectedItems);
        }
    }

    private void PosterEnabler()
    {
        
        if(m_collectibles != null){
            for(int i = 4; i > Posters -1 ; i--)
        {
            m_posterObjects[i].gameObject.SetActive(true);
        }
        }
        
    }


    [SerializeField] Collectibles m_collectibleAsset;

    
    public void CollectedSomething()
    {
        
        Managers.Instance.UIManager.ShowTrivia(m_collectibleAsset.PostCards[CollectedItems]);
        CollectedItems++;

        m_uiManager.UpdateCollectibleCount(CollectedItems);
        Managers.Instance.GameManager.CollectibleCount = CollectedItems;
        Managers.Instance.UIManager.ShowGameUpdate("Postcard Collected");
        
                    
    }

    public void CollectedPoster()
    {

        Managers.Instance.UIManager.ShowPoster(m_collectibleAsset.Posters[Posters]);
        Posters++;

        m_uiManager.UpdateCollectibleCount(CollectedItems);
        Managers.Instance.GameManager.PostersCount = Posters;
        Managers.Instance.UIManager.ShowGameUpdate("Poster Collected");

    }
}
