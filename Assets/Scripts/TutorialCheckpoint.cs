using UnityEngine;

public class TutorialCheckpoint : MonoBehaviour
{
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated && collision.GetComponent<PlayerController>() != null)
        {
            isActivated = true;
            if (LevelManager.instance != null)
            {
                LevelManager.instance.SetCheckpoint(transform.position);
            }
        }
    }
}
