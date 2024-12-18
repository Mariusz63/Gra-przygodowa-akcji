using UnityEngine;

public class TimeManager: MonoBehaviour
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

    public static object Season { get; internal set; }
    internal object currentSeason;

}