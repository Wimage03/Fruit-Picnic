using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{
    public int row;
    public int column;
    public int previousRow;
    public int previousColumn;
    public int finalPosX;
    public int finalPosY;

    public GridGenerator grid;
    private Vector2 firstClickedPosition;
    private Vector2 finalClickedPosition;
    private Vector2 tempPosition;
    [SerializeField] private float swipeAngle;
    private GameObject otherFruit;
    public bool colorMatched = false;
    private float movementThreshold = 1f;
    private matchFinder matchFinderScript;

    //public static int moveCount = 15;

    private void Start()
    {
        grid = FindObjectOfType<GridGenerator>();
        matchFinderScript = FindObjectOfType<matchFinder>();
        //finalPosX = (int)transform.position.x;
        //finalPosY = (int)transform.position.y;
        //row = finalPosY;
        //column = finalPosX;

    }

    private void Update()
    {
        if (colorMatched)
        {
            Renderer fruitRenderer = GetComponent<Renderer>();
            fruitRenderer.material.color = new Color(0f, 0f, 0f, 0.2f);
        }

        finalPosX = column;
        finalPosY = row;
        // Horizontal swapping

        // If finalPos different from current position
        if(Mathf.Abs(finalPosX - transform.position.x) > 0.1f)
        {
            // Move towards target
            tempPosition = new Vector2(finalPosX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .2f);
            if (grid.generatedFruits[column, row] != gameObject)
            {
                grid.generatedFruits[column, row] = gameObject;
            }
            matchFinderScript.DetectAllMatches();
        }
        else
        {
            tempPosition = new Vector2(finalPosX, transform.position.y);
            transform.position = tempPosition;
        }

        if (Mathf.Abs(finalPosY - transform.position.y) > 0.1f)
        {
            tempPosition = new Vector2(transform.position.x, finalPosY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .2f);
            if (grid.generatedFruits[column, row] != gameObject)
            {
                grid.generatedFruits[column, row] = gameObject;
            }
            matchFinderScript.DetectAllMatches();
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, finalPosY);
            transform.position = tempPosition;

        }
    }

    private void OnMouseDown()
    {
        if (grid.currentState == GameState.move)
        {
            firstClickedPosition = Input.mousePosition;
        }
    }

    private void OnMouseUp()
    {
        if (grid.currentState == GameState.move)
        {
            finalClickedPosition = Input.mousePosition;
            Angle();
        }
    }

    private void Angle()
    {
        if (Mathf.Abs(firstClickedPosition.y - finalClickedPosition.y) > movementThreshold || Mathf.Abs(firstClickedPosition.x - finalClickedPosition.x) > movementThreshold)
        {
            swipeAngle = Mathf.Atan2(finalClickedPosition.y - firstClickedPosition.y, finalClickedPosition.x - firstClickedPosition.x) * 180 / Mathf.PI;
            MovePieces();
            grid.currentState = GameState.wait;
        }
        else
        {
            grid.currentState = GameState.move;
        }
    }

    private void MovePieces()
    {
        if(swipeAngle > 45 && swipeAngle <= 135 && row < grid.height - 1)
        {
            //MoveUp
            otherFruit = grid.generatedFruits[column, row + 1];
            previousRow = row;
            previousColumn = column;
            otherFruit.GetComponent<Fruits>().row--;
            row++;
        }
        else if (((swipeAngle >= 135 && swipeAngle <= 180) || (swipeAngle >= -180 && swipeAngle < -135)) && column > 0)
        {
            //MoveLeft
            otherFruit = grid.generatedFruits[column - 1, row];
            previousRow = row;
            previousColumn = column;
            otherFruit.GetComponent<Fruits>().column++;
            column--;
        }
        else if (swipeAngle >= -135 && swipeAngle < -45 && row > 0)
        {
            //MoveDown
            otherFruit = grid.generatedFruits[column, row - 1];
            previousRow = row;
            previousColumn = column;
            otherFruit.GetComponent<Fruits>().row++;
            row--;
        }
        // 
        else if (((swipeAngle >= 0 && swipeAngle <= 45) || (swipeAngle >= -45 && swipeAngle < 0)) && column < grid.width - 1)
        {
            //MoveRight
            otherFruit = grid.generatedFruits[column + 1, row];
            previousRow = row;
            previousColumn = column;
            otherFruit.GetComponent<Fruits>().column--;
            column++;
        }
        StartCoroutine(CheckMatched());
    }

    private IEnumerator CheckMatched()
    {
        yield return new WaitForSeconds(0.3f);
        if (otherFruit != null)
        {
            if (!colorMatched && !otherFruit.GetComponent<Fruits>().colorMatched)
            {
                otherFruit.GetComponent<Fruits>().column = column;
                otherFruit.GetComponent<Fruits>().row = row;
                column = previousColumn;
                row = previousRow;
                matchFinder.moveCount--;
                grid.currentState = GameState.move;
            }
            else
            {
                matchFinder.moveCount--;
                //List<GameObject> currentMatches = matchFinderScript.currentMatches;
                foreach (GameObject fruit in matchFinderScript.currentMatches)
                {
                    if (!matchFinderScript.matchedColors.Contains(fruit.tag))
                    {
                        matchFinderScript.matchedColors.Add(fruit.tag);
                    }
                }

                matchFinder.score += matchFinderScript.currentMatches.Count;
                Debug.Log(matchFinder.score);
                grid.DestroyMatchedFruits();
            }
            otherFruit = null;
        }
    }

}