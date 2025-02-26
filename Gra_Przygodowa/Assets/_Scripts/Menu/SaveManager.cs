using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    #region || -------------- Singleton ---------------- ||
    public static SaveManager Instance;
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
    #endregion

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
        Debug.Log("LoadGame method called...");

        // Continue loading game data after the scene is fully loaded
        AllGameData data = SelectLoadingType(slotNumber);
        if (data == null)
        {
            Debug.LogError("Failed to load game data. File may not exist or is corrupted.");
            return;
        }

        //Player data
        SetPlayerData(SelectLoadingType(slotNumber).playerData);

        //Environment data
        SetEnviromentData(SelectLoadingType(slotNumber).environmentData);

        isLoading = false;

        // After loading all data disable loading screen
        DisableLoadingScreen();

        // Final confirmation that loading is complete
        Debug.Log("Game data loaded successfully.");
    }

    /// <summary>
    /// Runs when we load the game
    /// </summary>
    /// <param name="environmentData"></param>
    private void SetEnviromentData(EnviromentData environmentData)
    {
        // ------------------ Picked Up items ------------------ //
        foreach (Transform itemType in EnvironmentManager.Instance.allItems.transform)
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

        // -------------------- Trees ---------------------------//

        //Destroy all default trees in scene
        foreach (Transform tree in EnvironmentManager.Instance.allTrees.transform)
        {
            Destroy(tree.gameObject);
        }

        // Add trees and stumps 
        foreach (TreeData treeData in environmentData.treeData)
        {
            GameObject treePrefab = Resources.Load<GameObject>(treeData.name);

            if (treePrefab == null)
            {
                Debug.LogError($"Nie znaleziono prefabu o nazwie: {treeData.name}. Sprawd� folder Resources.");
                continue;  // Pomijamy ten wpis, aby unikn�� b��du
            }

            var instantiatedTree = Instantiate(treePrefab,
                new Vector3(treeData.position.x, treeData.position.y, treeData.position.z),
                Quaternion.Euler(treeData.rotation.x, treeData.rotation.y, treeData.rotation.z));

            instantiatedTree.transform.SetParent(EnvironmentManager.Instance.allTrees.transform);
        }

        // ---------------------- Aniamls ----------------------//

        // Destroy animals that sholud not exist
        foreach (Transform animalType in EnvironmentManager.Instance.allAnimals.transform)
        {
            foreach (Transform animal in animalType)
            {
                if (environmentData.animals.Contains(animal.gameObject.name) == false)
                    Destroy(animal.gameObject);
            }
        }

        // ------------------- Storage ------------------------//
        foreach (StorageData storage in environmentData.storage)
        {
            var storageBoxPrefab = Instantiate(Resources.Load<GameObject>("StorageBox_Model"),
                 new Vector3(storage.position.x, storage.position.y, storage.position.z),
                 Quaternion.Euler(storage.rotation.x, storage.rotation.y, storage.rotation.z));

            storageBoxPrefab.GetComponent<StorageBox>().items = storage.items;
            storageBoxPrefab.transform.SetParent(EnvironmentManager.Instance.allPleacables.transform);
        }
    }

    private void SetPlayerData(PlayerData playerData)
    {
        Debug.Log("SetPlayerData");
        //Setting Player stats
        PlayerState.Instance.currentHealth = playerData.playerStats[0];
        PlayerState.Instance.currentStamina = playerData.playerStats[1];
        Vector3 loadedPosition = new Vector3(playerData.playerPositionAndRotation[0], playerData.playerPositionAndRotation[1], playerData.playerPositionAndRotation[2]);
        Vector3 loadedRotation = new Vector3(playerData.playerPositionAndRotation[3], playerData.playerPositionAndRotation[4], playerData.playerPositionAndRotation[5]);
        // PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);

        if (PlayerState.Instance != null && PlayerState.Instance.playerBody != null)
        {
            PlayerState.Instance.playerBody.transform.position = loadedPosition;
            PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);
        }
        else
        {
            Debug.LogWarning("Player body jest null!");
        }

        // Setting the quick slots content
        //foreach (string item in playerData.quickSlotContent)
        //{
        //    Debug.Log("Item to instantiate: " + item);
        //    GameObject itemToInstantiate = Resources.Load<GameObject>(item);
        //    if (itemToInstantiate != null)
        //    {
        //        GameObject availableSlot = EquipSystem.Instance.FindNextEmptySlot();
        //        GameObject instantiatedItem = Instantiate(itemToInstantiate);
        //        instantiatedItem.transform.SetParent(availableSlot.transform, false);
        //    }
        //    else
        //    {
        //        Debug.LogError("Prefab for item " + item + " not found in Resources.");
        //    }
        //}

        // Setting the quick slots content with quantity
        foreach (string item in playerData.quickSlotContent)
        {
            Debug.Log("Item to instantiate: " + item);

            // Sprawdzenie, czy item zawiera ilo�� np. "Stick (6)"
            string itemName = item;
            int itemCount = 1; // Domy�lna ilo��

            int startIndex = item.LastIndexOf('(');
            int endIndex = item.LastIndexOf(')');

            if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
            {
                string numberStr = item.Substring(startIndex + 1, endIndex - startIndex - 1);
                if (int.TryParse(numberStr, out int parsedCount))
                {
                    itemCount = parsedCount;
                    itemName = item.Substring(0, startIndex).Trim(); // Usuni�cie ilo�ci z nazwy przedmiotu
                    Debug.Log("Item to instantiate: " + itemName);
                    Debug.Log("Item to instantiate: " + itemCount);
                }
            }

            GameObject itemToInstantiate = Resources.Load<GameObject>(itemName);
            if (itemToInstantiate != null)
            {
                GameObject availableSlot = EquipSystem.Instance.FindNextEmptySlot();
                if (availableSlot != null)
                {
                    GameObject instantiatedItem = Instantiate(itemToInstantiate);
                    instantiatedItem.transform.SetParent(availableSlot.transform, false);

                    // Szukanie obiektu AmountTXT w instancji przedmiotu
                    Transform amountTextTransform = availableSlot.transform.Find("AmountTXT");
                    if (amountTextTransform != null)
                    {
                        TextMeshProUGUI amountText = amountTextTransform.GetComponent<TextMeshProUGUI>();
                        if (amountText != null)
                        {
                            amountText.text = itemCount.ToString();
                        }
                        else
                        {
                            Debug.LogWarning($"AmountTXT on {itemName} does not have a TextMeshProUGUI component.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"AmountTXT object not found in {itemName} prefab.");
                    }
                }
                else
                {
                    Debug.LogError("No available quick slot found.");
                }
            }
            else
            {
                Debug.LogError("Prefab for item " + itemName + " not found in Resources.");
            }
        }


    }

    // StartLoadedGame
    public void LoadSavedGame(int slotNumber)
    {
        ActivateLoadingScreen();
        isLoading = true;

        // Load the scene asynchronously
        SceneManager.LoadSceneAsync("MainBase").completed += (asyncOperation) =>
        {
            // After the scene is completely loaded, call LoadGame
            Debug.Log("Scene loaded, now loading game data...");
            LoadGame(slotNumber);
        };
    }


    private IEnumerator DelayedLoading(int slotNumber)
    {
        LoadGame(slotNumber);
        // after 1s all scripts will be loaded
        yield return new WaitForSeconds(1f);   
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

    /// <summary>
    /// Runs when we save game
    /// </summary>
    /// <returns></returns>
    private EnviromentData GetEnviromentData()
    {
        //Get all items
        List<string> itemPickedup = InventorySystem.Instance.pickupItems;

        // Get all Trees and stumps
        List<TreeData> treeToSave = new List<TreeData>();
        foreach (Transform tree in EnvironmentManager.Instance.allTrees.transform)
        {
            if (tree.CompareTag("Tree"))
            {
                var td = new TreeData();
                td.name = "TreeParent"; // This needs to be same as prefab name
                td.position = tree.position;
                td.rotation = new Vector3(tree.rotation.x, tree.rotation.y, tree.rotation.z);
                treeToSave.Add(td);
                //var td = new TreeData(tree.position, tree.eulerAngles, "Tree_Parent");
                //treeToSave.Add(td);
            }
            else
            {
                var td = new TreeData();
                td.name = "Stump"; // This needs to be same as prefab name
                td.position = tree.position;
                td.rotation = new Vector3(tree.rotation.x, tree.rotation.y, tree.rotation.z);
                treeToSave.Add(td);
                //var td = new TreeData(tree.position, tree.eulerAngles, "Stump");
                //treeToSave.Add(td);
            }
        }

        // Get all animals 
        List<string> allAnimals = new List<string>();
        foreach (Transform animalType in EnvironmentManager.Instance.allAnimals.transform)
        {
            foreach (Transform animal in animalType)
            {
                allAnimals.Add(animal.gameObject.name);
            }
        }

        // Get all storages
        List<StorageData> allStorage = new List<StorageData>();
        foreach (Transform placeable in EnvironmentManager.Instance.allPleacables.transform)
        {
            if (placeable.gameObject.GetComponent<StorageBox>())
            {
                var storageData = new StorageData();
                storageData.items = placeable.gameObject.GetComponent<StorageBox>().items;
                storageData.position = placeable.position;
                storageData.rotation = new Vector3(placeable.rotation.x, placeable.rotation.y, placeable.rotation.z);
                allStorage.Add(storageData);
            }
        }

        return new EnviromentData(itemPickedup, treeToSave, allAnimals, allStorage);
    }

    private PlayerData GetPlayerData()
    {
        float[] playerStats = new float[2];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentStamina;

        float[] playerPosAndRot = new float[6];
        playerPosAndRot[0] = PlayerState.Instance.playerBody.transform.position.x;
        playerPosAndRot[1] = PlayerState.Instance.playerBody.transform.position.y;
        playerPosAndRot[2] = PlayerState.Instance.playerBody.transform.position.z;

        playerPosAndRot[3] = PlayerState.Instance.playerBody.transform.rotation.x;
        playerPosAndRot[4] = PlayerState.Instance.playerBody.transform.rotation.y;
        playerPosAndRot[5] = PlayerState.Instance.playerBody.transform.rotation.z;

       // string[] inventoryContent = InventorySystem.Instance.itemList.ToArray();
        string[] quickSlotContent = GetQuickSlotsContent();

        return new PlayerData(playerStats, playerPosAndRot, quickSlotContent);
    }

    //private string[] GetQuickSlotsContent()
    //{
    //    List<string> temp = new List<string>();

    //    foreach (GameObject slot in EquipSystem.Instance.quickSlotsList)
    //    {
    //        if (slot.transform.childCount != 1)
    //        {
    //            string name = slot.transform.GetChild(0).name;
    //            string str2 = "(Clone)";
    //            string cleanName = name.Replace(str2, "");
    //            temp.Add(cleanName);
    //        }
    //    }
    //    return temp.ToArray();
    //}

    private string[] GetQuickSlotsContent()
    {
        List<string> temp = new List<string>();

        foreach (GameObject slot in EquipSystem.Instance.quickSlotsList)
        {
            if (slot.transform.childCount != 1)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string cleanName = name.Replace(str2, "");

                // Pobranie ilo�ci przedmiot�w z nazwy lub komponentu
                int itemCount = 1; // Domy�lnie ustaw 1 sztuk�, je�li nie ma dok�adnej liczby

                // Sprawd�, czy w nazwie jest ilo�� np. "Stick (6)"
                int startIndex = cleanName.LastIndexOf('(');
                int endIndex = cleanName.LastIndexOf(')');

                if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
                {
                    string numberStr = cleanName.Substring(startIndex + 1, endIndex - startIndex - 1);
                    if (int.TryParse(numberStr, out int parsedCount))
                    {
                        itemCount = parsedCount;
                        cleanName = cleanName.Substring(0, startIndex).Trim(); // Usu� ilo�� z nazwy
                    }
                }

                temp.Add($"{cleanName} ({itemCount})");
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

        //string encrypted = EncryptionDecryption(json);

        // change to jsonPathPersistant
        using (StreamWriter writer = new StreamWriter(jsonPathProject + fileName + slotNumber + ".json"))
        {
            writer.Write(json);
            print("Saved Game to Json file at " + jsonPathProject + fileName + slotNumber + ".json");
        };

    }

    public AllGameData LoadGameDataFromJsonFile(int slotNumber)
    {
        using (StreamReader reader = new StreamReader(jsonPathProject + fileName + slotNumber + ".json"))
        {
            string json = reader.ReadToEnd();

            // string decrypted = EncryptionDecryption(json);

            AllGameData gameData = JsonUtility.FromJson<AllGameData>(json);
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
    [Serializable]
    public class KeybindSettings
    {
        public Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>();
    }

    public void SaveKeybindingSettings()
    {
        KeybindSettings keybindSettings = new KeybindSettings()
        {
            keybinds = SettingsManager.Instance.GetKeybinds()
        };

        // Save as JSON
        PlayerPrefs.SetString("KeybindSettings", JsonUtility.ToJson(keybindSettings));
        PlayerPrefs.Save();
        Debug.Log("Keybind settings saved");
    }

    public KeybindSettings LoadKeybindingSettings()
    {
        if (PlayerPrefs.HasKey("KeybindSettings"))
        {
            Debug.Log("Keybind settings loaded");
            return JsonUtility.FromJson<KeybindSettings>(PlayerPrefs.GetString("KeybindSettings"));
        }
        else
        {
            Debug.LogWarning("Keybind settings not found, using defaults.");
            return new KeybindSettings();
        }
    }
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
        if (loadingScreen != null)
        {
            loadingScreen.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Loading screen jest null!");
        }
    }

    #endregion

}
