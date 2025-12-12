using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Müzik Dosyaları")]
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    private AudioSource musicSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        musicSource = GetComponent<AudioSource>();
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0.5f;
    }

    void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    void PlayMusicForScene(string sceneName)
    {
        AudioClip targetClip = null;
        if (sceneName == "MainMenu") 
        {
            targetClip = menuMusic;
        }
        else
        {
            targetClip = gameMusic;
        }
        if (musicSource.clip == targetClip) return;
        musicSource.Stop();
        musicSource.clip = targetClip;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        musicSource.PlayOneShot(clip);
    }
}
