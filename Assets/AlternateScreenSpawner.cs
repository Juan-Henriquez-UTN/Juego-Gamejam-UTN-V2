using UnityEngine;
using UnityEngine.UI;

public class AlternateScreenSpawner : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject loreScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loreScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !loreScreen.activeInHierarchy)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
