using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float moveSpeed = 500f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float destroyTime = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Time.time >= destroyTime)
        {
            Destroy(gameObject);
            return;
        }

        if (rb != null)
        {
            return;
        }

        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            return;
        }

        EnemyHpController enemyController = other.GetComponentInParent<EnemyHpController>();

        if (enemyController != null)
        {
            enemyController.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    public void Initialize(Vector3 direction, float lifeTime)
    {
        if (direction.sqrMagnitude < 0.001f)
        {
            direction = Vector3.forward;
        }

        moveDirection = direction.normalized;
        destroyTime = Time.time + lifeTime;
        transform.forward = moveDirection;

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = false;
            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }
}
