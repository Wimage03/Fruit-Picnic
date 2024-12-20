using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;


public enum GameState {wait, move};

public class GridGenerator : MonoBehaviour
{
    public GameState currentState = GameState.move;
    public int width;
    public int height;
    public int offSet = 10;
    [SerializeField] private GameObject tileprefab;
    [SerializeField] private GameObject[] fruitTypes;
    public GameObject[,] generatedFruits;
    private matchFinder matchFinderScript;
    private Tile[,] tiles;
    private timer timerScript;
    //private moveRemaining moveRemainingScript;
    bool levelCompleted = false;

    public GameObject levelFailScreen;
    public GameObject levelCompleteScreen;

    private void Start()
    {   
        tiles = new Tile[width, height];
        generatedFruits = new GameObject[width, height];
        matchFinderScript = FindObjectOfType<matchFinder>();
        timerScript = FindObjectOfType<timer>();
        //moveRemainingScript = FindObjectOfType<moveRemaining>();

        levelFailScreen.SetActive(false);
        levelCompleteScreen.SetActive(false);

        matchFinder.moveCount = 15;

        SetUp();
    }

    private void Update()
    {
        if(matchFinder.redMatched > 2 &&
            matchFinder.blueMatched > 2 &&
            matchFinder.yellowMatched > 2 &&
            matchFinder.greenMatched > 2 &&
            matchFinder.purpleMatched > 2)
        {
            if (!levelCompleted)
            {
                levelCompleteScreen.SetActive(true);

                FindObjectOfType<LevelComplete>().SetFinalScore(matchFinder.score);


                matchFinder.redMatched = 0;
                matchFinder.blueMatched = 0;
                matchFinder.yellowMatched = 0;
                matchFinder.greenMatched = 0;
                matchFinder.purpleMatched = 0;
                matchFinder.score = 0;
                
                levelCompleted = true;
            }
        }

        if ((timerScript.timeRemaining < 1 || moveRemaining.moves < 1) && !levelCompleted)
        {

            levelFailScreen.SetActive(true);
        }
    }

    private void SetUp()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Vector2 tilePosition = new Vector2(i, j);
                GameObject tile = Instantiate(tileprefab, tilePosition, Quaternion.identity, transform);

                tile.name = "( " + i + ", " + j + " )";

                int randomFruitIndex = Random.Range(0, fruitTypes.Length);

                while (ColorMatchedAtStart(i, j, fruitTypes[randomFruitIndex]))
                {
                    randomFruitIndex = Random.Range(0, fruitTypes.Length);
                }

                Vector2 fruitPosition = new Vector2(i, j + offSet + 15);

                GameObject fruit = Instantiate(fruitTypes[randomFruitIndex], fruitPosition, Quaternion.identity, transform);
                fruit.GetComponent<Fruits>().row = j;
                fruit.GetComponent<Fruits>().column = i;

                fruit.name = "( " + i + ", " + j + " )";
                generatedFruits[i, j] = fruit;
            }
        }
    }



    private bool ColorMatchedAtStart(int column, int row, GameObject fruit)
    {
        if(row > 1 && column > 1)
        {
            
                if(generatedFruits[column - 1, row].tag == fruit.tag && generatedFruits[column - 2, row].tag == fruit.tag)
                {
                    return true;
                }
                else if(generatedFruits[column, row - 1].tag == fruit.tag && generatedFruits[column, row - 2].tag == fruit.tag)
                {
                    return true;
                }
            
        }
        else if (row <= 1 || column <= 1)
        {
            if (column > 1)
            {
                if (generatedFruits[column - 1, row].tag == fruit.tag && generatedFruits[column - 2, row].tag == fruit.tag)
                {
                    return true;
                }
            }
            else if (row > 1)
            {
                if (generatedFruits[column, row - 1].tag == fruit.tag && generatedFruits[column, row - 2].tag == fruit.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void DestroyMatchedFruitsHelper(int column, int row)
    {
        if (generatedFruits[column, row].GetComponent<Fruits>().colorMatched)
        {
            matchFinderScript.currentMatches.Remove(generatedFruits[column, row]);

            if (matchFinderScript.matchedColors.Contains(generatedFruits[column, row].tag))
            {
                if(generatedFruits[column, row].tag.EndsWith("Red"))
                {
                    matchFinder.redMatched++;
                }
                if (generatedFruits[column, row].tag.EndsWith("Blue"))
                {
                    matchFinder.blueMatched++;
                }
                if (generatedFruits[column, row].tag.EndsWith("Yellow"))
                {
                    matchFinder.yellowMatched++;
                }
                if (generatedFruits[column, row].tag.EndsWith("Purple"))
                {
                    matchFinder.purpleMatched++;
                }
                if (generatedFruits[column, row].tag.EndsWith("Green"))
                {
                    matchFinder.greenMatched++;
                }
            }

            if (matchFinderScript.matchedColors.Contains(generatedFruits[column, row].tag))
            {
                matchFinderScript.matchedColors.Remove(generatedFruits[column, row].tag);
            }
            
            Destroy(generatedFruits[column, row]);
            generatedFruits[column, row] = null;
        }
    }

    public void DestroyMatchedFruits()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(generatedFruits[i, j] != null)
                {
                    DestroyMatchedFruitsHelper(i, j);
                }
            }
        }
        StartCoroutine(CascadingFruits());
    }

    private IEnumerator CascadingFruits()
    {
        int destroyedFruitCount = 0;

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(generatedFruits[i, j] == null)
                {
                    destroyedFruitCount++;
                }
                else if(destroyedFruitCount > 0)
                {
                    generatedFruits[i, j].GetComponent<Fruits>().row -= destroyedFruitCount;
                    generatedFruits[i, j] = null;
                }
            }

            destroyedFruitCount = 0;
        }

        yield return new WaitForSeconds(.3f);
        StartCoroutine(FillGrid());
    }

    

    private bool MatchesOnGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if(generatedFruits[i, j] != null)
                {
                    if(generatedFruits[i, j].GetComponent<Fruits>().colorMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void FillGridHelper()
    {
        for (int i = 0; i < width; i++)
        {
            GameObject lastGeneratedFruit = null;

            for (int j = 0; j < height; j++)
            {
                if (generatedFruits[i, j] == null)
                {
                    Vector2 newPos = new Vector2(i, j + offSet);
                    GameObject newFruit;

                    bool isBottomFruit = (lastGeneratedFruit == null);

                    if(SceneManager.GetActiveScene().buildIndex == 1)
                    {
                        newFruit = Instantiate(GetNextFruit(i, j, isBottomFruit, matchFinderScript.isHorizontalMatch, lastGeneratedFruit), newPos, Quaternion.identity);
                        Debug.Log("Level1!!!");
                    }

                    else if (SceneManager.GetActiveScene().buildIndex == 2)
                    {
                        Debug.Log("Level2!!!");
                        newFruit = Instantiate(GetNextFruitLevelTwo(i, j, isBottomFruit, matchFinderScript.isHorizontalMatch, lastGeneratedFruit), newPos, Quaternion.identity);
                    }

                    else
                    {
                        newFruit = Instantiate(GetNextFruit(i, j, isBottomFruit, matchFinderScript.isHorizontalMatch, lastGeneratedFruit), newPos, Quaternion.identity);
                        Debug.Log("Default");
                    }

                    generatedFruits[i, j] = newFruit;
                    if (!matchFinderScript.isHorizontalMatch)
                    {
                        lastGeneratedFruit = newFruit;
                    }

                    newFruit.GetComponent<Fruits>().row = j;
                    newFruit.GetComponent<Fruits>().column = i;
                }
            }
        }
    }

    private IEnumerator FillGrid()
    {
        FillGridHelper();
        yield return new WaitForSeconds(0.2f);

        while(MatchesOnGrid())
        {
            yield return new WaitForSeconds(0.2f);
            foreach (GameObject fruit in matchFinderScript.currentMatches)
            {
                if (!matchFinderScript.matchedColors.Contains(fruit.tag))
                {
                    matchFinderScript.matchedColors.Add(fruit.tag);
                }
            }

            matchFinder.score += matchFinderScript.currentMatches.Count;


            DestroyMatchedFruits();
        }
        yield return new WaitForSeconds(0.2f);
        currentState = GameState.move;
    }

    private GameObject GetNextFruitLevelTwo(int row, int column, bool isBottomFruit, bool isHorizontalMatch, GameObject lastGeneratedFruit = null)
    {
        // Ensure fruitTypes is not empty
        if (fruitTypes == null || fruitTypes.Length == 0)
        {
            Debug.LogError("fruitTypes array is empty or not initialized.");
            return null;
        }

        // For horizontal matches, each fruit has a 60% chance of matching the fruit directly below in the same column
        if (isHorizontalMatch)
        {
            GameObject chosenFruit = null;

            if (row > 0)
            {
                GameObject belowFruit = generatedFruits[column, row - 1];

                // Ensure belowFruit is not null
                if (belowFruit != null)
                {
                    if (Random.value < 0.6f)
                    {
                        chosenFruit = belowFruit;
                    }
                    else
                    {
                        int randomIndex = Random.Range(0, fruitTypes.Length);
                        chosenFruit = fruitTypes[randomIndex];
                    }
                }
                else
                {
                    // If belowFruit is null, choose a random fruit
                    int randomIndex = Random.Range(0, fruitTypes.Length);
                    chosenFruit = fruitTypes[randomIndex];
                }
            }
            else
            {
                // If row == 0, choose a random fruit
                int randomIndex = Random.Range(0, fruitTypes.Length);
                chosenFruit = fruitTypes[randomIndex];
            }

            // Ensure chosenFruit is not null
            if (chosenFruit == null)
            {
                Debug.LogError("chosenFruit is null in horizontal match logic.");
            }

            return chosenFruit;
        }

        // For vertical matches, calculate probability based on neighbors of the fruit directly below the matched area
        if (isBottomFruit && row > 0)
        {
            GameObject chosenFruit = null;

            // Find the fruit directly below the matched area
            GameObject belowFruit = generatedFruits[column, row - 1];

            // Ensure belowFruit is not null
            if (belowFruit != null)
            {
                // Get neighbor colors and counts
                Dictionary<string, int> colorCounts = new Dictionary<string, int>();
                int totalNeighborCount = 0;

                // List of neighbor positions (up, down, left, right, and diagonals)
                Vector2Int[] neighborOffsets = {
                new Vector2Int(0, 1),    // up
                new Vector2Int(0, -1),   // down
                new Vector2Int(1, 0),    // right
                new Vector2Int(-1, 0),   // left
                new Vector2Int(1, 1),    // diagonal up-right
                new Vector2Int(1, -1),   // diagonal down-right
                new Vector2Int(-1, -1),  // diagonal down-left
                new Vector2Int(-1, 1)    // diagonal up-left
            };

                // Loop through neighbors
                foreach (var offset in neighborOffsets)
                {
                    int neighborRow = row - 1 + offset.y;
                    int neighborColumn = column + offset.x;

                    // Check if the neighbor is within grid bounds
                    if (neighborRow >= 0 && neighborRow < height && neighborColumn >= 0 && neighborColumn < width)
                    {
                        GameObject neighborFruit = generatedFruits[neighborColumn, neighborRow];
                        if (neighborFruit != null)
                        {
                            string color = neighborFruit.tag;  // Assuming color is represented by the tag
                            if (!colorCounts.ContainsKey(color))
                            {
                                colorCounts[color] = 0;
                            }
                            colorCounts[color]++;
                            totalNeighborCount++;
                        }
                    }
                }

                // Calculate probability distribution for each color
                Dictionary<string, float> colorProbabilities = new Dictionary<string, float>();
                float probabilitySum = 0f;

                foreach (var colorCount in colorCounts)
                {
                    float probability = (float)(1 + colorCount.Value) / (1 + totalNeighborCount);
                    colorProbabilities[colorCount.Key] = probability;
                    probabilitySum += probability;
                }

                // Normalize probabilities to ensure they sum to 1
                if (probabilitySum > 0f)
                {
                    List<string> colors = new List<string>(colorProbabilities.Keys);
                    foreach (var color in colors)
                    {
                        colorProbabilities[color] /= probabilitySum;
                    }

                    // Select the first fruit color based on calculated probabilities
                    float randomValue = Random.value;
                    float cumulativeProbability = 0f;

                    foreach (var colorProbability in colorProbabilities)
                    {
                        cumulativeProbability += colorProbability.Value;
                        if (randomValue <= cumulativeProbability)
                        {
                            chosenFruit = fruitTypes.FirstOrDefault(fruit => fruit.tag == colorProbability.Key);
                            if (chosenFruit != null)
                            {
                                break;
                            }
                        }
                    }
                }

                // If no color matched or chosenFruit is still null, default to a random fruit
                if (chosenFruit == null)
                {
                    int randomIndex = Random.Range(0, fruitTypes.Length);
                    chosenFruit = fruitTypes[randomIndex];
                }

                // Ensure chosenFruit is not null
                if (chosenFruit == null)
                {
                    Debug.LogError("chosenFruit is null in vertical match logic.");
                }

                return chosenFruit;
            }
            else
            {
                // If belowFruit is null, choose a random fruit
                int randomIndex = Random.Range(0, fruitTypes.Length);
                chosenFruit = fruitTypes[randomIndex];

                // Ensure chosenFruit is not null
                if (chosenFruit == null)
                {
                    Debug.LogError("chosenFruit is null when belowFruit is null.");
                }

                return chosenFruit;
            }
        }
        else
        {
            // For other fruits in the vertical bar: 60% chance to match the previous generated fruit
            GameObject chosenFruit = null;

            if (lastGeneratedFruit != null && Random.value < 0.6f)
            {
                chosenFruit = lastGeneratedFruit;
            }
            else
            {
                int randomIndex = Random.Range(0, fruitTypes.Length);
                chosenFruit = fruitTypes[randomIndex];
            }

            // Ensure chosenFruit is not null
            if (chosenFruit == null)
            {
                Debug.LogError("chosenFruit is null in else block.");
            }

            return chosenFruit;
        }
    }

    private GameObject GetNextFruit(int row, int column, bool isBottomFruit, bool isHorizontalMatch, GameObject lastGeneratedFruit = null)
    {
        // If there's a fruit directly below the matched area (for both horizontal and vertical matches)
        if (row > 0 && generatedFruits[column, row - 1] != null)
        {
            GameObject belowFruit = generatedFruits[column, row - 1];
            GameObject chosenFruit;

            if (isHorizontalMatch)
            {
                // For horizontal matches, each fruit has a 60% chance of matching the fruit directly below in the same column
                if (Random.value < 0.6f)
                {
                    chosenFruit = belowFruit;
                }
                else
                {
                    int randomIndex = Random.Range(0, fruitTypes.Length);
                    chosenFruit = fruitTypes[randomIndex];
                }
            }
            else
            {
                // For vertical matches, handle the bottom-most fruit with a 40% probability to match the color of the below fruit
                if (isBottomFruit)
                {
                    if (Random.value < 0.4f)
                    {
                        chosenFruit = belowFruit;
                    }
                    else
                    {
                        int randomIndex = Random.Range(0, fruitTypes.Length);
                        chosenFruit = fruitTypes[randomIndex];
                    }
                }
                else
                {
                    // For other fruits in a vertical bar: 60% chance to match the previous generated fruit in the bar
                    if (lastGeneratedFruit != null && Random.value < 0.6f)
                    {
                        chosenFruit = lastGeneratedFruit;
                    }
                    else
                    {
                        int randomIndex = Random.Range(0, fruitTypes.Length);
                        chosenFruit = fruitTypes[randomIndex];
                    }
                }
            }

            return chosenFruit;
        }
        else
        {
            // If no fruit below, fall back to a random fruit
            int randomIndex = Random.Range(0, fruitTypes.Length);
            return fruitTypes[randomIndex];
        }
    }

}
