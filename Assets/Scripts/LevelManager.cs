using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";
    public static LevelManager instance;

    [Header("Level Hedefleri")]
    public float targetTime = 60f;

    [Header("UI Panelleri")]
    public GameObject gameHUD;
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Win Panel Görselleri")]
    public GameObject star1Image;
    public GameObject star2Image;
    public GameObject star3Image;

    [Header("İstatistik UI (Tik/Çarpı)")]
    public Image[] checkmarks;
    public Image[] crosses;

    [Header("Lose Panel İstatistikleri")]
    public Image[] loseCheckmarks;
    public Image[] loseCrosses;
    

    [Header("HUD Textleri")]
    public TextMeshProUGUI timerText;

    [Header("HUD Görev Listesi")]
    public TextMeshProUGUI missionCollectionText;
    public TextMeshProUGUI missionTimeText;
    public TextMeshProUGUI missionStealthText;

    private float currentTime;
    private int totalPaintings;
    private int collectedPaintings;
    private bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }

    [System.Obsolete]
    void Start()
    {
        totalPaintings = FindObjectsOfType<StealableItem>().Length;
        
        UpdateHUD();
        UpdateMissionTexts();
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        gameHUD.SetActive(true);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!isGameOver)
        {
            currentTime += Time.deltaTime;
            UpdateHUD();
            if (currentTime > targetTime && missionTimeText != null)
            {
                missionTimeText.color = Color.red;
            }
        }
    }

    void UpdateHUD()
    {
        if(timerText != null) 
            timerText.text = "Time : " + currentTime.ToString("F1");
    }
    void UpdateMissionTexts()
    {
        if (missionCollectionText != null)
        {
            missionCollectionText.text = $"- Steal all the pictures ({collectedPaintings}/{totalPaintings})";
            
            // Hepsi toplandıysa Yeşil yap
            if (collectedPaintings >= totalPaintings)
                missionCollectionText.color = Color.green;
            else
                missionCollectionText.color = Color.white;
        }
        if (missionTimeText != null)
        {
            missionTimeText.text = $"- Finish in Under {targetTime} Seconds ";
            missionTimeText.color = Color.white;
        }


        if (missionStealthText != null)
        {
            missionStealthText.text = "- Don't get caught.";
        }
    }

    public void CollectPainting()
    {
        collectedPaintings++;
        UpdateMissionTexts();
    }

    public void LevelComplete()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;

        gameHUD.SetActive(false);
        winPanel.SetActive(true);

        int stars = 1;
        bool collectedAll = (collectedPaintings >= totalPaintings);
        bool timeSuccess = (currentTime <= targetTime);

        if (collectedAll) stars++;
        if (timeSuccess) stars++;

        star1Image.SetActive(stars == 1);
        star2Image.SetActive(stars == 2);
        star3Image.SetActive(stars == 3);

        SetStatus(0, true);
        SetStatus(1, collectedAll);
        SetStatus(2, timeSuccess);
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;
        bool collectedAll = (collectedPaintings >= totalPaintings);
        bool timeSuccess = (currentTime <= targetTime);
        SetLoseStatus(0, false);
        SetLoseStatus(1, collectedAll);
        SetLoseStatus(2, timeSuccess);

        gameHUD.SetActive(false);
        losePanel.SetActive(true);
    }
    void SetLoseStatus(int index, bool success)
    {
        if (loseCheckmarks.Length > index) 
            loseCheckmarks[index].gameObject.SetActive(success);
        
        if (loseCrosses.Length > index) 
            loseCrosses[index].gameObject.SetActive(!success);
    }

    void SetStatus(int index, bool success)
    {
        if (checkmarks.Length > index) checkmarks[index].gameObject.SetActive(success);
        if (crosses.Length > index) crosses[index].gameObject.SetActive(!success);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Oyun bitti! Ana menüye dönülüyor.");
            LoadMainMenu();
        }
    }
}