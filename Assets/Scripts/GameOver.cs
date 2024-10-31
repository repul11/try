using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public  Text txScore;
    public Text txHighScore;
    Text txSelamat;
    int highScore;

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt("HS", 0);
        if (Data.score > highScore)
        {
            highScore = Data.score;
            PlayerPrefs.SetInt("HS", highScore);
        }
        else if(EnemyController.EnemyKilled == 13)
        {
            SceneManager.LoadScene("Congrats");
        }
        txHighScore.text = "Highscores: " + highScore;
        txScore.text = "Scores: " + Data.score;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Replay()
    {
        Data.score = 0;
        EnemyController.EnemyKilled = 0;
        SceneManager.LoadScene("Gameplay");
    }
}
