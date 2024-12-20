using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class fruitCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI redCounterText;
    [SerializeField] private TextMeshProUGUI blueCounterText;
    [SerializeField] private TextMeshProUGUI yellowCounterText;
    [SerializeField] private TextMeshProUGUI purpleCounterText;
    [SerializeField] private TextMeshProUGUI greenCounterText;

    void Update()
    {
        UpdateFruitCounter();
    }

    private void UpdateFruitCounter()
    {
        if (matchFinder.redMatched <= 3)
        {
            redCounterText.text = matchFinder.redMatched.ToString();
        }

        if (matchFinder.blueMatched <= 3)
        {
            blueCounterText.text = matchFinder.blueMatched.ToString();
        }

        if (matchFinder.yellowMatched <= 3)
        {
            yellowCounterText.text = matchFinder.yellowMatched.ToString();
        }

        if (matchFinder.purpleMatched <= 3)
        {
            purpleCounterText.text = matchFinder.purpleMatched.ToString();
        }

        if (matchFinder.greenMatched <= 3)
        {
            greenCounterText.text = matchFinder.greenMatched.ToString();
        }
    }
}
