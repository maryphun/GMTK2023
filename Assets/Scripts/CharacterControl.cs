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
    [SerializeField] private float moveTime = 0.1f;
    [Header("Reference")]
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite leftSprite;
    [SerializeField] private DrumsticksController drumsticks;
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
                graphic.sprite = leftSprite;
                graphic.flipX = false;
                status = Status.Left;
                this.thisTransform.DOMoveX(originalPos.x - moveRange, moveTime);
                drumsticks.MoveDrumSticks(new Vector3(-moveRange * 0.5f, 0.0f, 0.0f), moveTime * 5.0f);
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (status != Status.Right)
            {
                graphic.sprite = leftSprite;
                graphic.flipX = true;
                status = Status.Right;
                this.thisTransform.DOMoveX(originalPos.x + moveRange, moveTime);
                drumsticks.MoveDrumSticks(new Vector3(moveRange * 0.5f, 0.0f, 0.0f), moveTime * 5.0f);
            }
        }
        else
        {
            if (status != Status.Middle)
            {
                graphic.sprite = idleSprite;
                graphic.flipX = false;
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
}
