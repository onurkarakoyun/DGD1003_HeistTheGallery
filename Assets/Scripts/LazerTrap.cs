using UnityEngine;
using System.Collections;

public class LaserTrap : MonoBehaviour
{
    [Header("Zamanlama Ayarları")]
    public float activeTime = 2f;
    public float inactiveTime = 2f;
    public float startDelay = 0f;

    [Header("Bileşenler")]
    private SpriteRenderer spriteRenderer;
    private Collider2D laserCollider;
    private AudioSource audioSource;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        laserCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(LaserRoutine());
    }

    IEnumerator LaserRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            SetLaserState(true);
            yield return new WaitForSeconds(activeTime);
            SetLaserState(false);
            yield return new WaitForSeconds(inactiveTime);
        }
    }

    void SetLaserState(bool isActive)
    {
        if (spriteRenderer != null) 
            spriteRenderer.enabled = isActive;

        if (laserCollider != null) 
            laserCollider.enabled = isActive;

        // Sesi aç/kapat
        if (audioSource != null)
        {
            if (isActive)
            {
                if (!audioSource.isPlaying) audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }
    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Oyuncu lazere yakalandı!");
            if (LevelManager.instance != null)
            {
                LevelManager.instance.GameOver();
            }
        }
    }
}