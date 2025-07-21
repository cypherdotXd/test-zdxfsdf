using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
public class ImageCard : Card
{
    [SerializeField] private Image frontFaceImage;
    [SerializeField] private Image frontFaceBgImage;
    
    public bool IsFolded { get; private set; } = true;
    
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

    private void Start()
    {
        _flipSequence = DOTween.Sequence();
        _flipSequence.Append(transform.DORotate(Vector3.up * 90, 0.15f))
            .AppendCallback(() => frontFaceBgImage.gameObject.SetActive(!frontFaceBgImage.gameObject.activeSelf))
            .Append(transform.DORotate(Vector3.zero, 0.15f))
            .SetAutoKill(false)
            .Pause();
    }

    public void SetFrontFace(Sprite face)
    {
        _image.sprite = face;
    }
    private void OnCardClicked()
    {
        if (!IsFolded) return;
        
        Flip();
        CardsManager.Instance.RegisterCardForMatch(this);
    }
    public override bool Match(Card card)
    {
        if (card is not ImageCard imageCard) return false;
        return frontFaceImage.sprite == imageCard.frontFaceImage.sprite;
    }
    public override void Flip()
    {
        _flipSequence.Restart();
        IsFolded = false;
    }

    public override void Fold()
    {
        _flipSequence.PlayBackwards();
        IsFolded = true;
    }
}
