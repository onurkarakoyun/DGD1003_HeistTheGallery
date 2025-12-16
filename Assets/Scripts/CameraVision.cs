using UnityEngine;

public class CameraVision : MonoBehaviour
{
    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Yakalandın!");
            if (LevelManager.instance != null)
            {
                LevelManager.instance.GameOver();
            }
        }
    }
}
