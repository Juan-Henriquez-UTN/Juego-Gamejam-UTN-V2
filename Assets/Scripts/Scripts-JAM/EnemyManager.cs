using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public bool isRangedEnemy;
    public int healthPoints;
    public float shootingRange;
    public float moveSpeed = 2f;

    public float attackSpeed = 1f; // Disparos por segundo
    public float projectileSpeed = 5f;
    public int shotsPerAttack = 1;
    public GameObject projectilePrefab; // Drag and drop tu projectile prefab en el Inspector

    private float shootTimer = 0f;
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

        if (isSlowed)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0)
            {
                moveSpeed = originalSpeed;
                isSlowed = false;
            }
        }

        if (shootTimer > 0)
            shootTimer -= Time.deltaTime;

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
                if (shootTimer <= 0)
                {
                    Shoot();
                    shootTimer = 1f / attackSpeed;
                }
            }
            else
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
    }

    public void Shoot()
    {
        if (projectilePrefab == null) return; // Evita errores si no se asigna el prefab

        Vector2 direction = (player.transform.position - transform.position).normalized; 

        for (int i = 0; i < shotsPerAttack; i++) 
        {
            // Si hay mas de 1 proyectil, los distribuye en abanico
            float angleOffset = 0f;
            if (shotsPerAttack > 1)
                angleOffset = Mathf.Lerp(-20f, 20f, (float)i / (shotsPerAttack - 1)); // Distribuye los ángulos entre -20 y 20 grados

            Vector2 spreadDirection = Quaternion.Euler(0, 0, angleOffset) * direction;

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = spreadDirection * projectileSpeed;

            Destroy(projectile, 5f); // se destruye solo si no golpea nada
        }
    }

    public void TakeDamage(int playerLevel)
    {
        healthPoints -= playerLevel;
        if (healthPoints <= 0)
            DefeatEnemy();
    }

    public void TakeCriticalDamage(int playerLevel)
    {
        Debug.Log("CRIT");
        healthPoints -= 2 * playerLevel;
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
        int levelProgressCounter = PlayerPrefs.GetInt("LevelProgress", 0);
        CharacterCombat playerCombat = FindFirstObjectByType<CharacterCombat>();
        if (levelProgressCounter == 2 && playerCombat != null)
        {
            playerCombat.Heal(sceneProgressionManager.healAmount);
        }
        playerCombat.exp++;
        sceneProgressionManager.enemiesRequiredToDefeat--;
        Destroy(gameObject);
    }
}