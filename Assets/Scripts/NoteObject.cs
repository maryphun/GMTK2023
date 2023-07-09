using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    [SerializeField] bool initialized = false;
    [SerializeField] float travelTimeLeft = 0.0f;
    [SerializeField] float totalTravelTime = 0.0f;

    private DrumsticksController drumsticks;
    [SerializeField] private Vector3 startingPosition, targetPosition;
    private Transform thisTransform;
    private bool orderedDrumHit = false;
    private bool noteMissed = false;
    private bool objDestroyed = false;
    private NoteType noteType;

    public void Initialize(float travelTime, DrumsticksController drumstick, Vector3 startingPos, Vector3 targetPos, Transform transfrm, NoteType noteTyp)
    {
        thisTransform = transfrm;

        totalTravelTime = travelTime;
        travelTimeLeft = travelTime;
        drumsticks = drumstick;
        startingPosition = startingPos;
        targetPosition = targetPos;
        noteType = noteTyp;

        thisTransform.localPosition = startingPosition;

        initialized = true;
    }

    private void FixedUpdate()
    {
        if (!initialized) return;


        travelTimeLeft = Mathf.Max(travelTimeLeft-Time.fixedDeltaTime, 0.0f);
        if (!orderedDrumHit && travelTimeLeft <= drumsticks.GetDrumAnimationTime())
        {
            orderedDrumHit = true;
            drumsticks.HitDrum(this);
        }
        if (travelTimeLeft <= 0.0f || noteMissed)
        {
            if (noteMissed)
            {
                Vector3 newPos = Vector3.Lerp(targetPosition, startingPosition, travelTimeLeft / totalTravelTime);
                thisTransform.localPosition = newPos;
            }
            if (!objDestroyed)
            {
                thisTransform.localPosition = targetPosition;
                objDestroyed = true;
            }
        }
        else
        {
            Vector3 newPos = Vector3.Lerp(targetPosition, startingPosition, travelTimeLeft / totalTravelTime);
            thisTransform.localPosition = newPos;
        }
    }

    public NoteType GetNoteType()
    {
        return noteType;
    }

    public void Success()
    {
        GetComponent<SpriteRenderer>().DOFade(0.0f, 0.4f);
        thisTransform.DOScale(1.5f, 0.4f);

        Destroy(gameObject, 0.5f);
    }

    public void Miss()
    {
        noteMissed = true;
        var newPos = new Vector3(targetPosition.x - Mathf.Abs(startingPosition.x - targetPosition.x), targetPosition.y);

        startingPosition = targetPosition;
        targetPosition = newPos;

        // reset time 
        travelTimeLeft = totalTravelTime + Time.fixedDeltaTime;

        Destroy(gameObject, totalTravelTime);

        GetComponent<SpriteRenderer>().DOFade(0.0f, 1.0f);
    }
}
