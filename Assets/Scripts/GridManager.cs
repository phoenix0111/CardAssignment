using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int rows = 4;
    [SerializeField] private int columns = 4;
    [SerializeField] private GridLayoutGroup gridLayout;

    [Header("Card Settings")]
    [SerializeField] private Cards cardPrefab;
    [SerializeField] private List<Sprite> cardSprites;

    private List<Cards> revealedCards = new List<Cards>();

    private void Start()
    {
        SetupGrid(rows, columns);
    }

    #region Grid Setup

    public void SetupGrid(int newRows, int newColumns)
    {
        rows = newRows;
        columns = newColumns;

        ClearGrid();
        CalculateCellSize();
        GenerateGrid();
    }

    private void ClearGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        revealedCards.Clear();
    }

    private void CalculateCellSize()
    {
        RectTransform rect = GetComponent<RectTransform>();

        float totalWidth = rect.rect.width - gridLayout.padding.left - gridLayout.padding.right;
        float spacingX = gridLayout.spacing.x;

        float calculatedWidth = (totalWidth - (spacingX * (columns - 1))) / columns;

        
        float maxSize = 250f;                // It Limit maximum size                                                      

        float finalSize = Mathf.Min(calculatedWidth, maxSize);

        gridLayout.cellSize = new Vector2(finalSize, finalSize);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;
    }






    #endregion

    #region Grid Generation

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

    #endregion

    #region Matching Logic

    private void HandleCardRevealed(Cards card)
    {
        revealedCards.Add(card);

        if (revealedCards.Count >= 2)
        {
            CheckMatch(revealedCards[0], revealedCards[1]);
            revealedCards.RemoveRange(0, 2);
        }
    }

    private void CheckMatch(Cards first, Cards second)
    {
        if (first.CardSprite == second.CardSprite)
        {
            first.SetMatched();
            second.SetMatched();
            Debug.Log("Match!");
        }
        else
        {
            StartCoroutine(HideCards(first, second));
        }
    }

    private IEnumerator HideCards(Cards first, Cards second)
    {
        yield return new WaitForSeconds(0.5f);

        first.Hide();
        second.Hide();
    }

    #endregion

    #region Utility

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

    #endregion
}
