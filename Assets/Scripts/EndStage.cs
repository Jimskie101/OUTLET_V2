using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Managers.Instance.UIManager.FadeToBlack(true);
            StartCoroutine(MoveToNextStage());
        }
    }

    IEnumerator MoveToNextStage()
    {
        yield return new WaitForSeconds(2);
        {
            Managers.Instance.SceneHandler.LoadStage(Managers.Instance.SceneHandler.GetCurrentScene() +1);
        }
    }

    [SerializeField] Transform m_player;
    [SerializeField] MovePlatform m_train;
    public void FirstEnd()
    {
        Managers.Instance.CutsceneManager.PlayCutscene(4);
        Managers.Instance.TaskManager.NextTask();
        StartCoroutine(MoveTrain());
    }
    IEnumerator MoveTrain()
    {
        Managers.Instance.AudioManager.PlayHere("train", transform.parent.gameObject, true);
        m_player.GetComponent<CharacterController>().enabled = false;
        m_player.SetParent(transform);
        yield return new WaitForSeconds(2f);
        m_train.MoveMe(true);
        Managers.Instance.UIManager.FadeToBlack(true);
    }
}
