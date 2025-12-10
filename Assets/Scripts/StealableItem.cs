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
        if (interactionText == null) interactionText = GetComponentInChildren<TextMeshProUGUI>();
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
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
        if (interactionText != null) interactionText.gameObject.SetActive(false);
        if (col != null) col.enabled = false;
        if (sr != null) sr.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.GetComponent<PlayerController>() != null)
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
        if (other.CompareTag("Player") || other.GetComponent<PlayerController>() != null)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.GetComponent<PlayerController>() != null)
        {
            playerInRange = false;
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
        }
    }
}
