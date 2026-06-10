using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class SceneProgressionManager : MonoBehaviour
{
    [SerializeField] CharacterMovement characterMovementScript;
    public int enemiesRequiredToDefeat;
    public int levelProgress;
    public float levelTimer;
    public TextMeshProUGUI timeText;

    [SerializeField] private bool isFirstLevel;
    [SerializeField] public float[] healthProgression = new float[3];
    [SerializeField] public float[] speedProgression = new float[3];
    [SerializeField] public float healAmount = 10f; // AGREGADO: curación por enemigo derrotado en nivel 3

    void Start()
    {
        levelTimer = 0;
        if (!isFirstLevel)
            levelProgress = PlayerPrefs.GetInt("LevelProgress", 0); 
        else
        {
            levelProgress = 1;
            PlayerPrefs.SetInt("LevelProgress", levelProgress);
        }
    }

    void Update()
    {
        levelTimer += Time.deltaTime;
        DisplayTime(levelTimer);

        if (enemiesRequiredToDefeat == 0)
        {
            UpdateProgress();
        }
    }

    void UpdateProgress()
    {
        levelProgress++;
        PlayerPrefs.SetInt("LevelProgress", levelProgress);
        LoadSceneWithName("Level" + levelProgress.ToString());
        // LOAD NEXT LEVEL
    }

    public void LoadSceneWithName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void WinGame()
    {
        LoadSceneWithName("Victory Screen");
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}