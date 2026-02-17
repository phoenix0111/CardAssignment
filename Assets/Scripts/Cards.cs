using UnityEngine;
using UnityEngine.UI;
using System;

public class Cards : MonoBehaviour
{
    [SerializeField] private Image frontImage;
    [SerializeField] private GameObject front;
    [SerializeField] private GameObject back;

    public Sprite CardSprite { get; private set; }

    private bool isRevealed = false;
    private bool isMatched = false;

    public event Action<Cards> OnCardRevealed;

    public void Initialize(int id, Sprite sprite)
    {
        CardSprite = sprite;
        frontImage.sprite = sprite;

        Hide();
    }

    public void Reveal()
    {
        if (isRevealed || isMatched) return;

        isRevealed = true;

        front.SetActive(true);
        back.SetActive(false);

        OnCardRevealed?.Invoke(this);
    }

    public void Hide()
    {
        isRevealed = false;

        front.SetActive(false);
        back.SetActive(true);
    }

    public void SetMatched()
    {
        isMatched = true;
    }

    
    public void OnClick()
    {
        Reveal();
    }
}
