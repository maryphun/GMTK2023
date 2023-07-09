using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SceneCtrl : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float bgmSound = 1.0f;
    [SerializeField] private float seSound = 1.0f;

    [Header("TrackManager")] 
    [SerializeField] private TrackManager trackMng;

    [Header("Screen Alpha")]
    [SerializeField] private Image alpha;

    [Header("Tutorials")]
    [SerializeField] private Transform leftArrowKey;
    [SerializeField] private Transform rightArrowKey;
    [SerializeField] private bool leftArrowKeyPressed, rightArrowKeyPressed;
    [SerializeField] private Transform drumsticks;
    [SerializeField] private Transform trackboard;
    [SerializeField] private TMP_Text controlDescription;

    private void Start()
    {
        //AudioManager.Instance.SetMusicVolume(bgmSound);
        AudioManager.Instance.SetSEMasterVolume(seSound);

        alpha.DOFade(0.0f, 5.0f);

        StartCoroutine(TutorialSequence());
    }

    IEnumerator GamePlay()
    {
        drumsticks.GetComponent<DrumsticksController>().InitializeDrumStick();
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.PlaySFX("starting");
        yield return new WaitForSeconds(2.0f);
        AudioManager.Instance.PlaySFX("3");
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.PlaySFX("2");
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.PlaySFX("1");
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.PlaySFX("start");
        yield return new WaitForSeconds(1.0f);
        AudioManager.Instance.PlayMusicWithFade("8bit-act10_stage05", 4.0f);

        trackMng.StartTrack();
    }

    IEnumerator TutorialSequence()
    {
        yield return new WaitForSeconds(3.5f);

        // arrow keys
        leftArrowKey.gameObject.SetActive(true);
        rightArrowKey.gameObject.SetActive(true);

        leftArrowKey.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        rightArrowKey.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

        leftArrowKey.GetComponent<SpriteRenderer>().DOFade(1.0f, 1.0f);
        rightArrowKey.GetComponent<SpriteRenderer>().DOFade(1.0f, 1.0f);
        controlDescription.DOFade(1.0f, 1.0f);

        // init
        leftArrowKeyPressed = false;
        rightArrowKeyPressed = false;

        while ((!leftArrowKeyPressed) || (!rightArrowKeyPressed))
        {
            if (Input.GetKey(KeyCode.LeftArrow) && !leftArrowKeyPressed)
            {
                leftArrowKeyPressed = true;
                leftArrowKey.DOMoveX(leftArrowKey.position.x - 2.0f, 2.0f);
                leftArrowKey.GetComponent<SpriteRenderer>().DOFade(0.0f, 1.5f);
            }

            if (Input.GetKey(KeyCode.RightArrow) && !rightArrowKeyPressed)
            {
                rightArrowKeyPressed = true;
                rightArrowKey.DOMoveX(rightArrowKey.position.x + 2.0f, 2.0f);
                rightArrowKey.GetComponent<SpriteRenderer>().DOFade(0.0f, 1.5f);
            }
            yield return new WaitForEndOfFrame();
        }

        // finish tutorials
        drumsticks.DOMoveY(0.0f, 2.0f);
        trackboard.DOMoveY(0.0f, 2.0f);
        controlDescription.DOFade(0.0f, 1.0f);

        yield return new WaitForSeconds(2.0f);

        StartCoroutine(GamePlay());
    }
}
