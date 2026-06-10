using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCombat : MonoBehaviour
{
    public float distance;
    public bool isTargetingEnemy;
    public float healthPoints;
    public float healthPerLevel;
    public int damagePerLevel = 1;
    public float critChance;
    private CharacterMovement characterMovement;

    public float damageCooldown = 0.4f; // Segundos entre ticks de daño
    private float damageTimer = 0f;

    public float invincibilityCooldown = 2f; // Segundos entre ticks de daño
    private float invincibilityTimer = 0f;

    public SceneProgressionManager sceneProgressionManager;
    public LoadScene loadSceneScript;
    private int levelProgressCounter;

    public int exp;
    public int expThreshold;
    public int level;
    public TextMeshProUGUI levelText;

    void Start()
    {
        loadSceneScript = GameObject.Find("SceneProgressionManager").GetComponent<LoadScene>();
        levelProgressCounter = PlayerPrefs.GetInt("LevelProgress", 0);
        isTargetingEnemy = false;
        healthPoints = sceneProgressionManager.healthProgression[levelProgressCounter];
    }

    void Update()
    {
        if (damageTimer > 0)
            damageTimer -= Time.deltaTime;
        if (invincibilityTimer > 0)
            invincibilityTimer -= Time.deltaTime;

        if (healthPoints <= 0)
            DefeatPlayer();

        if (exp >= expThreshold)
            LevelUp();
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.CompareTag("Enemy"))
    //    {
    //        ApplyHitEffect(other.GetComponent<EnemyManager>());
    //    }
    //}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && damageTimer <= 0)
        {
            ApplyHitEffect(other.GetComponent<EnemyManager>());
            damageTimer = damageCooldown;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Enemy proyectile")) && invincibilityTimer <= 0) //Diferentes tags segun tipo de enemigo y ataque para distintos damage
        {
            Debug.Log("DAMAGE");
            invincibilityTimer = invincibilityCooldown;
            healthPoints--;
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
            enemy.TakeCriticalDamage(damagePerLevel);
        else
            enemy.TakeDamage(damagePerLevel);

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

    public void DefeatPlayer()
    {
        loadSceneScript.LoadSceneWithName("Defeat screen");
    }

    public void LevelUp()
    {
        level++;
        healthPoints += healthPerLevel;
        damagePerLevel++;
        exp = 0;
        levelText.text = ("Lvl " + level.ToString());
    }
}