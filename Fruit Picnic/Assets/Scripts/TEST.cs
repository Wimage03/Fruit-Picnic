using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TEST : MonoBehaviour
{
    public void LoadLevel1()
    {
        matchFinder.moveCount = 15;
        matchFinder.score = 0;
        matchFinder.redMatched = 0;
        matchFinder.blueMatched = 0;
        matchFinder.yellowMatched = 0;
        matchFinder.greenMatched = 0;
        matchFinder.purpleMatched = 0;
        SceneManager.LoadScene(1);
    }

    public void LoadLevel2()
    {
        matchFinder.moveCount = 15;
        matchFinder.score = 0;
        matchFinder.redMatched = 0;
        matchFinder.blueMatched = 0;
        matchFinder.yellowMatched = 0;
        matchFinder.greenMatched = 0;
        matchFinder.purpleMatched = 0;
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
