using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyInstantiator : MonoBehaviour
{
    public Rigidbody2D enemyType1;
    public Rigidbody2D enemyType2;
    public Rigidbody2D enemyType3;
    public int enemy1spawnCount;
    public int enemy2spawnCount;
    public int enemy3spawnCount;

    public int spawnAmountCounter;
    public float spawnTimer;
    public float spawnTimeDifference;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTimer = spawnTimeDifference;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
             SpawnEnemy(enemyType2, enemy2spawnCount);
             SpawnEnemy(enemyType1, enemy1spawnCount);
        }
    }

    void SpawnEnemy(Rigidbody2D enemy, int spawnCount)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Rigidbody2D clone;
            clone = Instantiate(enemy, transform.position, transform.rotation);
            spawnAmountCounter++;
        }
        spawnTimer = spawnTimeDifference;
    }
}
