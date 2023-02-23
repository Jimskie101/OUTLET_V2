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
}
