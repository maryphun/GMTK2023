using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class OutroManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image screenAlpha;
    [SerializeField] private Image scorebarFill;
    [SerializeField] private TMP_Text maxcomboText;
    [SerializeField] private TMP_Text hitText;
    [SerializeField] private TMP_Text missText;
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] private int combo, hit, miss, score;

    // Start is called before the first frame update
    void Start()
    {
        screenAlpha.DOFade(0.0f, 5.0f);

        combo = Score.Instance.GetCombo();
        hit = Score.Instance.GetHit();
        miss = Score.Instance.GetMiss();
        score = Score.Instance.GetScore();

        StartCoroutine(OutroSequence());
    }

    IEnumerator OutroSequence()
    {
        yield return new WaitForEndOfFrame();
    }
}
