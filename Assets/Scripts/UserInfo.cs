using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    public static UserInfo Instance;
    public static string playerName {get; private set;}
    public static List<Scores> scores {get; private set;}
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        LoadScores();
    }
    bool IsRanked(int score)
    {
        if (scores == null || scores.Capacity < 3)
        {
            return true;
        }
        Scores lastPlace = scores[2];
        if (lastPlace.Points < score)
        {
            return true;
        }
        return false;
    }
    public void UpdatePlayerName(string name)
    {
        playerName = name;
    }
    public void SaveScores(int newScore)
    {
        
        if (IsRanked(newScore))
        {       
            Scores newScoreToAdd = new Scores(playerName, newScore);
            List<Scores> newScoresToSave = new List<Scores>(3);            
            newScoresToSave.Add(newScoreToAdd);

            if (scores != null && scores.Capacity > 0)
            {
                newScoresToSave.AddRange(scores);
            }
            
            newScoresToSave.Sort((obj1, obj2) => obj2.Points.CompareTo(obj1.Points));
            ScoresList scoresWrapper = new ScoresList(newScoresToSave);
            string json = JsonUtility.ToJson(scoresWrapper);
            File.WriteAllText(Application.persistentDataPath + "/scoreslist.json", json);
        }
    }
    public void LoadScores()
    {
        string path = Application.persistentDataPath + "/scoreslist.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ScoresList scoreWrapper = JsonUtility.FromJson<ScoresList>(json);

            scores = scoreWrapper.scoresList;
        }
    }
    public bool isHighScore(int score)
    {

        if (scores == null || scores.Capacity == 0)
        {
            return true;
        }
        Scores firstPlace = scores[0];
        if (score > firstPlace.Points)
        {
            return true;
        }

        return false;
    }
    [System.Serializable]
    public class Scores
    {
        public string Name;
        public int Points;
        public Scores(string name, int points)
        {
            Name = name;
            Points = points;
        }
    }
    [System.Serializable]
    public class ScoresList
    {
        public List<Scores> scoresList;
        public ScoresList(List<Scores> scores)
        {
            scoresList = scores;
        }
    }
}
