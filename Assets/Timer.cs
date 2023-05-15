using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Timer : MonoBehaviour
{
    public TMP_Text timer;
    public float time = 5;
    bool Tigil = true;
    // Start is called before the first frame update
    void Start()
    {
        DOVirtual.DelayedCall(2, () => Tigil = false);
    }
    public void Stop()
    {
        Tigil = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!Tigil)
        {
            if (time > 0)
            {
                time = time - Time.deltaTime;
                timer.text = "" + ((int)time) + "";
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

    }
}
