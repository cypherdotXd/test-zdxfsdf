using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public abstract class CardBehaviour : MonoBehaviour
{
    [SerializeField] private Sprite backFace;
    
    private Sequence flipSequence;
    
    public void FlipToShow(float duration = 0.2f, Sprite frontFace = null, Action onFlipped = null)
    {
        flipSequence = DOTween.Sequence();
        flipSequence.Append(transform.DORotate(Vector3.up * 90, duration * 0.5f))
            .AppendCallback(() => image.sprite = frontFace)
            .Append(transform.DORotate(Vector3.zero, duration * 0.5f))
            .AppendCallback(() => onFlipped?.Invoke());
    }
}
