using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region || ---------------- Singleton --------------- ||
    public static TimeManager Instance { get; set; }

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
    #endregion

    public TextMeshProUGUI dayUI;
    public int dayInGame = 1;

    private void Start()
    {
        dayUI.text = $"Day: {dayInGame}";
    }

    public void TriggerNextDay()
    {
        dayInGame++;
        dayUI.text = $"Day: {dayInGame}";
    }

}