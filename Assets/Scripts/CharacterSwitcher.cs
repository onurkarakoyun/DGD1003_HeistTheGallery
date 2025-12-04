using System.Collections;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    public GameObject Character;
    public GameObject Character2;
    public CameraFollow cameraScript;

    private GameObject currentCharacter;
    private GameObject inactiveCharacter;

    private bool canSwitch = true;
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
        // YENİ: Oyun başlar başlamaz kamera doğru kişiye baksın
        if(cameraScript != null)
        {
            cameraScript.ChangeTarget(currentCharacter.transform);
        }
    }

    [System.Obsolete]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && canSwitch)
        {
            // YENİ: Önce polis kontrolü yap!
            if (CheckIfChased())
            {
                Debug.Log("Polis seni kovalarken kılık değiştiremezsin!");
                return; // Fonksiyonu burada durdur, aşağıya inip karakter değiştirme
            }

            SwitchCharacter();
            StartCoroutine(SwitchCooldown());
        }
    }

    [System.Obsolete]
    bool CheckIfChased()
    {
        // Sahnedeki "GuardAI" scriptine sahip tüm objeleri bul
        GuardAI[] guards = FindObjectsOfType<GuardAI>();

        // Tek tek hepsine sor: Beni kovalıyor musun?
        foreach (GuardAI guard in guards)
        {
            if (guard.isChasing)
            {
                return true;
            }
        }
        return false;
    }

    void SwitchCharacter()
    {
        Vector3 lastPosition = currentCharacter.transform.position;
        Vector3 lastScale = currentCharacter.transform.localScale;

        currentCharacter.SetActive(false);

        inactiveCharacter.transform.position = lastPosition;
        inactiveCharacter.transform.localScale= lastScale;

        inactiveCharacter.SetActive(true);

        GameObject temp = currentCharacter;
        currentCharacter = inactiveCharacter;
        inactiveCharacter = temp;
        if (cameraScript != null)
        {
            cameraScript.ChangeTarget(currentCharacter.transform);
        }

    }

    IEnumerator SwitchCooldown()
    {

        canSwitch = false;
        yield return new WaitForSeconds(5f);
        canSwitch = true;
    }







}
