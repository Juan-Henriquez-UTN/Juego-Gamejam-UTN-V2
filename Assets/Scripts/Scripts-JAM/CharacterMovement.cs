using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed;
    [SerializeField] private Rigidbody2D characterRB;
    public SceneProgressionManager sceneProgressionManager;
    private int levelProgressCounter;
    private Vector2 movementDirection;

    void Start()
    {
        levelProgressCounter = PlayerPrefs.GetInt("LevelProgress", 0);
        characterRB = GetComponent<Rigidbody2D>();
        speed = sceneProgressionManager.speedProgression[levelProgressCounter];
    }

    void Update()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        characterRB.linearVelocity = movementDirection * speed; // FIX: sacado Time.deltaTime, linearVelocity ya es unidades/segundo
    }
}