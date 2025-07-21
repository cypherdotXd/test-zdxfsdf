using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
public class ImageCard : CardBehaviour
{
    [SerializeField] private Sprite frontFace;

    private Image _image;
    private Button _button;
    private Sequence _flipSequence;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnCardClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnCardClicked);
    }

    public void SetFrontFace(Sprite face)
    {
        _image.sprite = face;
    }

    public bool Match(ImageCard card)
    {
        return card.frontFace == frontFace;
    }

    private void OnCardClicked()
    {
        CardsManager.Instance.RegisterCardForMatch(this);
    }

    public override void Flip(float duration = 0.2f, Action onFlipped = null)
    {
        _flipSequence?.Complete();
        _flipSequence = DOTween.Sequence();
        _flipSequence.Append(transform.DORotate(Vector3.up * 90, duration * 0.5f))
            .AppendCallback(() => _image.sprite = frontFace)
            .Append(transform.DORotate(Vector3.zero, duration * 0.5f))
            .AppendCallback(() => onFlipped?.Invoke());
    }

    public override void Fold(float duration = 0.2f, Action onFlipped = null)
    {
        
    }
}
