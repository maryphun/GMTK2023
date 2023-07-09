using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DrumsticksController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float hitAnimTime = 0.5f;
    [SerializeField] private float animationTime;

    [Header("References")]
    [SerializeField] private Transform drumstickLeft;
    [SerializeField] private Transform drumstickRight;
    [SerializeField] private CharacterControl drumCtrl;

    [Header("Debug")]
    [SerializeField] private bool isLeftStickInAnim = false;
    [SerializeField] private bool isRightStickInAnim = false;
    [SerializeField] private int combo = 0;
    [SerializeField] private bool isEnabled = false;

    private Vector3 drumstickOriginPosLeft, drumstickOriginPosRight;


    // Start is called before the first frame update
    void Start()
    {
        isEnabled = false;
    }

    public void InitializeDrumStick()
    {
        combo = 0;
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
            if (Random.Range(0, 2) == 0)
            {
                HitDrumRight(note);
            }
            else
            {
                HitDrumLeft(note);
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

        AudioManager.Instance.PlaySFX("don", 0.1f);

        yield return new WaitForSeconds(hitAnimTime);

        if (CheckIsDrumHit(note.GetNoteType()))
        {
            AddCombo();
            note.Success();
            drumCtrl.GetHit();
        }
        else
        {
            ResetCombo();
            note.Miss();
        }

        yield return new WaitForSeconds(animationTime - hitAnimTime);

        isLeftStickInAnim = false;
    }
    IEnumerator DrumRightAnimation(NoteObject note)
    {
        isRightStickInAnim = true;

        drumstickRight.GetComponent<Animator>().SetTrigger("Hit");

        AudioManager.Instance.PlaySFX("don", 0.1f);

        yield return new WaitForSeconds(hitAnimTime);

        if (CheckIsDrumHit(note.GetNoteType()))
        {
            AddCombo();
            note.Success();
            drumCtrl.GetHit();
        }
        else
        {
            ResetCombo();
            note.Miss();
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
        return (noteType == NoteType.Blue && (drumCtrl.GetDrumStatus() == Status.Left || drumCtrl.GetDrumStatus() == Status.Right))
             || (noteType == NoteType.Red && drumCtrl.GetDrumStatus() == Status.Middle);
    }

    private void AddCombo()
    {
        combo++;
    }
    private void ResetCombo()
    {
        combo = 0;
    }
}
