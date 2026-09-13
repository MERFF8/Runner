using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    public Transform player;

    public float chaseRadius = 25f;
    public float moveSpeed = 2.5f;
    public Animator animator;
    public string runBool = "Run";
    public string deathBool = "Die";

    private Rigidbody rb;
    private EnemyHpController enemyController;
    private bool isDeathAnimationStarted;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemyController = GetComponent<EnemyHpController>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void Start()
    {
        SetAnimationState(false);
    }

    private void FixedUpdate()
    {
        if (!GameController.isGameStarted || GameController.isGameOver || GameController.isGameWon)
        {
            if (rb != null)
            {
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }

            SetAnimationState(false);
            return;
        }

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                SetAnimationState(false);
                return;
            }
        }

        if (enemyController != null && enemyController.IsDead)
        {
            SetDeathAnimation();
            return;
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        float distanceToPlayer = direction.magnitude;
        bool shouldChase = distanceToPlayer <= chaseRadius;

        if (shouldChase)
        {
            direction.Normalize();
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

            if (rb != null)
            {
                rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.fixedDeltaTime);
            }

            SetAnimationState(true);
        }
        else
        {
            if (rb != null)
            {
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }

            SetAnimationState(false);
        }
    }

    private void SetAnimationState(bool running)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(runBool, running);
    }

    private void SetDeathAnimation()
    {
        if (animator == null || isDeathAnimationStarted)
        {
            return;
        }

        isDeathAnimationStarted = true;
        animator.SetBool(deathBool, true);
        animator.SetBool(runBool, false);
    }
}
