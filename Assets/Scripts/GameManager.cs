using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject winPanel;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI movesText;

    private int matchedPairs = 0;
    private int totalPairs;

    private int score = 0;
    private int moves = 0;

    public float totalTime = 60f;
    private bool isPlaying = true;
    public int scorePerPair = 1;

    private void Start()
    {
        totalPairs = gridManager.GetTotalPairs();
        UpdateUI();
    }

    private void Update()
    {
        if (!isPlaying) return;

        totalTime -= Time.deltaTime;

        timerText.text = "Time: " + Mathf.FloorToInt(totalTime);

    }

    // Called when a pair matches
    public void OnPairMatched()
    {
        matchedPairs++;
        score += scorePerPair;

        if (matchedPairs >= totalPairs)
        {
            isPlaying = false;
            winPanel.SetActive(true);
        }

        UpdateUI();
    }

    // Called when player flips two cards
    public void OnMoveMade()
    {
        moves++;
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        movesText.text = "Moves: " + moves;
    }

    //public void RestartGame()
    //{
    //    matchedPairs = 0;
    //    score = 0;
    //    moves = 0;
    //    timer = 0f;
    //    isPlaying = true;

    //    winPanel.SetActive(false);
   
   // SceneManager.LoadScene("MainMenu");
}
