using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndStage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
