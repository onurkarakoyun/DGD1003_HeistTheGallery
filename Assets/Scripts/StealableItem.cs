using UnityEngine;
using TMPro;

public class StealableItem : MonoBehaviour
{
    public KeyCode stealKey = KeyCode.E;
    private bool playerInRange = false;
    public TextMeshProUGUI interactionText;
    public string stealMessage = "E - Çal";
    private Collider2D col;
    private SpriteRenderer sr;

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.text = stealMessage;
            interactionText.gameObject.SetActive(false);
        }
        else
        {
            if(interactionText != null) 
            {
                interactionText.text = stealMessage;
                interactionText.gameObject.SetActive(false);
            }
        }
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(stealKey))
        {
            Steal();
        }
    }

    void Steal()
    {
        if (LevelManager.instance != null)
        {
            LevelManager.instance.CollectPainting();
            Debug.Log("Tablo çalındı! İstatistiklere işlendi.");
        }
        else
        {
            Debug.LogWarning("UYARI: Sahnede LevelManager yok! Tablo sayılmadı.");
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactionText != null)
            {
                interactionText.text = stealMessage;
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }
}
