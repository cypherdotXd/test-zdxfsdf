using System;
using System.Collections;
using UnityEngine;

public abstract class CardBehaviour : MonoBehaviour
{
    [SerializeField] private Sprite backFace;
    

    public abstract void Flip();

    public abstract void Fold();

}
