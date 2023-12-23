using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text playerText;
    public GameObject GameOverText;
    public GameObject mainMenuButton;

    public string bestScoreName;
    public int highScoreTotal;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        WallBuilder();
        InfoLoader();
        ShowBestScore();
    }

    private void Update()
    {
        Launcher();
    }

    private void WallBuilder()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Launcher()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HighScoreUpdate();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void HighScoreUpdate()
    {
        if (MainManager.Instance != null)
        {
            if(m_Points > MainManager.Instance.highScore)
            {
                MainManager.Instance.highScore = m_Points;
                MainManager.Instance.highScoreName = MainManager.Instance.playerName;
            }
        }
    }

    public void InfoLoader()
    {
        //this if statement is important to avoid errors when testing the game
        //straight from the main scene rather than the Menu Scene

        if (MainManager.Instance != null)
        {
            bestScoreName = MainManager.Instance.highScoreName;
            highScoreTotal = MainManager.Instance.highScore;
        }
    }

    public void ShowBestScore()
    {
        playerText.text = $"Best Score: {bestScoreName} : {highScoreTotal}";
    }

    public void MainMenuReturn()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        HighScoreUpdate();
        MainManager.Instance.SavePlayerInfo();
        m_GameOver = true;
        GameOverText.SetActive(true);
        mainMenuButton.SetActive(true);
    }
}
