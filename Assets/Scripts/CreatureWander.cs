using UnityEngine;

public class CreatureWander : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float wanderRadius = 2f;
    [SerializeField] private float minIdleTime = 1f;
    [SerializeField] private float maxIdleTime = 3f;

    [Header("Optional")]
    [SerializeField] private Animator animator; 

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        PickNewTarget();
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        Vector2 currentPos = transform.position;
        Vector2 direction = (targetPosition - currentPos).normalized;

        transform.position = Vector2.MoveTowards(currentPos, targetPosition, moveSpeed * Time.deltaTime);

        
        if (spriteRenderer != null && direction.x != 0)
            spriteRenderer.flipX = direction.x < 0;

       
        if (Vector2.Distance(currentPos, targetPosition) < 0.05f)
        {
            isMoving = false;
            if (animator) animator.SetBool("IsWalking", false);
            Invoke(nameof(PickNewTarget), Random.Range(minIdleTime, maxIdleTime));
        }
    }

    void PickNewTarget()
    {
        Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
        targetPosition = startPosition + randomOffset;
        isMoving = true;
        if (animator) animator.SetBool("IsWalking", true);
    }
}