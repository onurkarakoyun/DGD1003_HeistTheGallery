using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    public GameObject Character;
    public GameObject Character2;

    private GameObject currentCharacter;
    private GameObject inactiveCharacter;

    private bool timerRunning = false;
    private float timer = 0f;
    public float limitTime = 15f;

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
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SwitchCharacter();
        }
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
        if (currentCharacter == Character)
        {
            timer = 0;
            timerRunning = true;
        }
        else
        {
            timerRunning = false;
            timer = 0;
        }





    }









}
