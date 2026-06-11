using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCombat : MonoBehaviour
{
    public float distance;
    public bool isTargetingEnemy;
    public float healthPoints;
    public float maxHP;
    public float healthPerLevel;
    public int damagePerLevel = 1;
    public float critChance;
    private CharacterMovement characterMovement;

    public float damageCooldown = 0.4f;
    private float damageTimer = 0f;

    public float invincibilityCooldown = 2f;
    public float invincibilityTimer = 0f;

    public SceneProgressionManager sceneProgressionManager;
    public LoadScene loadSceneScript;
    private int levelProgressCounter;

    public int exp;
    public int expThreshold;
    public int level;
    public TextMeshProUGUI levelText;
    public Image healthBar; // Imagen con fill type para la barra de vida

    void Start()
    {
        loadSceneScript = GameObject.Find("SceneProgressionManager").GetComponent<LoadScene>();
        levelProgressCounter = PlayerPrefs.GetInt("LevelProgress", 0);
        isTargetingEnemy = false;
        healthPoints = sceneProgressionManager.healthProgression[levelProgressCounter];
        maxHP = healthPoints;
        UpdateHealthBar();
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

        UpdateHealthBar();
    }

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
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Enemy projectile")) && invincibilityTimer <= 0)
        {
            Debug.Log("DAMAGE");
            invincibilityTimer = invincibilityCooldown;
            healthPoints--;
            if(collision.gameObject.CompareTag("Enemy projectile"))
                Debug.Log("Balaceado");
            //Destroy(collision.gameObject);
        }
    }

    void ApplyHitEffect(EnemyManager enemy)
    {
        if (enemy == null) return;

        float randomNum = UnityEngine.Random.Range(1.0f, 100.0f);
        int critRoll = Mathf.RoundToInt(randomNum);
        bool isCrit = critRoll < critChance;

        if (isCrit)
            enemy.TakeCriticalDamage(damagePerLevel);
        else
            enemy.TakeDamage(damagePerLevel);

        switch (levelProgressCounter)
        {
            case 1:
                break;
            case 2:
                enemy.ApplySlow(2f, 0.4f);
                break;
            case 3:
                break;
        }
    }

    public void Heal(float amount)
    {
        healthPoints += amount;
        healthPoints = Mathf.Min(healthPoints, maxHP); // No superar el máximo 
        Debug.Log($"Curado! HP actual: {healthPoints}");
    }

    public void DefeatPlayer()
    {
        loadSceneScript.LoadSceneWithName("Defeat screen");
    }

    public void LevelUp()
    {
        level++;
        maxHP += healthPerLevel; // El techo de HP sube con cada nivel
        healthPoints += healthPerLevel; // Al subir de nivel, el jugador también recupera la cantidad de HP que subió el techo, para no quedarse atrás
        damagePerLevel++; 
        exp = 0;
        levelText.text = ("Lvl " + level.ToString()); // Actualiza el texto del nivel en la UI
    }

    void UpdateHealthBar()
    {
        if (healthBar != null) 
            healthBar.fillAmount = healthPoints / maxHP; 
    }
}