using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class OutroManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image screenAlpha;
    [SerializeField] private Image scorebarFill;
    [SerializeField] private RectTransform drumchan;
    [SerializeField] private TMP_Text maxcomboText;
    [SerializeField] private TMP_Text hitText;
    [SerializeField] private TMP_Text missText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text conclusionText;
    [SerializeField] private TMP_Text restartText;

    [SerializeField] private int combo, hit, miss, score;
    [SerializeField] private Sprite veryhurt, hurt, happy, veryhappy;

    // Start is called before the first frame update
    void Start()
    {
        combo = Score.Instance.GetCombo();
        hit = Score.Instance.GetHit();
        miss = Score.Instance.GetMiss();
        score = Score.Instance.GetScore();

        StartCoroutine(OutroSequence());
    }

    IEnumerator OutroSequence()
    {
        screenAlpha.DOFade(0.0f, 5.0f);

        yield return new WaitForSeconds(5.0f);

        float fillAmount = ((float)hit / (float)Score.Instance.GetMaxNoteCnt());
        Debug.Log("fill amount:" + fillAmount.ToString());
        scorebarFill.DOFillAmount(fillAmount, 2.0f);

        AudioManager.Instance.PlaySFX("data");
        for (float waitTime = 2.0f; waitTime >= 0.0f; waitTime -= Time.deltaTime)
        {
            scoreText.text = ((int)((float)score * (2.0f - waitTime))).ToString();
            
            yield return new WaitForEndOfFrame();
        }

        scoreText.text = score.ToString();
        scorebarFill.DOComplete();
        AudioManager.Instance.PlaySFX("Reveal");

        yield return new WaitForSeconds(0.5f);

        maxcomboText.text = combo.ToString();
        AudioManager.Instance.PlaySFX("Reveal");

        yield return new WaitForSeconds(0.5f);

        hitText.text = hit.ToString();
        AudioManager.Instance.PlaySFX("Reveal");

        yield return new WaitForSeconds(0.5f);

        missText.text = miss.ToString();
        AudioManager.Instance.PlaySFX("Reveal");

        yield return new WaitForSeconds(1.5f);

        AudioManager.Instance.PlaySFX("don");
        drumchan.GetComponent<Image>().sprite = GetDrumSprite(fillAmount);
        drumchan.DOAnchorPos(new Vector3(-247f, -184f, 0.0f), 2.0f);
        StartCoroutine(SetConclusionText(GetText(fillAmount), 3.0f));

        yield return new WaitForSeconds(3.5f);

        AudioManager.Instance.PlaySFX("messageSingle", 0.5f);
        restartText.DOFade(1.0f, 1.0f);

        while (!Input.GetKey(KeyCode.Z))
        {
            yield return new WaitForEndOfFrame();
        }

        AudioManager.Instance.PlaySFX("kirakira", 0.75f);
        screenAlpha.DOFade(1.0f, 1.0f);
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(1);
    }

    IEnumerator SetConclusionText(string txt, float time)
    {
        float textInterval = time / ((float)(txt.Length));
        for (int i = 0; i <= txt.Length; i++)
        {
            conclusionText.SetText(txt.Substring(0, i));

            AudioManager.Instance.PlaySFX("messageSingle", 0.5f);

            yield return new WaitForSeconds(textInterval);
        }
    }

    Sprite GetDrumSprite(float fillAmt)
    {
        if (fillAmt < 0.1f)
        {
            return veryhappy;
        }
        if (fillAmt < 0.5f)
        {
            return happy;
        }
        if (fillAmt < 0.9f)
        {
            return hurt;
        }
        
        return veryhurt;
    }

    string GetText(float fillAmt)
    {
        if (fillAmt < 0.1f)
        {
            return "Drum-chan is very happy!!!";
        }
        if (fillAmt < 0.5f)
        {
            return "Mediocore score but drum-chan is secretly happy because he got to avoid most of the hit this time.";
        }
        if (fillAmt < 0.9f)
        {
            return "Drum-chan is hurt, but he is still trying to cheer for your relatively high score.";
        }

        return "You get a near perfect score but at what cost? Drum-chan is once again, very hurt...";
    }
}
