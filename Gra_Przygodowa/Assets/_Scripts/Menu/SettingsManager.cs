using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    public Button saveButton;

    [Header("Volume UI")]
    public Slider masterSlider;
    public GameObject masterVolume;

    public Slider musicSlider;
    public GameObject musicVolume;

    public Slider effectsSlider;
    public GameObject effectsVolume;

    [Header("Keybinding UI Buttons")]
    [SerializeField] private Button jump;
    [SerializeField] private Button walkFront;
    [SerializeField] private Button walkLeft;
    [SerializeField] private Button walkRight;
    [SerializeField] private Button walkBack;
    [SerializeField] private Button interaction;
    [SerializeField] private Button hit;
    [SerializeField] private Button getItem;
    [SerializeField] private Button option;

    private string currentKey;

    private Dictionary<string, KeyCode> keybinds;

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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        jump.onClick.AddListener(() => StartRebinding("Jump"));
        walkFront.onClick.AddListener(() => StartRebinding("WalkFront"));
        walkLeft.onClick.AddListener(() => StartRebinding("WalkLeft"));
        walkRight.onClick.AddListener(() => StartRebinding("WalkRight"));
        walkBack.onClick.AddListener(() => StartRebinding("WalkBack"));
        interaction.onClick.AddListener(() => StartRebinding("Interaction"));
        hit.onClick.AddListener(() => StartRebinding("Hit"));
        getItem.onClick.AddListener(() => StartRebinding("GetItem"));
        option.onClick.AddListener(() => StartRebinding("Option"));

        saveButton.onClick.AddListener(() =>
        {
            SaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectsSlider.value, masterSlider.value);
            SaveManager.Instance.SaveKeybindingSettings();
        });

        StartCoroutine(LoadAndApplySettings());
    }

    private IEnumerator LoadAndApplySettings()
    {
        LoadAndSetVolume();
        //LoadGraphicsSettings
        LoadKeyBinds();

        yield return new WaitForSeconds(0.1f);
    }

    private void LoadKeyBinds()
    {
        KeybindSettings keybindingSettings = SaveManager.Instance.LoadKeybindingSettings();

        if (keybindingSettings.keybinds == null || keybindingSettings.keybinds.Count == 0)
        {
            InitializeKeybinds();
            SaveManager.Instance.SaveKeybindingSettings();
            UpdateUI();
        }
        else
        {
            InitializeKeybinds(); // Ensure all keys are present
            SetKeybindsFromSettings(keybindingSettings);
        }
    }


    private void LoadAndSetVolume()
    {
        VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();
        masterSlider.value = volumeSettings.master;
        musicSlider.value = volumeSettings.music;
        effectsSlider.value = volumeSettings.effects;
    }

    private void Update()
    {
        masterVolume.GetComponent<TextMeshProUGUI>().text = "" + (masterSlider.value) + "";
        musicVolume.GetComponent<TextMeshProUGUI>().text = "" + (musicSlider.value) + "";
        effectsVolume.GetComponent<TextMeshProUGUI>().text = "" + (effectsSlider.value) + "";
    }

    private void InitializeKeybinds()
    {
        keybinds = new Dictionary<string, KeyCode>()
    {
        { "Jump", KeyCode.Space },
        { "WalkFront", KeyCode.W },
        { "WalkLeft", KeyCode.A },
        { "WalkRight", KeyCode.D },
        { "WalkBack", KeyCode.S },
        { "Interaction", KeyCode.F },
        { "Hit", KeyCode.Mouse0 },
        { "GetItem", KeyCode.E },
        { "Inventory", KeyCode.I },
        { "Option", KeyCode.Escape }
    };
    }

    private void SetKeybindsFromSettings(KeybindSettings settings)
    {
        foreach (var key in settings.keybinds)
        {
            if (keybinds.ContainsKey(key.Key))
            {
                keybinds[key.Key] = key.Value;
            }
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        jump.GetComponentInChildren<TextMeshProUGUI>().text = keybinds["Jump"].ToString();
        walkFront.GetComponentInChildren<TextMeshProUGUI>().text = keybinds["WalkFront"].ToString();
        walkLeft.GetComponentInChildren<TextMeshProUGUI>().text = keybinds["WalkLeft"].ToString();
        walkRight.GetComponentInChildren<TextMeshProUGUI>().text = keybinds["WalkRight"].ToString();
        walkBack.GetComponentInChildren<TextMeshProUGUI>().text = keybinds["WalkBack"].ToString();
        interaction.GetComponentInChildren<TextMeshProUGUI>().text = keybinds["Interaction"].ToString();
        hit.GetComponentInChildren<TextMeshProUGUI>().text = keybinds["Hit"].ToString();
        getItem.GetComponentInChildren<TextMeshProUGUI>().text = keybinds["GetItem"].ToString();
        option.GetComponentInChildren<TextMeshProUGUI>().text = keybinds["Option"].ToString();
    }

    private void StartRebinding(string key)
    {
        currentKey = key;
        StartCoroutine(WaitForKeyPress());
    }

    private IEnumerator WaitForKeyPress()
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                keybinds[currentKey] = keyCode;
                UpdateUI();
                SaveManager.Instance.SaveKeybindingSettings();
                break;
            }
        }
    }

    public Dictionary<string, KeyCode> GetKeybinds()
    {
        return keybinds;
    }

    /// <summary>
    /// Zwraca KeyCode przypisany do okreœlonej akcji.
    /// </summary>
    /// <param name="action">Nazwa akcji (np. "Jump", "WalkFront").</param>
    /// <returns>KeyCode przypisany do danej akcji lub KeyCode.None, jeœli akcja nie istnieje.</returns>
    public KeyCode GetKeyCode(string action)
    {
        if (keybinds.TryGetValue(action, out KeyCode keyCode))
        {
            return keyCode;
        }
        else
        {
            Debug.LogWarning($"Keybind for action '{action}' not found.");
            return KeyCode.None;
        }
    }
}

