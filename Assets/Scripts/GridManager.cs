using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIShake shakeEffect;

    [Header("Grid Settings")]
    [SerializeField] private int rows = 4;
    [SerializeField] private int columns = 4;
    [SerializeField] private GridLayoutGroup gridLayout;

    [Header("Card Settings")]
    [SerializeField] private Cards cardPrefab;
    [SerializeField] private List<Sprite> cardSprites;

    private List<Cards> revealedCards = new List<Cards>();
    private bool isChecking = false;



    private void Start()
    {
        SetupGrid(rows, columns);
    }

    // Grid Setup ---------------------

    public void SetupGrid(int newRows, int newColumns)
    {
        rows = newRows;
        columns = newColumns;

        CalculateCellSize();
        GenerateGrid();
    }

   

    private void CalculateCellSize()
    {
        RectTransform rect = GetComponent<RectTransform>();

        float totalWidth = rect.rect.width - gridLayout.padding.left - gridLayout.padding.right;
        float spacingX = gridLayout.spacing.x;

        float calculatedWidth = (totalWidth - (spacingX * (columns - 1))) / columns;


        float maxSize = 150f;                // It Limits maximum size of the card                                                  

        float finalSize = Mathf.Min(calculatedWidth, maxSize);

        gridLayout.cellSize = new Vector2(finalSize, finalSize);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;
    }

    public int GetTotalPairs()
    {
        return (rows * columns) / 2;
    }




    // Grid Generation ---------------------

    private void GenerateGrid()
    {
        int totalCards = rows * columns;

        if (totalCards % 2 != 0)
        {
            Debug.LogError("Total cards must be even!");
            return;
        }

        int pairs = totalCards / 2;

        if (pairs > cardSprites.Count)
        {
            Debug.LogError("Not enough sprites for selected layout!");
            return;
        }

        List<Sprite> shuffledSprites = new List<Sprite>(cardSprites);
        Shuffle(shuffledSprites);

        List<Sprite> selectedSprites = shuffledSprites.GetRange(0, pairs);


        List<Sprite> finalSprites = new List<Sprite>();

        foreach (Sprite sprite in selectedSprites)
        {
            finalSprites.Add(sprite);
            finalSprites.Add(sprite);
        }


        Shuffle(finalSprites);


        for (int i = 0; i < totalCards; i++)
        {
            Cards card = Instantiate(cardPrefab, transform);
            card.Initialize(i, finalSprites[i]);
            card.OnCardRevealed += HandleCardRevealed;
        }
    }

    // Matchig Logic ---------------------

    private void HandleCardRevealed(Cards card)
    {
        if (isChecking) return;

        revealedCards.Add(card);

        if (revealedCards.Count >= 2)
        {
            isChecking = true;
                                
            gameManager.OnMoveMade();                       // Count move when 2 cards flipped

            CheckMatch(revealedCards[0], revealedCards[1]);
        }
    }

    private void CheckMatch(Cards first, Cards second)
    {
        if (first.CardSprite == second.CardSprite)
        {
            first.SetMatched();
            second.SetMatched();

            gameManager.OnPairMatched();

            revealedCards.Clear();
            isChecking = false;

            SetAllCardsInteractable(true);

            StartCoroutine(DeactivateCards(first.gameObject, second.gameObject));


        }
        else
        {
            SetAllCardsInteractable(false);
            StartCoroutine(HideCards(first, second));
            shakeEffect.Shake(0.2f, 2f);             // Shake effect on cards mismatch

        }
    }

    private IEnumerator HideCards(Cards first, Cards second)
    {
        yield return new WaitForSeconds(0.5f);

        first.Hide();
        second.Hide();

        revealedCards.Clear();
        isChecking = false;
        SetAllCardsInteractable(true);

    }

    private void SetAllCardsInteractable(bool value)
    {
        foreach (Transform child in transform)
        {
            Cards card = child.GetComponent<Cards>();
            if (card != null)
                card.SetInteractable(value);
        }
    }

    private IEnumerator DeactivateCards(GameObject firstCard, GameObject secondCard)
    {
        yield return new WaitForSeconds(0.5f);

        firstCard.GetComponent<Button>().interactable = false;
        secondCard.GetComponent<Button>().interactable = false;
        firstCard.GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 0f);              // fadng card effect for matched cards
        secondCard.GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 0f);                
    }





    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }




}
