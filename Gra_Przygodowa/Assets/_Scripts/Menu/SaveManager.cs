using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Playables;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public bool isSavingToJson;

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


    #region // ---- General Section ---- //

    public void SaveGame()
    {
        AllGameData data = new AllGameData();
        data.playerData = GetPlayerData();
        SaveAllGameData(data);
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

        return new PlayerData(playerStats,playerPosAndRot);
    }

    public void SaveAllGameData(AllGameData gameData)
    {
        if (isSavingToJson)
        {
            //SaveGameDataToJsonFile(gameData);
        }
        else
        {
            SaveGameDataToBinaryFile(gameData);
        }
    }

    #endregion

    #region // ---- To Binary Section ---- //

    public void SaveGameDataToBinaryFile(AllGameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/save_game.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Data saved to " + Application.persistentDataPath + "/save_game.bin");
    }

    public AllGameData LoadGameDataFromBinaryFile()
    {
        string path = Application.persistentDataPath+ "/save_game.bin";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stram = new FileStream(path, FileMode.Open);

            AllGameData data = formatter.Deserialize(stram) as AllGameData;
            stram.Close();

            return data;
        }
        else
        {
            return null;
        }
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
}
