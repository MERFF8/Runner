using UnityEngine;
using UnityEngine.Events;

public class EnemyHpController : MonoBehaviour
{
    [Header("HP Settings")]
    [SerializeField] private int maxHp = 3;
    [SerializeField] private int currentHp;
    [SerializeField] private bool destroyOnDeath = true;
    [SerializeField] private float deathDelay = 3f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string deathAnimationBool = "Die";

    [Header("Events")]
    public UnityEvent<int, int> OnHealthChanged;
    public UnityEvent OnEnemyDied;

    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;
    public bool IsDead { get; private set; }

    private void Awake()
    {
        currentHp = maxHp;
        IsDead = false;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHp, maxHp);
    }

    public void TakeDamage(int amount)
    {
        if (IsDead || amount <= 0)
        {
            return;
        }

        currentHp = Mathf.Max(0, currentHp - amount);
        OnHealthChanged?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (IsDead || amount <= 0)
        {
            return;
        }

        currentHp = Mathf.Min(maxHp, currentHp + amount);
        OnHealthChanged?.Invoke(currentHp, maxHp);
    }

    public void ResetHp()
    {
        currentHp = maxHp;
        IsDead = false;

        if (animator != null)
        {
            animator.SetBool(deathAnimationBool, false);
        }

        OnHealthChanged?.Invoke(currentHp, maxHp);
    }

    public void Die()
    {
        if (IsDead)
        {
            return;
        }

        IsDead = true;
        OnEnemyDied?.Invoke();

        Collider[] colliders = GetComponentsInChildren<Collider>(true);
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.detectCollisions = false;
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (animator != null)
        {
            animator.SetBool(deathAnimationBool, true);
        }

        if (destroyOnDeath)
        {
            Destroy(gameObject, deathDelay);
        }
    }
}
