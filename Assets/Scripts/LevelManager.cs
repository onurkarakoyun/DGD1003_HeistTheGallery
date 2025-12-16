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

    [Header("Bitiş Sesleri")]
    public AudioClip winSound;
    public AudioClip loseSound;

    [Header("Tutorial Ayarları")]
    public bool isTutorialLevel = false;
    public GameObject tutorialCompletePanel;
    public GameObject missionListParent;
    private Vector3 currentCheckpointPos;

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
        if (isTutorialLevel)
        {
            if (missionListParent != null)
                missionListParent.SetActive(false);
            if(timerText != null) timerText.gameObject.SetActive(false);
        }
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        gameHUD.SetActive(true);
        Time.timeScale = 1f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) currentCheckpointPos = player.transform.position;
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
        AudioListener.pause = true;
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayWinLoseSound(winSound);
        }
        gameHUD.SetActive(false);
        if (isTutorialLevel)
        {
            if (tutorialCompletePanel != null)
                tutorialCompletePanel.SetActive(true);
            else
                Debug.LogError("Tutorial Panel atanmamış!");
        }
        else
        {
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
    }

    [System.Obsolete]
    public void GameOver()
    {
        if (isGameOver) return;
        if (isTutorialLevel)
        {
            RespawnPlayer();
            return;
        }
        isGameOver = true;
        AudioListener.pause = true;
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayWinLoseSound(loseSound);
        }
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
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadMainMenu()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void NextLevel()
    {
        AudioListener.pause = false;
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
    public void SetCheckpoint(Vector3 newPos)
    {
        currentCheckpointPos = newPos;
        Debug.Log("Checkpoint Kaydedildi!");
    }

    [System.Obsolete]
    void RespawnPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null) player = FindObjectOfType<PlayerController>().gameObject;

        if (player != null)
        {
            player.transform.position = currentCheckpointPos;
        }
        GuardAI[] guards = FindObjectsOfType<GuardAI>();
        foreach (GuardAI guard in guards)
        {
            guard.isChasing = false;
        }
        AudioListener.pause = false; 
        
        Debug.Log("Tutorial: Checkpoint'e dönüldü.");
    }
}