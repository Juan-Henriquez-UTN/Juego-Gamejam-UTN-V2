using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public float distance;
    public bool isTargetingEnemy;
    public float healthPoints;
    public float critChance;

    public float damageCooldown = 0.4f; // Segundos entre ticks de daño
    private float damageTimer = 0f;

    public SceneProgressionManager sceneProgressionManager;
    private int levelProgressCounter;

    void Start()
    {
        levelProgressCounter = PlayerPrefs.GetInt("LevelProgress", 0);
        isTargetingEnemy = false;
    }

    void Update()
    {
        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            ApplyHitEffect(other.GetComponent<EnemyManager>());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && damageTimer <= 0)
        {
            ApplyHitEffect(other.GetComponent<EnemyManager>());
            damageTimer = damageCooldown;
        }
    }

    void ApplyHitEffect(EnemyManager enemy)
    {
        if (enemy == null) return; 

        // Tirar crítico
        float randomNum = UnityEngine.Random.Range(1.0f, 100.0f);
        int critRoll = Mathf.RoundToInt(randomNum);
        bool isCrit = critRoll < critChance;

        if (isCrit)
            enemy.TakeCriticalDamage();
        else
            enemy.TakeDamage();

        // Efecto según nivel
        switch (levelProgressCounter)
        {
            case 0:
                // Solo daño, sin efecto extra
                break;
            case 1:
                enemy.ApplySlow(2f, 0.4f); // 2 segundos, 40% de velocidad
                break;
            case 2:
                // La curación se aplica al derrotar, no al golpear (ver EnemyManager)
                break;
        }
    }

    public void Heal(float amount)
    {
        healthPoints += amount;
        Debug.Log($"Curado! HP actual: {healthPoints}");
    }
}