using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image alpha; 
    [SerializeField] private TMP_Text text; 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        yield return new WaitForSeconds(2.0f);

        alpha.DOFade(0.0f, 5.0f);

        yield return new WaitForSeconds(6.0f);

        StartCoroutine(SetText("Hello, my name is Drum-Chan.", 3.0f));

        yield return new WaitForSeconds(4.0f);

        StartCoroutine(SetText("As you can see, I’m a Taiko drum;", 3.0f));

        yield return new WaitForSeconds(4.0f);

        StartCoroutine(SetText("More specifically, I’m a Taiko drum that hates pain…", 3.0f));

        yield return new WaitForSeconds(4.0f);

        StartCoroutine(SetText("The role of being a drum really hurts! >_<", 3.0f));

        yield return new WaitForSeconds(4.0f);

        StartCoroutine(SetText("Even though this might upset my owner,", 3.0f));

        yield return new WaitForSeconds(4.0f);

        StartCoroutine(SetText("From now on, I’ll try my best to avoid getting hit!!", 3.0f));

        yield return new WaitForSeconds(4.0f);

        alpha.DOFade(1.0f, 5.0f);

        yield return new WaitForSeconds(5.0f);

        SceneManager.LoadScene(1);
    }

    IEnumerator SetText(string txt, float time)
    {
        float textInterval = time / ((float)(txt.Length));
        for (int i = 0; i <= txt.Length; i++)
        {
            text.SetText(txt.Substring(0, i));

            yield return new WaitForSeconds(textInterval);
        }
    }
}
