using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Interakcja kiedy gracz bedzie w poblizu NPC
public class NPC : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void StartConversation()
    {
        isTalkingWithPlayer = true;
        print("Conversation started");

        DialogSystem.Instance.OpenDialogUI();
        DialogSystem.Instance.dialogText.text = "Hello traveler!";
        DialogSystem.Instance.option1.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Bye";
        DialogSystem.Instance.option1.onClick.AddListener(() =>
        {
            isTalkingWithPlayer = false;
            DialogSystem.Instance.CloseDialogUI();
        });

    }
}
