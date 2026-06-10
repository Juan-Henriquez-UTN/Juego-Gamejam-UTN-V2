using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneProgressionManager : MonoBehaviour
{
    [SerializeField] CharacterMovement characterMovementScript;
    public int enemiesRequiredToDefeat;
    public int levelProgress;

    [SerializeField] private bool isFirstLevel;

    [SerializeField] public float[] healthProgression = new float[3];
    [SerializeField] public float[] speedProgression = new float[3];
    [SerializeField] public float healAmount = 10f; // AGREGADO: curación por enemigo derrotado en nivel 3

    void Start()
    {
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
}