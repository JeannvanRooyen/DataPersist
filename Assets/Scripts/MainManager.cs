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
    public Text ScoreTextBest;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public static string recordName { get; set; }
    public static int recordScore { get; set; }

    public static string userName { get; set; }
    public static int userBestScore { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        if (string.IsNullOrEmpty(userName))
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            LoadBest();

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

            ScoreText.text = $"Name: {userName} Score: {m_Points} Best: { userBestScore}";
            ScoreTextBest.text = $"Record: {recordName} Score: {recordScore}";
        }
    }

    private void Update()
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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;

        if (m_Points > userBestScore)
        {
            userBestScore = m_Points;
        }

        if (userBestScore > recordScore)
        {
            SaveBest();
            recordName = userName;
            recordScore = userBestScore;
            ScoreTextBest.text = $"Record: {recordName} Score: {recordScore}";
        }

        ScoreText.text = $"Name: {userName} Score: {m_Points} Best: { userBestScore}";

    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void LoadBest()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            recordName = data.RecordName;
            recordScore = data.RecordScore;
        }
    }

    public void SaveBest()
    {
    

        SaveData data = new SaveData();
        data.RecordScore = userBestScore;
        data.RecordName = userName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    [System.Serializable]
    class SaveData
    {
        public string RecordName;

        public int RecordScore;
    }
}
