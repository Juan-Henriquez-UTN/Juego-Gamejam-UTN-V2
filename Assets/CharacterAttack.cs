using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public CharacterCombat characterCombat;

    private SpriteRenderer m_spriteRenderer;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && characterCombat.damageTimer <= 0)
        {
            m_spriteRenderer.enabled = true;
            characterCombat.ApplyHitEffect(other.GetComponent<EnemyManager>());
            characterCombat.damageTimer = characterCombat.damageCooldown;
        }
    }
}
