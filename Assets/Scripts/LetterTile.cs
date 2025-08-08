using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterTile : MonoBehaviour
{
    [SerializeField] private TMP_Text _letterText;
    [SerializeField] private Image _bgImage;
    [SerializeField] private Sprite _lockedSprite;

    private Color _bgDefaultColor;
    private Color _normalColor;
    private Sprite _bgDefaultSprite;
    
    private char _letter;
    public char Letter
    {
        get => _letter;
        set
        {
            _letter = value;
            _letterText.text = _letter.ToString();
        }
    }
    public Vector2Int Position { get; set; }

    private void Awake()
    {
        _normalColor = _bgImage.color;
        _bgDefaultColor = _bgImage.color;
        _bgDefaultSprite = _bgImage.sprite;
    }

    public void SetHighlighted(bool highlighted = true)
    {
        _bgImage.color = highlighted ? Color.green : _bgDefaultColor;
        _letterText.transform.localScale = highlighted ? Vector3.one * 1.2f : Vector3.one;
    }

    public void SetAsBug(bool isBug = true)
    {
        _bgDefaultColor = isBug ? Color.yellow : _normalColor;
        _bgImage.color = _bgDefaultColor;
    }

    public void SetLocked(bool locked = true)
    {
        _bgImage.sprite = locked ? _lockedSprite : _bgDefaultSprite;
    }
    
    public void OnPointerEnter()
    {
        if(!Input.GetMouseButton(0))
            return;
        var isValid = BoggleManager.Instance.RegisterForPath(this);
        if(!isValid) return;
        SetHighlighted();
    }

    public void OnPointerExit()
    {
        
    }

    public void OnPointerDown()
    {
        var valid = BoggleManager.Instance.NotifyPathStart(this);
        if(!valid) return;
        SetHighlighted();
    }

    public void OnPointerUp()
    {
        BoggleManager.Instance.NotifyPathComplete();
    }
}
