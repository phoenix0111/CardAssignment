using UnityEngine;
using UnityEngine.UI;
using System;

public class Cards : MonoBehaviour
{
    public Sprite CardSprite { get; private set; }

    [Header("Card Details")]
    [SerializeField] private Image frontImage;
    [SerializeField] private GameObject front;
    [SerializeField] private GameObject back;

    private bool isRevealed = false;
    private bool isMatched = false;

    public event Action<Cards> OnCardRevealed;
    private bool canInteract = true;


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

    public void SetInteractable(bool value)
    {
        canInteract = value;
    }

    public void OnClick()
    {
        if (!canInteract) return;

        Reveal();
    }
}
