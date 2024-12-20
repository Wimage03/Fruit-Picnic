using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelComplete : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI starText;
    private GridGenerator gridScript;
    private int scoreKeeper;

    private void Start()
    {
        gridScript = FindObjectOfType<GridGenerator>();
        matchFinder.score = 0;
        matchFinder.moveCount = 15;
        // Display the stored scoreKeeper value
        DisplayScore(scoreKeeper);
    }

    public void SetFinalScore(int finalScore)
    {
        scoreKeeper = finalScore * 10;
        if(scoreKeeper > 500 && scoreKeeper < 601)
        {
            DisplayStar("1 Star");
        }
        else if(scoreKeeper <= 500)
        {
            DisplayStar("0 Star");
        }
        else if(scoreKeeper > 600 && scoreKeeper < 700)
        {
            DisplayStar("2 Stars");
        }
        else if (scoreKeeper > 700)
        {
            DisplayStar("3 Stars");
        }

        DisplayScore(scoreKeeper);
    }

    public void Replay()
    {
        matchFinder.score = 0;
        moveRemaining.moves = 15;
        gridScript.levelCompleteScreen.SetActive(false);

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SceneManager.LoadScene(1);
        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(2);
        }
    }

    public void MainMenu()
    {
        matchFinder.score = 0;
        moveRemaining.moves = 15;
        SceneManager.LoadScene(0);
    }

    private void DisplayScore(int score)
    {
        scoreText.text = score.ToString();
    }

    private void DisplayStar(string stars)
    {
        starText.text = stars;
    }
}
