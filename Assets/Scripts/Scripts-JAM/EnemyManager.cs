using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public bool isRangedEnemy;
    public int healthPoints;
    public float shootingRange;
    public float moveSpeed = 2f;

    public float attackSpeed = 1f;
    public float projectileSpeed = 5f;
    public int shotsPerAttack = 1;
    public GameObject projectilePrefab;

    private float shootTimer = 0f;
    private float slowTimer = 0f;
    private float originalSpeed;
    private float distanceToPlayer;
    private bool isSlowed = false;
    private bool isShooting = false;
    private Animator animator;

    public SceneProgressionManager sceneProgressionManager;
    public GameObject player;
    public GameObject critTextPrefab;

    void Start()
    {
        originalSpeed = moveSpeed;
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
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
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        if (animator == null) return;
        animator.SetBool("IsShooting", isShooting);

        Vector2 moveDir = (player.transform.position - transform.position).normalized; // Calcular la direccion de movimiento hacia el jugador
        if (!isShooting)
        {
            Debug.Log(moveDir.x);
            animator.SetFloat("MoveX", moveDir.x);
            animator.SetFloat("MoveY", moveDir.y);
        }
        else
        {
            animator.SetFloat("MoveX", 0f);
            animator.SetFloat("MoveY", 0f);
        }
    }

    public void EnemyMovement(bool isRanged)
    {
        if (!isRanged)
        {
            isShooting = false;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            if (distanceToPlayer < shootingRange)
            {
                isShooting = true;
                if (shootTimer <= 0)
                {
                    Shoot();
                    shootTimer = 1f / attackSpeed;
                }
            }
            else
            {
                isShooting = false;
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    public void Shoot()
    {
        if (projectilePrefab == null) return;

        Vector2 direction = (player.transform.position - transform.position).normalized;

        for (int i = 0; i < shotsPerAttack; i++)
        {
            float angleOffset = 0f;
            if (shotsPerAttack > 1)
                angleOffset = Mathf.Lerp(-20f, 20f, (float)i / (shotsPerAttack - 1));

            Vector2 spreadDirection = Quaternion.Euler(0, 0, angleOffset) * direction;

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = spreadDirection * projectileSpeed;

            Destroy(projectile, 5f);
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
        GameObject critText = Instantiate(critTextPrefab, transform.position, Quaternion.identity);
        Destroy(critText, 2);
        healthPoints -= 2 * playerLevel;
        if (healthPoints <= 0)
            DefeatEnemy();
    }

    public void ApplySlow(float duration, float speedMultiplier)
    {
        Debug.Log("Slowed");
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
        if (levelProgressCounter == 3 && playerCombat != null)
        {
            playerCombat.Heal(sceneProgressionManager.healAmount);
        }
        playerCombat.exp++;
        sceneProgressionManager.enemiesRequiredToDefeat--;
        Destroy(gameObject);
    }
}