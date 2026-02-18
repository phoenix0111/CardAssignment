using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Cards : MonoBehaviour
{
    public Sprite CardSprite { get; private set; }

    [Header("Card Details")]
    [SerializeField] private Image frontImage;
    [SerializeField] private GameObject front;
    [SerializeField] private GameObject back;

    [Header("Flip Animation")]
    [SerializeField] private float flipDuration = 0.25f;

    private bool isFlipping = false;


    private bool isRevealed = false;
    private bool isMatched = false;

    public event Action<Cards> OnCardRevealed;
    private bool canInteract = true;

    public AudioClip cardPickSFX;

    public void Initialize(int id, Sprite sprite)
    {
        CardSprite = sprite;
        frontImage.sprite = sprite;

        Hide();
    }

    public void Reveal()
    {
        if (isRevealed || isMatched || isFlipping) return;

        StartCoroutine(Flip(true));
    }


    public void Hide()
    {
        if (isFlipping) return;

        StartCoroutine(Flip(false));
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
        AudioInstance.Instance.audioSource.PlayOneShot(cardPickSFX);
        Reveal();
    }

    private IEnumerator Flip(bool showFront)
    {
        isFlipping = true;
        canInteract = false;

        float elapsed = 0f;
        float halfDuration = flipDuration / 2f;

        
        while (elapsed < halfDuration)                                 // it rotates the card to 90 degree in y axis
        {
            float angle = Mathf.Lerp(0f, 90f, elapsed / halfDuration);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, 90f, 0f);

        
        front.SetActive(showFront);
        back.SetActive(!showFront);

        if (showFront)
            isRevealed = true;
        else
            isRevealed = false;

        elapsed = 0f;

                                                                          // flipping card back to 0 degree if showFront is false and to 180 degree if showFront is true
        while (elapsed < halfDuration)
        {
            float angle = Mathf.Lerp(90f, 180f, elapsed / halfDuration);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, showFront ? 180f : 0f, 0f);

        isFlipping = false;
        canInteract = true;

        if (showFront)
            OnCardRevealed?.Invoke(this);
    }

}
