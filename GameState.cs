using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameState : MonoBehaviour
{

    public static Gamestate instance; 

    private void Awake()
    {

        if (instance == null)
        {

            DontDestroyOnLoad(gameObject);
            instance = this;

        }
        else if (instance != this)
        {

            Destroy(gameObject);

        }

    }

    public void DeleteSavedGames()
    {

        PlayerPrefs.DeleteAll();

        if (File.Exists(Application.persistentDataPath + "/Gamestate.dat"))
        {

            try
            {

                File.Delete(Application.persistentDataPath + "/Gamestate.dat");
                Debug.Log("Deleted");

            }
            catch (Exception e)
            {

                Debug.Log(e);

            }

        }
    }


    public void SaveGame()
    {

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/Gamestate.dat", FileMode.OpenOrCreate);

        PlayerData playerData = new PlayerData();

        binaryFormatter.Serialize(file, playerData);
        file.Close();

    }


    public void LoadGame()
    {

        if (File.Exists(Application.persistentDataPath + "/Gamestate.dat"))
        {

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Gamestate.dat", FileMode.Open);

            PlayerData data = (PlayerData)binaryFormatter.Deserialize(file);
            file.Close();

            
        }
        else
        {


        }

    }


}

[Serializable]
public class PlayerData
{

    public PlayerData()
    {
        
    }
}

