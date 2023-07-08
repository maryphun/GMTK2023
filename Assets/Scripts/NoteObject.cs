using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    [SerializeField] bool initialized = false;
    [SerializeField] float travelTimeLeft = 0.0f;
    [SerializeField] float totalTravelTime = 0.0f;

    private DrumsticksController drumsticks;
    private Vector3 startingPosition, targetPosition;
    private Transform thisTransform;

    public void Initialize(float travelTime, DrumsticksController drumstick, Vector3 startingPos, Vector3 targetPos, Transform transfrm)
    {
        thisTransform = transfrm;

        totalTravelTime = travelTime;
        travelTimeLeft = travelTime;
        drumsticks = drumstick;
        startingPosition = startingPos;
        targetPosition = targetPos;

        thisTransform.localPosition = startingPosition;

        initialized = true;
    }

    private void FixedUpdate()
    {
        if (!initialized) return;


        travelTimeLeft = Mathf.Clamp(travelTimeLeft-Time.fixedDeltaTime, 0.0f, totalTravelTime);
        if (travelTimeLeft <= 0.0f)
        {
            thisTransform.localPosition = targetPosition;
            drumsticks.HitDrum();
            Destroy(gameObject);
        }
        else
        {
            Vector3 newPos = Vector3.Lerp(targetPosition, startingPosition, travelTimeLeft / totalTravelTime);
            thisTransform.localPosition = newPos;
        }
    }
}
