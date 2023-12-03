using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{ 

    [SerializeField] int Int_playerLives = 3;
    [SerializeField] int Int_score = 0;

    [SerializeField] TextMeshProUGUI livesText; 
    [SerializeField] TextMeshProUGUI scoreText;

    void Awake()
    {
        int Int_numGameSessions = FindObjectsOfType<GameSession>().Length;

        if (Int_numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = Int_playerLives.ToString();
        scoreText.text = Int_score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (Int_playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int Int_pointsToAdd)
    {
        Int_score += Int_pointsToAdd;
        scoreText.text = Int_score.ToString();
    }

    void TakeLife()
    {
        Int_playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = Int_playerLives.ToString();
    }

    private void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
