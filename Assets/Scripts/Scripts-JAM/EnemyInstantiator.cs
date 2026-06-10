using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyInstantiator : MonoBehaviour
{
    public Rigidbody2D enemyType1;
    public Rigidbody2D enemyType2;

    public int spawnAmountCounter;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnAmountCounter < 10)
        {
            SpawnEnemy(enemyType2);
        }
        else if (spawnAmountCounter < 20)
            SpawnEnemy(enemyType1);
    }

    void SpawnEnemy(Rigidbody2D enemy)
    {
        Rigidbody2D clone;
        clone = Instantiate(enemy, transform.position, transform.rotation);
        spawnAmountCounter++;
    }
}
