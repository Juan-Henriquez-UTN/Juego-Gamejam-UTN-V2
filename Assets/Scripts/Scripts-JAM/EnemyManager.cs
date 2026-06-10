using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public bool isRangedEnemy;
    public int healthPoints;
    public float shootingRange;
    public float moveSpeed = 2f; // Necesita su propia velocidad para el slow

    private float slowTimer = 0f;
    private float originalSpeed;
    private float distanceToPlayer;
    private bool isSlowed = false;

    public SceneProgressionManager sceneProgressionManager;
    public GameObject player;

    void Start()
    {
        originalSpeed = moveSpeed;
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

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

        EnemyMovement(isRangedEnemy);
    }

    public void EnemyMovement(bool isRanged)
    {
        if (!isRanged)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            if (distanceToPlayer < shootingRange)
            {
                Shoot();
            }
            else
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    public void Shoot()
    {
        Debug.Log("Shooting");
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