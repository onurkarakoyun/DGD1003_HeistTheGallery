using UnityEngine;
using TMPro;

public class TutorialInfoTrigger : MonoBehaviour
{
    [TextArea]
    public string message = "Bilgi Mesajı Buraya";
    
    public TextMeshProUGUI tutorialTextUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            tutorialTextUI.text = message;
            tutorialTextUI.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            tutorialTextUI.gameObject.SetActive(false);
        }
    }
}
