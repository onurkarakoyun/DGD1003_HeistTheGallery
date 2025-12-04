using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // Coroutine için gerekli

public class HidingSpot : MonoBehaviour
{
    [Header("Gerekli Bileşenler")]
    public SpriteRenderer columnRenderer; 
    public TextMeshProUGUI interactionText; 

    [Header("Mesajlar")]
    public string hideMessage = "E - Saklan";
    public string exitMessage = "E - Çık";

    private bool isPlayerInRange = false; 
    private bool isPlayerHiding = false;  
    
    // Titremeyi önlemek için kısa bir bekleme süresi kilidi
    private bool canInteract = true; 

    private PlayerController playerScript; 
    private SpriteRenderer playerRenderer;
    private Rigidbody2D playerRb;
    
    private float originalGravity;
    private int originalSortingOrder;

    void Start()
    {
        if (columnRenderer == null) columnRenderer = GetComponent<SpriteRenderer>();
        if (interactionText == null) interactionText = GetComponentInChildren<TextMeshProUGUI>();
        if(interactionText != null) interactionText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Eğer oyuncu menzildeyse ve etkileşim kilidi açık ise (canInteract)
        if (isPlayerInRange && canInteract && Input.GetKeyDown(KeyCode.E))
        {
            if (isPlayerHiding)
                ExitHiding();
            else
                EnterHiding();
        }

        // EKSTRA GÜVENLİK: Saklanırken karakter kaymasın diye her kare hızını sıfırla
        if (isPlayerHiding && playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
        }
    }

    void EnterHiding()
    {
        if (playerScript == null) return;

        isPlayerHiding = true;
        
        // 1. Fizik Ayarları (FreezeAll KULLANMIYORUZ, sadece hız kesiyoruz)
        playerRb.linearVelocity = Vector2.zero;
        playerRb.gravityScale = 0f;
        // Sadece dönmeyi engelle, pozisyonu dondurma (bu titreme yapar)
        playerRb.constraints = RigidbodyConstraints2D.FreezeRotation; 

        // Karakteri kolonun tam merkezine ışınla (Kenarda köşede kalırsa çıkmış sayabilir)
        playerScript.transform.position = new Vector3(transform.position.x, playerScript.transform.position.y, playerScript.transform.position.z);

        // 2. Etiket ve Değişkenler
        playerScript.isHidden = true;
        playerScript.gameObject.tag = "Untagged"; 

        // 3. Görsel Ayarlar
        originalSortingOrder = playerRenderer.sortingOrder;
        
        if (columnRenderer != null)
        {
            // Karakteri arkaya at
            playerRenderer.sortingOrder = columnRenderer.sortingOrder - 1; 
            // Kolonu şeffaflaştır
            Color colColor = columnRenderer.color;
            colColor.a = 0.5f; 
            columnRenderer.color = colColor;
        }

        UpdateUIText(exitMessage);
        
        // İşlem yapıldıktan sonra 0.5 saniye tuşu kilitle (Titremeyi engeller)
        StartCoroutine(InteractionCooldown());
    }

    void ExitHiding()
    {
        isPlayerHiding = false;

        // 1. Fizik Geri Yükleme
        playerScript.isHidden = false;
        playerScript.gameObject.tag = "Player";
        playerRb.gravityScale = originalGravity;
        
        // 2. Görsel Geri Yükleme
        playerRenderer.sortingOrder = originalSortingOrder; 

        if (columnRenderer != null)
        {
            Color colColor = columnRenderer.color;
            colColor.a = 1f; 
            columnRenderer.color = colColor;
        }

        UpdateUIText(hideMessage);

        // İşlem yapıldıktan sonra 0.5 saniye tuşu kilitle
        StartCoroutine(InteractionCooldown());
    }

    // Kısa süreliğine E tuşunu devre dışı bırakır
    IEnumerator InteractionCooldown()
    {
        canInteract = false;
        yield return new WaitForSeconds(0.2f); // 0.2 saniye bekle
        canInteract = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerScript = collision.GetComponent<PlayerController>();
            playerRenderer = collision.GetComponent<SpriteRenderer>();
            playerRb = collision.GetComponent<Rigidbody2D>();
            
            if(playerRb != null) originalGravity = playerRb.gravityScale;

            UpdateUIText(hideMessage);
            if(interactionText != null) interactionText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Eğer saklanıyorsak, fizik motoru "çıktı" dese bile inanma.
        // Çünkü saklanırken kımıldayamayız.
        if (isPlayerHiding) return; 

        if (collision.GetComponent<PlayerController>() != null)
        {
            isPlayerInRange = false;
            if(interactionText != null) interactionText.gameObject.SetActive(false);
        }
    }

    void UpdateUIText(string message)
    {
        if (interactionText != null) interactionText.text = message;
    }
}