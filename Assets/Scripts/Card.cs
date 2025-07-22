using System;
using System.Collections;
using UnityEngine;

public abstract class Card : MonoBehaviour // Card => ImageCard, TextCard, 3dCard
{
    [SerializeField] private Sprite backFace;
    

    public abstract void Flip();

    public abstract void Fold();

    public abstract bool Match(Card card);
}
