using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class moveRemaining : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveText;
    public static int moves;

    private void Start()
    {
        ObtainMove(matchFinder.moveCount);
    }

    

    private void Update()
    {
        if (moves != matchFinder.moveCount)
        {
            moves = matchFinder.moveCount;
            UpdateMoveText(moves); 
        }
    }

    public void ObtainMove(int initialMoveCount)
    {
        moves = initialMoveCount;
        UpdateMoveText(moves);
    }

    private void UpdateMoveText(int moveCount)
    {
        moveText.text = moveCount.ToString();
    }
}
