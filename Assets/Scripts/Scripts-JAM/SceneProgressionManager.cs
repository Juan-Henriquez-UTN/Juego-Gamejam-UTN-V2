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
    public int totalEnemies;
    public int levelProgress;
    public float levelTimer;
    public TextMeshProUGUI timeText;
    public Image enemyProgressBar; // Barra de progreso para los enemigos

    public AudioSource audioSource;
    public AudioClip[] levelMusic = new AudioClip[3]; // Una canción por nivel, asignar desde el Inspector

    [SerializeField] private bool isFirstLevel;
    [SerializeField] public float[] healthProgression = new float[3];
    [SerializeField] public float[] speedProgression = new float[3];
    [SerializeField] public float healAmount = 10f;

    void Start()
    {
        levelTimer = 0;
        totalEnemies = enemiesRequiredToDefeat;
        UpdateEnemyBar();

        if (!isFirstLevel)
            levelProgress = PlayerPrefs.GetInt("LevelProgress", 0);
        else
        {
            levelProgress = 1;
            PlayerPrefs.SetInt("LevelProgress", levelProgress);
        }

        PlayLevelMusic();
    }

    void Update()
    {
        levelTimer += Time.deltaTime;
        DisplayTime(levelTimer);
        UpdateEnemyBar();

        if (enemiesRequiredToDefeat == 0)
        {
            UpdateProgress();
        }
    }

    void PlayLevelMusic()
    {
        if (audioSource == null || levelMusic.Length == 0) return;

        int index = levelProgress - 1; // levelProgress arranca en 1, el array en 0
        if (index < 0 || index >= levelMusic.Length) return;
        if (levelMusic[index] == null) return; // Verifica que la canción esté asignada

        audioSource.clip = levelMusic[index]; // Asigna la canción correspondiente al nivel actual
        audioSource.loop = true;
        audioSource.Play();
    }

    void UpdateEnemyBar()
    {
        if (enemyProgressBar != null)
            enemyProgressBar.fillAmount = 1f - ((float)enemiesRequiredToDefeat / totalEnemies);
    }

    void UpdateProgress()
    {
        levelProgress++;
        PlayerPrefs.SetInt("LevelProgress", levelProgress);
        LoadSceneWithName("Level" + levelProgress.ToString());
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