using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCalculator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    void Update()
    {
        UpdateScoreCounter();
    }

    private void UpdateScoreCounter()
    {
        scoreText.text = "Score: " + (matchFinder.score * 10).ToString();
    }
}
