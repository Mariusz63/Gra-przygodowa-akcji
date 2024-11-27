using System.Collections.Generic;
using UnityEngine;

//ScriptableObject
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QuestInfo", order = 1)]
public class QuestInfo : ScriptableObject
{
    [TextArea(5, 10)]
    public List<string> initialDialog;
    public List<AudioClip> initialAudioClip;

    [Header("Options")]
    [TextArea(5, 10)]
    public string acceptOption;
    public AudioClip acceptOptionAudioClip;

    [TextArea(5, 10)]
    public string acceptAnswer;
    public AudioClip acceptAnswerAudioClip;

    [TextArea(5, 10)]
    public string declineOption;
    public AudioClip declineOptionAudioClip;

    [TextArea(5, 10)]
    public string declineAnswer;
    public AudioClip declineAnswerrAudioClip;

    [TextArea(5, 10)]
    public string comebackAfterDecline;
    public AudioClip comebackAfterDeclineAudioClip;

    [TextArea(5, 10)]
    public string comebackInProgress;
    public AudioClip comebackInProgressAudioClip;

    [TextArea(5, 10)]
    public string comebackCompleted;
    public AudioClip comebackCompletedAudioClip;

    [TextArea(5, 10)]
    public string finalWords;
    public AudioClip finalWordsAudioClip;

    [Header("Rewards")]
    public int coinReward;
    public string rewardItem1;
    public string rewardItem2;

    [Header("Requirements")]
    public string firstRequirmentItem;
    public int firstRequirementAmount;

    public string secondRequirmentItem;
    public int secondRequirementAmount;

    [Header("Checkpoint")]
    public bool hasCheckpoints;
    public List<Checkpoint> checkpoints;
}