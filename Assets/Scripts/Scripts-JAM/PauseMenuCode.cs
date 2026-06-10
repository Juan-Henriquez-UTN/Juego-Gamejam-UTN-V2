using UnityEngine;

public class PauseMenuCode : MonoBehaviour
{
    public void ClosePausedScreen(GameObject pauseScreen)
    {
        Time.timeScale = 1.0f;
        pauseScreen.SetActive(false);
    }
}
