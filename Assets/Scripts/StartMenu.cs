using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class StartMenu : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    public TMP_Text highScoreText;
    public string newPlayerName;

    public void Awake()
    {
        MainManager.Instance.LoadPlayerInfo();
    }

    public void Start()
    {
        highScoreText.text = $"High Score: {MainManager.Instance.highScoreName}: {MainManager.Instance.highScore}";
    }

    public void GetPlayerName(string s)
    {
        newPlayerName = s;
        Debug.Log(newPlayerName);
        NewNameEntered(s);
    }

    public void NewNameEntered(string s)
    {
        MainManager.Instance.playerName = s;
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ClearGame()
    {
        MainManager.Instance.highScore = 0;
        MainManager.Instance.highScoreName = " ";
        MainManager.Instance.SavePlayerInfo();
        //EditorApplication below needed the UnityEditor namespace to run
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

}
