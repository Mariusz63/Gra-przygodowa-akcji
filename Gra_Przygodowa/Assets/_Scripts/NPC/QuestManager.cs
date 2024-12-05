using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

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
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (!isQuestMenuOpen)
            {
                // Otwórz menu questów
                questMenu.SetActive(true);
                questMenu.GetComponentInChildren<Canvas>().sortingOrder = MenuManager.Instance.SetAsFront();

                Cursor.lockState = CursorLockMode.None; // Odblokuj kursor
                Cursor.visible = true;

                SelectionManager.Instance.DisableSelection();
                isQuestMenuOpen = true;
            }
            else
            {
                // Zamknij menu questów
                questMenu.SetActive(false);
                Cursor.visible = false;
                SelectionManager.Instance.EnableSelection();
                isQuestMenuOpen = false;
            }
        }
    }


    public void RefreshQuestList()
    {
        // Destroy everything in this content
        foreach (Transform child in questMenuContent.transform)
        {
            Destroy(child.gameObject);
        }

        // Initialize active completed quests
        foreach (Quest activeQuest in activeQuests)
        {
            InitializeQuestRow(activeQuest, activeQuestPrefab, true, true);
        }

        // Initialize completed quests
        foreach (Quest completedQuest in completedQuests)
        {
            InitializeQuestRow(completedQuest, completedQuestPrefab, false, false);
        }
    }

    private void InitializeQuestRow(Quest quest, GameObject prefab, bool isActive, bool isTracking)
    {
        GameObject questPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        questPrefab.transform.SetParent(questMenuContent.transform, false);

        QuestRow questRow = questPrefab.GetComponent<QuestRow>();
        questRow.questName.text = quest.questName;
        questRow.questGiver.text = quest.questGiver;

        questRow.isActive = isActive;
        questRow.isTracking = isTracking;
        questRow.coinAmount.text = $"{quest.info.coinReward}";

        SetReward(questRow.firstReward, questRow.firstRewardAmount, quest.info.rewardItem1);
        SetReward(questRow.secondReward, questRow.secondRewardAmount, quest.info.rewardItem2);
    }

    private void SetReward(Image rewardImage, Text rewardAmount, string rewardItem)
    {
        if (!string.IsNullOrEmpty(rewardItem))
        {
            rewardImage.sprite = GetSpriteForItem(rewardItem);
            rewardAmount.text = "";  // Mo¿na ustawiæ tekst, jeœli potrzebny
        }
        else
        {
            rewardImage.gameObject.SetActive(false);
            rewardAmount.text = "";
        }
    }

    //Szukamy "sprite" (obrazka) po nazwie przedmiotu
    private Sprite GetSpriteForItem(string item)
    {
        var itemToGet = Resources.Load<GameObject>(item);
        return itemToGet.GetComponent<Image>().sprite;
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
               $"{req2}" + secondItemAmount + "/" + $"{req2Amount}";
            }
            else // jeœli mamy 1 przedmiot
            {
                tRow.requirements.text = $"{req1} " + firstItemAmount + "/" + $"{req1Amount}";
            }

            // Jesli quest ma checkpointy chcemy je dodac do opisu
            if(trackedQuest.info.hasCheckpoints)
            {
                var existingText = tRow.requirements.text;
                tRow.requirements.text = PrintCheckpoint(trackedQuest,existingText);
            }
        }
    }

    //private string PrintCheckpoint(Quest trackedQuest, string existingText)
    //{
    //    string finalText = existingText;
    //    foreach (Checkpoint cp in trackedQuest.info.checkpoints)
    //    {
    //        if (cp.isCompleted)
    //        {
    //            finalText = finalText + "\n" + cp.name + " [Completed]";
    //        }
    //        else
    //        {
    //            finalText = finalText + "\n" + cp.name;
    //        }
    //    }
    //    return finalText;
    //}

    //Obsuga checpoint
    private string PrintCheckpoint(Quest trackedQuest, string existingText)
    {
        string finalText = existingText;

        foreach (Checkpoint cp in trackedQuest.info.checkpoints)
        {
            finalText += $"\n{cp.name}{(cp.isCompleted ? " [Completed]" : " [In progress]")}";
        }
        return finalText;
    }


}
