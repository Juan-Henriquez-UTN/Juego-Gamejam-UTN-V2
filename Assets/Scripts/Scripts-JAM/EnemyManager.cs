using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int healthPoints;
    public float moveSpeed = 2f; // Necesita su propia velocidad para el slow

    private float slowTimer = 0f;
    private float originalSpeed;
    private bool isSlowed = false;

    public SceneProgressionManager sceneProgressionManager;

    void Start()
    {
        originalSpeed = moveSpeed;
    }

    void Update()
    {
        // Manejo del slow
        if (isSlowed)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0)
            {
                moveSpeed = originalSpeed;
                isSlowed = false;
            }
        }
    }

    public void TakeDamage()
    {
        healthPoints--;
        if (healthPoints <= 0)
            DefeatEnemy();
    }

    public void TakeCriticalDamage()
    {
        Debug.Log("CRIT");
        healthPoints -= 2;
        if (healthPoints <= 0)
            DefeatEnemy();
    }

    public void ApplySlow(float duration, float speedMultiplier)
    {
        if (!isSlowed)
            originalSpeed = moveSpeed;

        moveSpeed = originalSpeed * speedMultiplier;
        slowTimer = duration;
        isSlowed = true;
    }

    public void DefeatEnemy()
    {
        // Nivel 3: curar al jugador al derrotar enemigo
        int levelProgressCounter = PlayerPrefs.GetInt("LevelProgress", 0);
        if (levelProgressCounter == 2)
        {
            CharacterCombat player = FindFirstObjectByType<CharacterCombat>();
            if (player != null)
                player.Heal(sceneProgressionManager.healAmount);
        }

        sceneProgressionManager.enemiesRequiredToDefeat--;
        Destroy(gameObject);
    }
}