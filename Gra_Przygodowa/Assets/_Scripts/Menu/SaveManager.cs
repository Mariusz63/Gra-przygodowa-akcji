using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public bool isSavingToJson;

    // Json Project Save Path
    string jsonPathProject;

    // Json External/Real Save Path
    string jsonPathPersistant;

    // Binary Save Path
    string binaryPath;

    string fileName = "SavedGame";

    public bool isLoading;

    // Loading Screen
    public Canvas loadingScreen;

    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }

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

    #region // ---- Loading ---- //

    public AllGameData SelectLoadingType(int slotNumber)
    {
        if (isSavingToJson)
        {
            AllGameData allGameData = LoadGameDataFromJsonFile(slotNumber);
            return allGameData;
        }
        else
        {
            AllGameData allGameData = LoadGameDataFromBinaryFile(slotNumber);
            return allGameData;
        }
    }

    public void LoadGame(int slotNumber)
    {
        //Player data
        SetPlayerData(SelectLoadingType(slotNumber).playerData);

        //Environment data
        SetEnviromentData(SelectLoadingType(slotNumber).environmentData);

        isLoading = false;

        // After loading all data disable loading screen
        DisableLoadingScreen();
    }

    private void SetEnviromentData(EnviromentData environmentData)
    {
        foreach (Transform itemType in EnviromentManager.Instance.allItems.transform)
        {
            foreach (Transform item in itemType.transform)
            {
                if (environmentData.pickedupItems.Contains(item.name))
                {
                    Destroy(item.gameObject);
                }
            }
        }

        InventorySystem.Instance.pickupItems = environmentData.pickedupItems;
    }

    private void SetPlayerData(PlayerData playerData)
    {
        //Setting Player stats
        PlayerState.Instance.currentHealth = playerData.playerStats[0];
        PlayerState.Instance.currentCalories = playerData.playerStats[1];
        PlayerState.Instance.currentHydration = playerData.playerStats[2];

        //Setting Player Position
        Vector3 loadedPosition;
        loadedPosition.x = playerData.playerPositionAndRotation[0];
        loadedPosition.y = playerData.playerPositionAndRotation[1];
        loadedPosition.z = playerData.playerPositionAndRotation[2];

        PlayerState.Instance.playerBody.transform.position = loadedPosition;

        //Setting Player Rotation
        Vector3 loadedRotation;
        loadedRotation.x = playerData.playerPositionAndRotation[3];
        loadedRotation.y = playerData.playerPositionAndRotation[4];
        loadedRotation.z = playerData.playerPositionAndRotation[5];

        PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);

        // Setting the quick slots content
        foreach (string item in playerData.quickSlotContent)
        {
            // Find next free quick slot
            GameObject availableSlot = EquipSystem.Instance.FindNextEmptySlot();

            var itemToAdd = Instantiate(Resources.Load<GameObject>(item));

            itemToAdd.transform.SetParent(availableSlot.transform, false);
        }
    }

    // StartLoadedGame
    public void LoadSavedGame(int slotNumber)
    {
        ActivateLoadingScreen();

        isLoading = true;

        SceneManager.LoadScene("MainBase");

        StartCoroutine(DelayedLoading(slotNumber));
    }

    private IEnumerator DelayedLoading(int slotNumber)
    {
        // after 1s all scripts will be loaded
        yield return new WaitForSeconds(1f);
        LoadGame(slotNumber);
    }

    #endregion

    #region // ---- General Section ---- //

    public void SaveGame(int slotNumber)
    {
        AllGameData data = new AllGameData();
        data.playerData = GetPlayerData();
        data.environmentData = GetEnviromentData();
        SelectSavingType(data, slotNumber);
    }

    private EnviromentData GetEnviromentData()
    {
        List<string> itemPickedup = InventorySystem.Instance.pickupItems;
        return new EnviromentData(itemPickedup);
    }

    private PlayerData GetPlayerData()
    {
        float[] playerStats = new float[3];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentCalories;
        playerStats[2] = PlayerState.Instance.currentHydration;

        float[] playerPosAndRot = new float[6];
        playerPosAndRot[0] = PlayerState.Instance.playerBody.transform.position.x;
        playerPosAndRot[1] = PlayerState.Instance.playerBody.transform.position.y;
        playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.position.z;

        playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.rotation.x;
        playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.rotation.y;
        playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.rotation.z;


        //string[] inventoryContent = InventorySystem.Instance.itemList.ToArray();
        string[] quickSlotContent = GetQuickSlotsContent();

        return new PlayerData(playerStats, playerPosAndRot, quickSlotContent);
    }

    private string[] GetQuickSlotsContent()
    {
        List<string> temp = new List<string>();

        foreach (GameObject slot in EquipSystem.Instance.quickSlotsList)
        {
            if (slot.transform.childCount != 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string cleanName = name.Replace(str2, "");
                temp.Add(cleanName);
            }
        }
        return temp.ToArray();
    }

    public void SelectSavingType(AllGameData gameData, int slotNumber)
    {
        if (isSavingToJson)
        {
            SaveGameDataToJsonFile(gameData, slotNumber);
        }
        else
        {
            SaveGameDataToBinaryFile(gameData, slotNumber);
        }
    }

    #endregion

    #region // ---- To Binary Section ---- //

    public void SaveGameDataToBinaryFile(AllGameData gameData, int slotNumber)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Data saved to " + binaryPath + fileName + slotNumber + ".bin");
    }

    public AllGameData LoadGameDataFromBinaryFile(int slotNumber)
    {

        if (File.Exists(binaryPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stram = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Open);

            AllGameData data = formatter.Deserialize(stram) as AllGameData;
            stram.Close();

            print("Data loaded from " + binaryPath + fileName + slotNumber + ".bin");

            return data;
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region // ---- To Json Section ---- //

    public void SaveGameDataToJsonFile(AllGameData gameData, int slotNumber)
    {
        string json = JsonUtility.ToJson(gameData);

        string encrypted = EncryptionDecryption(json);

        // change to jsonPathPersistant
        using (StreamWriter writer = new StreamWriter(jsonPathProject + fileName + slotNumber + ".json"))
        {
            writer.Write(encrypted);
            print("Saved Game to Json file at " + jsonPathProject + fileName + slotNumber + ".json");
        };

    }

    public AllGameData LoadGameDataFromJsonFile(int slotNumber)
    {
        using (StreamReader reader = new StreamReader(jsonPathProject + fileName + slotNumber + ".json"))
        {
            string json = reader.ReadToEnd();

            string decrypted = EncryptionDecryption(json);

            AllGameData gameData = JsonUtility.FromJson<AllGameData>(decrypted);
            return gameData;
        };
    }

    #endregion

    #region // ----- Settings Section ---- //

    #region // ----- Volume Settings ---- //
    [Serializable]
    public class VolumeSettings
    {
        public float music = 50;
        public float effects = 50;
        public float master = 50;
    }

    public void SaveVolumeSettings(float _music, float _effects, float _master)
    {
        VolumeSettings volumeSettings = new VolumeSettings()
        {
            music = _music,
            effects = _effects,
            master = _master
        };

        //saving as a JSON
        PlayerPrefs.SetString("VolumeSettings", JsonUtility.ToJson(volumeSettings));
        PlayerPrefs.Save();
        print("Saved to player pref");

    }

    public VolumeSettings LoadVolumeSettings()
    {
        print("Loaded player pref volume");
        return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("VolumeSettings"));
    }
    #endregion

    #region // ---- Graphics Settings ---- //

    #endregion

    #region // ---- Key Binds ---- //

    #endregion

    #endregion

    #region || ------ Encryption ----- ||

    public string EncryptionDecryption(string jsonString)
    {
        string keyword = "0987654321";

        string result = "";

        for (int i = 0; i < jsonString.Length; i++)
        {
            result += (char)(jsonString[i] ^ keyword[i % keyword.Length]);
        }
        return result;
    }

    #endregion

    #region || ------ Utility ------- ||

    public bool DoesFileExists(int slotNumber)
    {
        if (isSavingToJson)
        {
            //SaveGame1.json , SaveGame2.json ...
            if (System.IO.File.Exists(jsonPathProject + fileName + slotNumber + ".json"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (System.IO.File.Exists(binaryPath + fileName + slotNumber + ".bin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool IsSlotEmpty(int slotNumber)
    {
        if (DoesFileExists(slotNumber))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void DeselectButton()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    #endregion

    #region || -------- Loading Section ------------- ||

    public void ActivateLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // can add for example: animation, tips , music
    }

    public void DisableLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(false);
    }

    #endregion

}
