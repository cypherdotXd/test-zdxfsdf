using System;
using System.Collections;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [SerializeField] private Sprite backFace;
    

    public abstract void Flip();

    public abstract void Fold();

    public abstract bool Match(Card card);
}
