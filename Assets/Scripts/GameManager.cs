using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI movesText;


    [Header("Best Time UI")]
    [SerializeField] private TextMeshProUGUI bestTimeText;
    private string bestTimeKey;
    private float bestTime = 0f;
    public float totalTime = 60f;
    [SerializeField] private float timeRemaining = 0f;

    [Header("Win Panel Text")]
    [SerializeField] private TextMeshProUGUI playerTimeText;
    [SerializeField] private TextMeshProUGUI bestTimeWinText;

    [Header("Game Audio")]
    [SerializeField] private AudioClip winSFX;
    [SerializeField] private AudioClip loseSFX;

    [Header("Combo System")]
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private float comboResetDelay = 3f;
    private int comboCount = 0;
    private Coroutine comboCoroutine;

    private int matchedPairs = 0;
    private int totalPairs;

    private int score = 0;
    private int moves = 0;
 
    private bool isPlaying = true;
    public int scorePerPair = 1;

    void Start()
    {
        bestTimeKey = "BestTime_" + SceneManager.GetActiveScene().name;

        LoadBestTime();
        UpdateBestTimeUI();

        totalPairs = gridManager.GetTotalPairs();
        UpdateUI();
        timeRemaining = totalTime;

        // Ensuring scene starts fresh
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        isPlaying = true;
        score = 0;
        moves = 0;
        matchedPairs = 0;
    }

    void Update()
    {
        if (!isPlaying) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.FloorToInt(timeRemaining);
        }
        else                                                                            // if time is over ---- GAME OVER
        {
            timeRemaining = 0;
            timerText.text = "Time: 0";
            Debug.Log("Time's up!");

            isPlaying = false;
            losePanel.SetActive(true);
            AudioInstance.Instance.audioSource.PlayOneShot(loseSFX);
        }

    }

    
    public void OnPairMatched()
    {
        matchedPairs++;
        comboCount++;

        if (comboCoroutine != null)                                 // if player makes a match before combo timer runs out, it stops the timer and resets it
            StopCoroutine(comboCoroutine);

        comboCoroutine = StartCoroutine(ResetComboAfterDelay());       // it starts combo timer again after each match


        if (matchedPairs >= totalPairs)                          // checking matched pairs
        {
            isPlaying = false;
            CheckForBestTime();
            SaveBestTime();
            UpdateBestTimeUI();
            StartCoroutine(ShowWinPanel());
        }

        UpdateUI();
        UpdateComboUI();
    }


    // When player flips 2 cards
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

        PlayerPrefs.SetFloat(bestTimeKey, bestTime);
        Debug.Log("besttime Key: " + bestTimeKey + " saved with value: " + bestTime);
        PlayerPrefs.Save();
    }

    private void LoadBestTime()
    {
        bestTime = PlayerPrefs.GetFloat(bestTimeKey, 0f);
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

    private IEnumerator ShowWinPanel()
    {
        yield return new WaitForSeconds(1f);

        winPanel.SetActive(true);
        AudioInstance.Instance.audioSource.PlayOneShot(winSFX);
        isPlaying = false;
        playerTimeText.text = "Your Time: " + Mathf.FloorToInt( totalTime - timeRemaining) + " sec";
        bestTimeWinText.text = "Best Time: " + Mathf.FloorToInt(bestTime) + " sec";
    }

    private IEnumerator ResetComboAfterDelay()
    {
        yield return new WaitForSeconds(comboResetDelay);
        comboCount = 0;
        UpdateComboUI();
    }

    public void ResetCombo()
    {
        comboCount = 0;

        if (comboCoroutine != null)
            StopCoroutine(comboCoroutine);

        UpdateComboUI();
    }
    private void UpdateComboUI()
    {
        if (comboText == null) return;

        if (comboCount > 1)
            comboText.text = "Combo x" + comboCount;
        else
            comboText.text = "";
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
