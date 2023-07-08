using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DrumsticksController : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float animationTime;

    [Header("References")]
    [SerializeField] private Transform drumstickLeft;
    [SerializeField] private Transform drumstickRight;

    [Header("Debug")]
    [SerializeField] private bool isLeftStickInAnim = false;
    [SerializeField] private bool isRightStickInAnim = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!isLeftStickInAnim)
            {
                HitDrumLeft();
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isRightStickInAnim)
            {
                HitDrumRight();
            }
        }
    }

    public void HitDrum()
    {
        if (isLeftStickInAnim && isRightStickInAnim)
        {
            // both hand is used
            Debug.Log("Both hand is currently in use! Please change the note timing");
        }
        if (isLeftStickInAnim)
        {
            HitDrumRight();
        }
        else if (isRightStickInAnim)
        {
            HitDrumLeft();
        }
        else
        {
            // both hand are not used
            // so random a hand
            if (Random.Range(0, 2) == 0)
            {
                HitDrumRight();
            }
            else
            {
                HitDrumLeft();
            }
        }
    }

    public void HitDrumLeft()
    {
        StartCoroutine(DrumLeftAnimation());
    }
    public void HitDrumRight()
    {
        StartCoroutine(DrumRightAnimation());
    }

    IEnumerator DrumLeftAnimation()
    {
        isLeftStickInAnim = true;

        Vector2 originalPos = drumstickLeft.localPosition;
        drumstickLeft.DOLocalMove(new Vector3(originalPos.x + 2.0f, originalPos.y - 2f), animationTime / 2.0f);
        drumstickLeft.DORotate(new Vector3(0.0f, 0.0f, -24.0f), animationTime / 2.0f);

        yield return new WaitForSeconds(animationTime/2.0f);

        drumstickLeft.DOLocalMove(originalPos, animationTime / 2.0f);
        drumstickLeft.DORotate(new Vector3(0.0f, 0.0f, 0.0f), animationTime / 2.0f);

        yield return new WaitForSeconds(animationTime / 2.0f);
        isLeftStickInAnim = false;
    }
    IEnumerator DrumRightAnimation()
    {
        isRightStickInAnim = true;

        Vector2 originalPos = drumstickRight.localPosition;
        drumstickRight.DOLocalMove(new Vector3(originalPos.x - 2.0f, originalPos.y - 2f), animationTime / 2.0f);
        drumstickRight.DORotate(new Vector3(0.0f, 0.0f, 24.0f), animationTime / 2.0f);

        yield return new WaitForSeconds(animationTime/2.0f);

        drumstickRight.DOLocalMove(originalPos, animationTime / 2.0f);
        drumstickRight.DORotate(new Vector3(0.0f, 0.0f, 0.0f), animationTime / 2.0f);

        yield return new WaitForSeconds(animationTime / 2.0f);

        isRightStickInAnim = false;
    }

    public float GetDrumAnimationTime()
    {
        return animationTime;
    }
}
