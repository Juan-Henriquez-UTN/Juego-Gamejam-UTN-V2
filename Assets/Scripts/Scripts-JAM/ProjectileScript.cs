using Unity.VisualScripting;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject player;
    public CharacterCombat playerStats;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerStats = player.GetComponent<CharacterCombat>();
    }

    private void OnTriggerEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerStats.invincibilityTimer <= 0)
        {
            Debug.Log("Choque");
            playerStats.invincibilityTimer = playerStats.invincibilityCooldown;
            playerStats.healthPoints--;
            Destroy(gameObject);
        }
    }

}
