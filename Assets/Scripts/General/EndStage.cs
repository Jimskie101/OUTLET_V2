using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;



public class EndStage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EndTheStage();

        }
    }
    public void EndTheStage()
    {
        Managers.Instance.UIManager.FadeToBlack(true);
        if (Managers.Instance.SceneHandler.GetCurrentSceneName() == "Tutorial")
        {
            StartCoroutine(MoveToNextStage());
        }
        else
        {
            StartCoroutine(Managers.Instance.UIManager.WinScreen());
        }
    }

    [SerializeField] UnityEvent m_endingEvents;
    [SerializeField] UnityEvent m_finalEvent;
    [SerializeField] UnityEvent m_returnEvent;
    public void EndGame()
    {
        Managers.Instance.UIManager.FadeToBlack(true);
        StartCoroutine(EndingLoad());

    }
    VideoPlayer m_endVideo;
    IEnumerator EndingLoad()
    {
        yield return new WaitForSeconds(2);
        m_endingEvents.Invoke();
        m_endVideo = FindObjectOfType<VideoPlayer>();
        m_endVideo.loopPointReached += EndReached;
        Managers.Instance.UIManager.FadeInFromBlack();

    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        Managers.Instance.UIManager.FadeToBlack(true);
        StartCoroutine(ThankYou());
    }

    IEnumerator ThankYou()
    {
        yield return new WaitForSeconds(2);
        m_finalEvent.Invoke();
        Managers.Instance.UIManager.FadeInFromBlack();
        StartCoroutine(ReturnToMain());

    }

    IEnumerator ReturnToMain()
    {
        yield return new WaitForSeconds(7);
        m_returnEvent.Invoke();
    }


    IEnumerator MoveToNextStage()
    {
        yield return new WaitForSeconds(2);
        {
            Managers.Instance.SceneHandler.LoadStage(Managers.Instance.SceneHandler.GetCurrentScene() + 1);
        }
    }

    [SerializeField] Transform m_player;
    [SerializeField] MoverAndRotator m_train;
    public void FirstEnd()
    {
        Managers.Instance.CutsceneManager.PlayCutscene(4);
        Managers.Instance.TaskManager.NextTask();
        StartCoroutine(MoveTrain());
    }
    IEnumerator MoveTrain()
    {
        m_player.GetComponent<CharacterController>().enabled = false;
        m_player.GetComponent<PlayerMovement>().enabled = false;
        m_player.SetParent(transform);
        yield return new WaitForSeconds(2f);
        Managers.Instance.AudioManager.PlayHere("train", transform.parent.gameObject, false, true);
        m_train.MoveMe();
        Managers.Instance.UIManager.FadeToBlack(true);
        StartCoroutine(Managers.Instance.UIManager.WinScreen());
    }


}
