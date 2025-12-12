using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.GetComponent<PlayerController>() != null)
        {
            if (LevelManager.instance != null)
            {
                LevelManager.instance.LevelComplete();
            }
        }
    }
}
