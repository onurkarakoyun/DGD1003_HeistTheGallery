using UnityEngine;
using System.Collections;

public class CharacterSwitcher : MonoBehaviour
{
    public GameObject Character;      
    public GameObject Character2;     

    private GameObject currentCharacter;
    private GameObject inactiveCharacter;

    private bool canSwitch = true;   

    public float firstCharacterTime = 10f; 
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
    }

    void Update()
    {
        // Manuel deðiþtirme sadece kilit açýkken
        if (Input.GetKeyDown(KeyCode.Z) && canSwitch)
        {
            SwitchToCharacter(Character); // Ýlk karakteri seçmek istiyorsan çaðýrýlýr
        }
    }

    // Belirli karakteri aktif yap
    public void SwitchToCharacter(GameObject newCharacter)
    {
        if (!canSwitch) return; // Kilitliyse geçiþ yapma

        canSwitch = false; // Kilitle

        // Pozisyon ve yönü al
        Vector3 lastPosition = currentCharacter.transform.position;
        Vector3 lastScale = currentCharacter.transform.localScale;

        // Eski karakteri kapat
        currentCharacter.SetActive(false);

        // Yeni karakteri aç ve pozisyona taþý
        newCharacter.transform.position = lastPosition;
        newCharacter.transform.localScale = lastScale;
        newCharacter.SetActive(true);

        // Aktif / pasif deðiþtir
        inactiveCharacter = currentCharacter;
        currentCharacter = newCharacter;

        // Eðer aktif olan ilk karakterse süre baþlat
        if (currentCharacter == Character)
        {
            StartCoroutine(FirstCharacterTimer());
        }
        else
        {
            canSwitch = true; // Süresiz karakter ise kilidi aç
        }
    }

    // Ýlk karakterin süre limiti
    IEnumerator FirstCharacterTimer()
    {
        yield return new WaitForSeconds(firstCharacterTime);

        // Süre dolunca otomatik olarak diðer karakteri aç
        SwitchToCharacter(Character2);
        canSwitch = true; // Artýk tekrar deðiþtirilebilir
    }
}

