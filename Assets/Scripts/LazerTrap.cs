 using UnityEngine;

public class LaserTrap : MonoBehaviour
{
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
