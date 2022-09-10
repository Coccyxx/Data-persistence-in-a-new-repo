using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public string playerName;
    public int score;
    public string highscoreName;
    public int highscore;
    public Text highcoreText;

    public float maxVelocity = 3.0f;
    public float startForce = 2.0f;
    public int level = 1;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        LoadHighscore();
        highcoreText.text = "Current Highscore: " + highscoreName + ", " + highscore;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    class Highscore
    {
        public string player;
        public int score;

        public Highscore(string player, int score)
        {
            this.player = player;
            this.score = score;
        }
    }

    public void NewHighscore(int score)
    {
        highscoreName = playerName;
        highscore = score;
        SaveHighscore();
    }

    public void SaveHighscore()
    {
        Highscore hs = new Highscore(playerName, highscore);

        string json = JsonUtility.ToJson(hs);

        Debug.Log(Application.persistentDataPath);
        string path = Path.Combine(Application.persistentDataPath, "savehighscore.json");
        File.WriteAllText(path, json);
    }

    public void LoadHighscore()
    {
        string path = Path.Combine(Application.persistentDataPath, "savehighscore.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Highscore hs = JsonUtility.FromJson<Highscore>(json);

            highscoreName = hs.player;
            highscore = hs.score;
        }
    }

    public void NextLevel(int score)
    {
        this.score = score;
        level++;
        maxVelocity += 0.2f;
        startForce += 0.2f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
