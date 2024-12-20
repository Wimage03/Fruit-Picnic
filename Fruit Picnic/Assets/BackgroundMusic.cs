using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (FindObjectsOfType<BackgroundMusic>().Length > 1)
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }
}
