using UnityEngine;
using UnityEngine.UI;

public class CharacterSwitcher : MonoBehaviour
{
    [Header("Karakterler")]
    public GameObject Character;
    public GameObject Character2;
    public CameraFollow cameraScript;

    [Header("Cooldown UI")]
    public GameObject cooldownCanvas;
    public Image cooldownFillImage;
    public Vector3 uiOffset = new Vector3(0, 1.5f, 0);
    public float switchCooldown = 5f;

    private GameObject currentCharacter;
    private GameObject inactiveCharacter;

    private bool isCooldown = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        if (Character.activeSelf)
        {
            currentCharacter = Character;
            inactiveCharacter = Character2;
        }
        else
        {
            currentCharacter = Character2;
            inactiveCharacter = Character;
        }

        if(cameraScript != null)
            cameraScript.ChangeTarget(currentCharacter.transform);

        if (cooldownCanvas != null) cooldownCanvas.SetActive(false);
    }

    [System.Obsolete]
    void Update()
    {

        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownFillImage != null)
            {
                cooldownFillImage.fillAmount = cooldownTimer / switchCooldown;
            }

            if (cooldownCanvas != null)
            {
                cooldownCanvas.transform.position = currentCharacter.transform.position + uiOffset;
            }

            if (cooldownTimer <= 0)
            {
                isCooldown = false;
                if (cooldownCanvas != null) cooldownCanvas.SetActive(false); // Barı gizle
            }
        }
        if (Input.GetKeyDown(KeyCode.Z) && !isCooldown)
        {
            if (CheckIfChased())
            {
                Debug.Log("Polis seni kovalarken kılık değiştiremezsin!");
                return;
            }

            SwitchCharacter();
            StartCooldown();
        }
    }

    void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = switchCooldown;

        if (cooldownCanvas != null)
        {
            cooldownCanvas.SetActive(true);
            cooldownCanvas.transform.position = currentCharacter.transform.position + uiOffset;
        }
    }

    [System.Obsolete]
    bool CheckIfChased()
    {
        GuardAI[] guards = FindObjectsOfType<GuardAI>();
        foreach (GuardAI guard in guards)
        {
            if (guard.isChasing) return true;
        }
        return false;
    }

    void SwitchCharacter()
    {
        Vector3 lastPosition = currentCharacter.transform.position;
        Vector3 lastScale = currentCharacter.transform.localScale;

        currentCharacter.SetActive(false);

        inactiveCharacter.transform.position = lastPosition;
        inactiveCharacter.transform.localScale = lastScale;

        inactiveCharacter.SetActive(true);

        GameObject temp = currentCharacter;
        currentCharacter = inactiveCharacter;
        inactiveCharacter = temp;

        if (currentCharacter.name.Contains("Thief"))
        {
            currentCharacter.tag = "Player";
        }
        else
        {
            currentCharacter.tag = "NormalChar";
        }

        if (cameraScript != null)
        {
            cameraScript.ChangeTarget(currentCharacter.transform);
        }
    }
}