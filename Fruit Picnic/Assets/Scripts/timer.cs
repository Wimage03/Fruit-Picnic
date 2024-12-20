using UnityEngine;
using TMPro;

public class timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeRemaining = 90f;
    bool isPaused = false;

    public void PauseTheGame()
    {
        isPaused = true;
    }

    public void ResumeTheGame()
    {
        isPaused = false;
    }

    private void Update()
    {
        if (!isPaused)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                UpdateTimerDisplay(timeRemaining);
            }
        }
        else
        {
            UpdateTimerDisplay(timeRemaining);
        }
    }

    private void UpdateTimerDisplay(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

    }
}
