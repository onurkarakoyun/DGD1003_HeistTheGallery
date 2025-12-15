using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject pausePanel;
    public string mainMenuSceneName = "MainMenu";
    public static bool isPaused = false;
    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        pausePanel.SetActive(true); 
        Time.timeScale = 0f;
        isPaused = true;
        AudioListener.pause = true;
    }
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        AudioListener.pause = false;
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        isPaused = false;
        
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
