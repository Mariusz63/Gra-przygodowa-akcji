using System;

[Serializable]
public class PlayerData
{
    public float[] playerStats;// [0] - Health, [1] - Calories, [2] - Hydration

    public float[] playerPositionAndRotation; // position x,y,z and rotation x,y,z

    //public float[] inventoryContent;

    public PlayerData(float[] _playerStats, float[] _playerPosAndRot)
    {
        this.playerStats = _playerStats;
        this.playerPositionAndRotation = _playerPosAndRot;
    }

}

// mozna rozbic na rozne zmienne np.
/*
 * public float playerHealth;
 * public float playerHydration; ... * 
 */