using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HitIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var sprite = GetComponent<SpriteRenderer>();
        
        if (ReferenceEquals(sprite, null))
        {
            Destroy(gameObject);
            return;
        }

        transform.DOLocalMoveY(-1.0f, 1.0f);
        transform.DOScale(2.0f, 0.5f);
        sprite.DOFade(0.0f, 0.5f);

        Destroy(gameObject, 1.0f);
    }
}
