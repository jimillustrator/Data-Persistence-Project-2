using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public string playerName;
    public int highScore;

    private void Awake()
    {
        //this keeps a new instance of MainManager being
        //created every time you switch between scenes
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadPlayerInfo();
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int highScore;
    }

    public void SavePlayerInfo()
    {
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.highScore = highScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadPlayerInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerName = data.playerName;
            highScore = data.highScore;
        }
    }

}
