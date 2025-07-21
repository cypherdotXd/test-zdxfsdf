using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public abstract class CardBehaviour : MonoBehaviour
{
    [SerializeField] private Sprite backFace;
    

    public abstract void Flip(float duration = 0.2f, Action onFlipped = null);

    public abstract void Fold(float duration = 0.2f, Action onFlipped = null);

}
