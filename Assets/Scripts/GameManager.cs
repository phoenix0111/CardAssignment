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


    [Header("Best Time UI")]
    [SerializeField] private TextMeshProUGUI bestTimeText;
    private const string BEST_TIME_KEY = "BestTime";
    private float bestTime = 0f;
    public float totalTime = 60f;
    public float timeRemaining = 0f;


    private int matchedPairs = 0;
    private int totalPairs;

    private int score = 0;
    private int moves = 0;

    
    private bool isPlaying = true;
    public int scorePerPair = 1;

    void Start()
    {
        LoadBestTime();
        UpdateBestTimeUI();

        totalPairs = gridManager.GetTotalPairs();
        UpdateUI();
        timeRemaining = totalTime;
    }

    void Update()
    {
        if (!isPlaying) return;

        timeRemaining -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.FloorToInt(timeRemaining);

    }

    // Called when a pair matches
    public void OnPairMatched()
    {
        matchedPairs++;
        score += scorePerPair;

        if (matchedPairs >= totalPairs)
        {
            isPlaying = false;
            CheckForBestTime();
            SaveBestTime();
            UpdateBestTimeUI();
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
        scoreText.text = "Matches: " + score;
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

    private void CheckForBestTime()
    {
       float newTime;

        newTime = totalTime - timeRemaining;

        if(bestTime>newTime) 
        {
            bestTime = newTime;
        }
        else if (bestTime == 0f)
        {
            bestTime = newTime;
        }

        Debug.Log("Best time " + bestTime);
    }

    private void SaveBestTime()
    {

        PlayerPrefs.SetFloat(BEST_TIME_KEY, bestTime);
        PlayerPrefs.Save();
    }

    private void LoadBestTime()
    {
        bestTime = PlayerPrefs.GetFloat(BEST_TIME_KEY, 0f);
    }

    private void UpdateBestTimeUI()
    {
        if (bestTime <= 0f)
        {
            bestTimeText.text = "Best Time: --";
            return;
        }


        int seconds = Mathf.FloorToInt(bestTime);
        

        bestTimeText.text = "Best Time: " +"\n" + seconds + "sec";
    }

}
