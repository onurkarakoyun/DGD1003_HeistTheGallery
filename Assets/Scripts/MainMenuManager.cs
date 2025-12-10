using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;
    void Start()
    {
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
        if(settingsPanel != null) settingsPanel.SetActive(false);
    }
    public void OnPlayPressed()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }
    public void OnSettingsPressed()
    {
        mainMenuPanel.SetActive(false);
        if(settingsPanel != null) settingsPanel.SetActive(true);
    }
    public void OnBackToMainPressed()
    {
        levelSelectPanel.SetActive(false);
        if(settingsPanel != null) settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void OnQuitPressed()
    {
        Debug.Log("Oyundan çıkıldı!"); 
        Application.Quit();
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
