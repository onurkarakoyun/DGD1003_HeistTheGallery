using UnityEngine;

public class OneWayWall : MonoBehaviour
{
    public GameObject invisibleWall;

    void Start()
    {
        if(invisibleWall != null) invisibleWall.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            if (invisibleWall != null)
            {
                invisibleWall.SetActive(true);
            }
            Destroy(gameObject); 
        }
    }
}