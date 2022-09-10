using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;

    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    private bool m_LevelOver = false;
    public Ball ball;

    public int bricks = 36;

    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Main");
        HighScoreText.text = "Best Score " + MenuManager.Instance.highscoreName + " : " + MenuManager.Instance.highscore;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
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

        if (MenuManager.Instance.level > 1) startGame();
    }

    public void startGame()
    {
        m_Points = MenuManager.Instance.score;
        m_Started = true;
        float randomDirection = Random.Range(-1.0f, 1.0f);
        Vector3 forceDir = new Vector3(randomDirection, 1, 0);
        forceDir.Normalize();

        Ball.transform.SetParent(null);
        Ball.AddForce(forceDir * MenuManager.Instance.startForce, ForceMode.VelocityChange);

    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startGame();
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (bricks == 0)
        {
            MenuManager.Instance.NextLevel(m_Points);
        }

    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Level: {MenuManager.Instance.level}, Score : {m_Points}";
        bricks--;
    }

    public void GameOver()
    {
        m_GameOver = true;
        CheckHighscore();
        GameOverText.SetActive(true);
    }

    public void CheckHighscore()
    {
        if (m_Points > MenuManager.Instance.highscore)
        {
            MenuManager.Instance.NewHighscore(m_Points);
        }
    }

}
