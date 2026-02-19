using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartLevel1()
    {
        SceneManager.LoadScene("Level 1");     // loads easy mode scene
    }

    public void StartLevel2()
    {
        SceneManager.LoadScene("Level 2");        // loads medium mode scene
    }

    public void StartLevel3()
    {
        SceneManager.LoadScene("Level 3");          // loads hard mode scene
    }

    public void QuitGame()
    {
        Application.Quit();                       // quit the game
    }
}
