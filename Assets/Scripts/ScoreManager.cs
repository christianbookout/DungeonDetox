using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI inputScore;

    [SerializeField]
    private TMP_InputField inputName;

    public UnityEvent<string, int> submitScoreEvent;

    public EnemySpawner enemySpawner;

    void Update()
    {
        if (enemySpawner != null)
        {
            CalculateScore(enemySpawner.GetNumberOfWaves(), enemySpawner.GetEnemiesRemaining());
        }
    }

    public void SubmitScore()
    {

        submitScoreEvent.Invoke(inputName.text, int.Parse(inputScore.text));
    }

    public void CalculateScore(int waveNumber, int enemiesLeft)
    {
        //Debug.Log("CALLED CALCULATE SCORE");
        int score = (waveNumber * 100) - (enemiesLeft * 10);
        inputScore.text = score.ToString(); // Set the text of the TextMeshProUGUI component.
        
    }

}
