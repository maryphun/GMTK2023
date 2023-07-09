using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public enum Status
{
    Left,
    Right,
    Middle
};

public class CharacterControl : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float moveRange = 0.25f;
    [SerializeField] private float drumstickFollowMultiplier = 1.5f;
    [SerializeField] private float drumstickFollowDelay = 7.0f;
    [SerializeField] private float moveTime = 0.1f;
    [Header("Reference")]
    [SerializeField] private DrumsticksController drumsticks;
    [SerializeField] private Animator animator;
    [Header("Debug")]
    [SerializeField] private Status status;

    private SpriteRenderer graphic;
    private Transform thisTransform;
    private Vector2 originalPos;
    


    // Start is called before the first frame update
    void Start()
    {
        graphic = GetComponent<SpriteRenderer>();
        status = Status.Middle;
        thisTransform = GetComponent<Transform>();
        originalPos = base.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (status != Status.Left)
            {
                animator.Play("PlayerTurn");
                animator.SetBool("IsTurn", true);
                graphic.flipX = true;
                status = Status.Left;
                this.thisTransform.DOMoveX(originalPos.x - moveRange, moveTime);
                drumsticks.MoveDrumSticks(new Vector3(-moveRange * drumstickFollowMultiplier, 0.0f, 0.0f), moveTime * drumstickFollowDelay);

                AudioManager.Instance.PlaySFX("turn", 0.01f);
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (status != Status.Right)
            {
                animator.Play("PlayerTurn");
                animator.SetBool("IsTurn", true);
                graphic.flipX = false;
                status = Status.Right;
                this.thisTransform.DOMoveX(originalPos.x + moveRange, moveTime);
                drumsticks.MoveDrumSticks(new Vector3(moveRange * 1.2f, 0.0f, 0.0f), moveTime * 7.0f);

                AudioManager.Instance.PlaySFX("turn", 0.01f);
            }
        }
        else
        {
            if (status != Status.Middle)
            {
                animator.Play("PlayerReturn");
                animator.SetBool("IsTurn", false);
                //graphic.flipX = false;
                status = Status.Middle;
                this.thisTransform.DOMoveX(originalPos.x, moveTime);
                drumsticks.MoveDrumSticks(new Vector3() * 0.5f, moveTime * 5.0f); 
            }
        }
    }

    public Status GetDrumStatus()
    {
        return status;
    }

    public void GetHit()
    {
        if (status == Status.Middle)
        {
            animator.Play("PlayerGetHitMid");
        }
        else
        {
            animator.Play("PlayerGetHitSide");
        }
    }
}
