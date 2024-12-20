using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFailScreen : MonoBehaviour
{
    public void Retry()
    {
        FindObjectOfType<GridGenerator>().levelFailScreen.SetActive(false);
        matchFinder.moveCount = 15;
        matchFinder.score = 0;
        matchFinder.redMatched = 0;
        matchFinder.blueMatched = 0;
        matchFinder.yellowMatched = 0;
        matchFinder.greenMatched = 0;
        matchFinder.purpleMatched = 0;
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
        SceneManager.LoadScene(0);
    }
}
