using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed;
    [SerializeField] private Rigidbody2D characterRB;
    private Animator animator;
    public SceneProgressionManager sceneProgressionManager;
    private int levelProgressCounter;
    private Vector2 movementDirection;

    void Start()
    {
        levelProgressCounter = PlayerPrefs.GetInt("LevelProgress", 0);
        characterRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speed = sceneProgressionManager.speedProgression[levelProgressCounter];
    }

    void Update()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        characterRB.linearVelocity = movementDirection * speed;
    }

    void UpdateAnimator()
    {
        if (animator == null) return;

        animator.SetFloat("MoveX", movementDirection.x);
        animator.SetFloat("MoveY", movementDirection.y);
        animator.SetBool("IsMoving", movementDirection.magnitude > 0.1f); 
    }
}