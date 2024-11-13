using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton
public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; set; }

    // Lista aktywnych zadañ
    public List<Quest> activeQuests;
    //Lista skonczonych zadañ
    public List<Quest> completedQuests;

    [Header("QuestMenu")]
    public GameObject questMenu;
    public bool isQuestMenuOpen;

    public GameObject activeQuestPrefab;
    public GameObject completedQuestPrefab;
    public GameObject questMenuContent;

    [Header("QuestTracker")]
    public GameObject questTrackerContent;
    public GameObject trackerRowPref;

    // Lista przechowywujaca wszystkie zadania ktore sledzimy
    public List<Quest> trackedQuests;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Update()
    {
        // Obs³uga Menu Questów
        if(Input.GetKeyUp(KeyCode.Q) && !isQuestMenuOpen)
        {
            questMenu.SetActive(true);
            questMenu.GetComponentInChildren<Canvas>().sortingOrder = MenuManager.Instance.SetAsFront();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
            isQuestMenuOpen = true;
        }else if(Input.GetKeyUp(KeyCode.Q) && isQuestMenuOpen)
        {
            questMenu.SetActive(false);

            //TO DO: mozliwe ze do usuniecia
            //if (!InventorySystem.Instance.isOpen)
            //{
            //    Cursor.lockState = CursorLockMode.Locked; 
            //    Cursor.visible = false;

            //    SelectionManager.Instance.EnableSelection();
            //    SelectionManager.Instance.GetComponent<SelectionManager>().enabled=true;
            //}    
        }
    }

    public void RefreshQuestList()
    {
        // destroy everything in this content
        foreach(Transform child in questMenuContent.transform)
        {
            Destroy(child.gameObject);
        }

        // active quests
        foreach (Quest activeQuest in activeQuests)
        {
            GameObject questPrefab = Instantiate(activeQuestPrefab, Vector3.zero, Quaternion.identity);
            questPrefab.transform.SetParent(questMenuContent.transform, false);

            QuestRow questRow = questPrefab.GetComponent<QuestRow>();
            questRow.questName.text = activeQuest.questName;
            questRow.questGiver.text = activeQuest.questGiver;

            questRow.isActive = true;
            questRow.isTracking = true;

            questRow.coinAmount.text = $"{activeQuest.info.coinReward}";

            //questRow.firstReward.sprite = "";
            questRow.firstRewardAmount.text = "";

            //questRow.secondReward.sprite = "";
            questRow.secondRewardAmount.text = "";
        }

        // completed quests
        foreach (Quest completedQuest in completedQuests)
        {
            GameObject questPrefab = Instantiate(completedQuestPrefab, Vector3.zero, Quaternion.identity);
            questPrefab.transform.SetParent(questMenuContent.transform, false);

            QuestRow questRow = questPrefab.GetComponent<QuestRow>();
            questRow.questName.text = completedQuest.questName;
            questRow.questGiver.text = completedQuest.questGiver;

            questRow.isActive = false;
            questRow.isTracking = false;

            questRow.coinAmount.text = $"{completedQuest.info.coinReward}";

            //questRow.firstReward.sprite = "";
            questRow.firstRewardAmount.text = "";

            //questRow.secondReward.sprite = "";
            questRow.secondRewardAmount.text = "";
        }
    }

    //Kiedy zaakceptujemy zadanie dodajemy je listy zadañ oraz do œledzenia
    public void AddActiveQuest(Quest quest)
    {
        activeQuests.Add(quest);
        TrackQuest(quest);
        RefreshQuestList();
    }

    public void MarkQuestCompleted(Quest quest)
    {
        //Usuniecie z aktywnych zadan
        activeQuests.Remove(quest);
        //Dodanie do listy ukoñczonych zadan
        completedQuests.Add(quest);
        //Skonczenie œledzenia zadania
        UnTrackQuest(quest);
        RefreshQuestList();
    }

    public void TrackQuest(Quest thisQuest)
    {
        trackedQuests.Add(thisQuest);
        RefreshTrackerList();
    }

    public void UnTrackQuest(Quest thisQuest)
    {
        trackedQuests.Remove(thisQuest);
        RefreshTrackerList();
    }

    public void RefreshTrackerList()
    {
        // Destroying the previous list
        foreach (Transform child in questTrackerContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Quest trackedQuest in trackedQuests)
        {
            GameObject trackerPrefab = Instantiate(trackerRowPref, Vector3.zero, Quaternion.identity);
            trackerPrefab.transform.SetParent(questTrackerContent.transform, false);

            TrackerRow tRow = trackerPrefab.GetComponent<TrackerRow>();

            tRow.questName.text = trackedQuest.questName;
            tRow.description.text = trackedQuest.questDescription;

            //Potrzebne
            var req1 = trackedQuest.info.firstRequirmentItem;
            var req1Amount = trackedQuest.info.firstRequirementAmount;
            var req2 = trackedQuest.info.secondRequirmentItem;
            var req2Amount = trackedQuest.info.secondRequirementAmount;

            //Posiadane - sprawdzane w ekwipunku
            int firstItemAmount = InventorySystem.Instance.CheckItemAmount(req1);
            int secondItemAmount = InventorySystem.Instance.CheckItemAmount(req2);

            if (trackedQuest.info.secondRequirmentItem != "") // Jeœli mamy 2 przedmioty
            {
                tRow.requirements.text = $"{req1} " + firstItemAmount + "/" + $"{req1Amount}\n" +
               $"{req2}" + secondItemAmount + "/" + $"{req2Amount}\n";
            }
            else // jeœli mamy 1 przedmiot
            {
                tRow.requirements.text = $"{req1} " + firstItemAmount + "/" + $"{req1Amount}\n";
            }
        }
    }
}
