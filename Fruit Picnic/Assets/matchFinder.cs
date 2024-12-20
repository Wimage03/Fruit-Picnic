using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matchFinder : MonoBehaviour
{
    private GridGenerator grid;
    public List<GameObject> currentMatches = new List<GameObject>();
    public List<string> matchedColors = new List<string>();
    public bool isHorizontalMatch = false;

    public static int blueMatched = 0;
    public static int redMatched = 0;
    public static int yellowMatched = 0;
    public static int purpleMatched = 0;
    public static int greenMatched = 0;
    public static int score = 0;
    public static int moveCount = 15;

    private void Start()
    {
        grid = FindObjectOfType<GridGenerator>();
    }

    public void DetectAllMatches()
    {
        StartCoroutine(DetectAllMatchesCo());
    }

    private IEnumerator DetectAllMatchesCo()
    {
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                GameObject currentFruit = grid.generatedFruits[i, j];

                if (currentFruit != null)
                {
                    if (i > 0 && i < grid.width - 1)
                    {
                        GameObject leftFruit = grid.generatedFruits[i - 1, j];
                        GameObject rightFruit = grid.generatedFruits[i + 1, j];
                        if (leftFruit != null && rightFruit != null)
                        {
                            if (leftFruit.tag == currentFruit.tag && rightFruit.tag == currentFruit.tag)
                            {
                                isHorizontalMatch = true;
                                if(!currentMatches.Contains(leftFruit))
                                {
                                    currentMatches.Add(leftFruit);
                                }
                                leftFruit.GetComponent<Fruits>().colorMatched = true;

                                if (!currentMatches.Contains(rightFruit))
                                {
                                    currentMatches.Add(rightFruit);
                                }
                                rightFruit.GetComponent<Fruits>().colorMatched = true;

                                if (!currentMatches.Contains(currentFruit))
                                {
                                    currentMatches.Add(currentFruit);
                                }
                                currentFruit.GetComponent<Fruits>().colorMatched = true;
                            }
                        }
                    }

                    if (j > 0 && j < grid.height - 1)
                    {
                        GameObject downFruit = grid.generatedFruits[i, j - 1];
                        GameObject upFruit = grid.generatedFruits[i, j + 1];
                        if (downFruit != null && upFruit != null)
                        {
                            if (downFruit.tag == currentFruit.tag && upFruit.tag == currentFruit.tag)
                            {
                                if (!currentMatches.Contains(downFruit))
                                {
                                    currentMatches.Add(downFruit);
                                }
                                downFruit.GetComponent<Fruits>().colorMatched = true;

                                if (!currentMatches.Contains(upFruit))
                                {
                                    currentMatches.Add(upFruit);
                                }
                                upFruit.GetComponent<Fruits>().colorMatched = true;

                                if (!currentMatches.Contains(currentFruit))
                                {
                                    currentMatches.Add(currentFruit);
                                }
                                currentFruit.GetComponent<Fruits>().colorMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
