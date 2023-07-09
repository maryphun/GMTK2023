using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class DrumsticksController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float hitAnimTime = 0.5f;
    [SerializeField] private float animationTime;

    [Header("References")]
    [SerializeField] private Transform drumstickLeft;
    [SerializeField] private Transform drumstickRight;
    [SerializeField] private CharacterControl drumCtrl;
    [SerializeField] private TrackManager trackMng;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject missPrefab;
    [SerializeField] private TMP_Text comboCntText;
    [SerializeField] private TMP_Text missComboCntText;

    [Header("Debug")]
    [SerializeField] private bool isLeftStickInAnim = false;
    [SerializeField] private bool isRightStickInAnim = false;
    [SerializeField] private int combo = 0;
    [SerializeField] private int missCombo = 0;
    [SerializeField] private int score = 0;
    [SerializeField] private int hit = 0;
    [SerializeField] private int miss = 0;
    [SerializeField] private int maxCombo = 0;
    [SerializeField] private bool isEnabled = false;

    private Vector3 drumstickOriginPosLeft, drumstickOriginPosRight;


    // Start is called before the first frame update
    void Start()
    {
        isEnabled = false;
    }

    public void InitializeDrumStick()
    {
        score = 0;
        score = 0;
        missCombo = 0;
        maxCombo = 0;
        hit = 0;
        miss = 0;
        drumstickOriginPosLeft = new Vector3();
        drumstickOriginPosRight = new Vector3();
        isEnabled = true;
    }

    public void MoveDrumSticks(Vector3 offset, float time)
    {
        if (!isEnabled) return;
        drumstickLeft.DOKill(false);
        drumstickRight.DOKill(false);
        drumstickLeft.DOLocalMove(drumstickOriginPosLeft + offset, time);
        drumstickRight.DOLocalMove(drumstickOriginPosRight + offset, time);
    }

    public void HitDrum(NoteObject note)
    {
        if (isLeftStickInAnim && isRightStickInAnim)
        {
            // both hand is used
            Debug.Log("Both hand is currently in use! Please change the note timing");
        }
        if (isLeftStickInAnim)
        {
            HitDrumRight(note);
        }
        else if (isRightStickInAnim)
        {
            HitDrumLeft(note);
        }
        else
        {
            // both hand are not used
            // so random a hand
            if (note.GetNoteType() == NoteType.Red)
            {
                if (Random.Range(0, 2) == 0)
                {
                    HitDrumRight(note);
                }
                else
                {
                    HitDrumLeft(note);
                }
            }
            else
            {
                if (note.GetNoteType() == NoteType.Blue_Right)
                {
                    HitDrumRight(note);
                }
                else if (note.GetNoteType() == NoteType.Blue_Left)
                {
                    HitDrumLeft(note);
                }
            }
        }
    }

    public void HitDrumLeft(NoteObject note)
    {
        StartCoroutine(DrumLeftAnimation(note));
    }
    public void HitDrumRight(NoteObject note)
    {
        StartCoroutine(DrumRightAnimation(note));
    }

    IEnumerator DrumLeftAnimation(NoteObject note)
    {
        isLeftStickInAnim = true;

        drumstickLeft.GetComponent<Animator>().SetTrigger("Hit");

        //AudioManager.Instance.PlaySFX("don", 0.1f);

        yield return new WaitForSeconds(hitAnimTime);
        
        if (CheckIsDrumHit(note.GetNoteType()))
        {
            AddCombo();
            note.Success();
            drumCtrl.GetHit();
            AddScore(((int)(75.0f / (float)note.GetTotalTravelTime())));

            var obj = Instantiate(hitPrefab, trackMng.GetEndPosition());
            obj.transform.localPosition = new Vector3();

            trackMng.GetEndPosition().GetComponent<Animator>().Play("Hit");
        }
        else
        {
            ResetCombo();
            note.Miss();

            var obj = Instantiate(missPrefab, trackMng.GetEndPosition());
            obj.transform.localPosition = new Vector3();

            trackMng.GetEndPosition().GetComponent<Animator>().Play("Miss");

            AudioManager.Instance.PlaySFX("MissNote2");
        }

        yield return new WaitForSeconds(animationTime - hitAnimTime);

        isLeftStickInAnim = false;
    }
    IEnumerator DrumRightAnimation(NoteObject note)
    {
        isRightStickInAnim = true;

        drumstickRight.GetComponent<Animator>().SetTrigger("Hit");

        //AudioManager.Instance.PlaySFX("don", 0.1f);

        yield return new WaitForSeconds(hitAnimTime);

        if (CheckIsDrumHit(note.GetNoteType()))
        {
            AddCombo();
            note.Success();
            drumCtrl.GetHit();
            AddScore(((int)(75.0f / (float)note.GetTotalTravelTime())));

            var obj = Instantiate(hitPrefab, trackMng.GetEndPosition());
            obj.transform.localPosition = new Vector3();

            trackMng.GetEndPosition().GetComponent<Animator>().Play("Hit");
        }
        else
        {
            ResetCombo();
            note.Miss();

            var obj = Instantiate(missPrefab, trackMng.GetEndPosition());
            obj.transform.localPosition = new Vector3();

            trackMng.GetEndPosition().GetComponent<Animator>().Play("Miss");

            AudioManager.Instance.PlaySFX("MissNote2");
        }

        yield return new WaitForSeconds(animationTime - hitAnimTime);

        isRightStickInAnim = false;
    }

    public float GetDrumAnimationTime()
    {
        return animationTime;
    }

    private bool CheckIsDrumHit(NoteType noteType)
    {
        return (noteType == NoteType.Blue_Left && drumCtrl.GetDrumStatus() == Status.Left)
             || (noteType == NoteType.Blue_Right && drumCtrl.GetDrumStatus() == Status.Right)
             || (noteType == NoteType.Red && drumCtrl.GetDrumStatus() == Status.Middle);
    }

    private void AddCombo()
    {
        combo++;
        if (combo > maxCombo) maxCombo = combo;
        hit++;
        comboCntText.text = combo.ToString();

        missCombo = 0;
        missComboCntText.gameObject.SetActive(false);
    }
    private void ResetCombo()
    {
        combo = 0;
        miss++;
        comboCntText.text = string.Empty;

        missCombo++;
        missComboCntText.text = "Miss Streak: " + missCombo.ToString();
        missComboCntText.gameObject.SetActive(true);
    }

    public void SubmitResult()
    {
        Score.Instance.SetMaxNoteCnt(trackMng.GetTotalNoteCount());
        Score.Instance.SetResult(maxCombo, hit, miss, score);

        Debug.Log("Submit result");
    }

    public void AddScore(int value)
    {
        score += value;
    }
}
