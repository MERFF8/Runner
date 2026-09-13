using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlayerHpController : MonoBehaviour
{
    [Header("HP Settings")]
    [SerializeField] private int maxHp = 7;
    [SerializeField] private int currentHp;
    [SerializeField] private float damageInterval = 1f;

    [Header("Events")]
    public UnityEvent<int, int> OnHealthChanged;
    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;

    private readonly HashSet<EnemyHpController> enemiesTouchingPlayer = new HashSet<EnemyHpController>();
    private float nextDamageTime;

    private void Awake()
    {
        currentHp = maxHp;
    }

    private void Update()
    {
        if (!GameController.isGameStarted || GameController.isGameOver || GameController.isGameWon)
        {
            return;
        }

        if (currentHp <= 0 || enemiesTouchingPlayer.Count == 0)
        {
            return;
        }

        if (Time.time >= nextDamageTime)
        {
            foreach (EnemyHpController enemyHp in enemiesTouchingPlayer)
            {
                if (enemyHp != null && !enemyHp.IsDead)
                {
                    TakeDamage(1);
                    nextDamageTime = Time.time + damageInterval;
                    break;
                }
            }
        }

        var enemiesToRemove = new List<EnemyHpController>();

        foreach (EnemyHpController enemyHp in enemiesTouchingPlayer)
        {
            if (enemyHp == null || enemyHp.IsDead)
            {
                enemiesToRemove.Add(enemyHp);
            }
        }

        foreach (EnemyHpController enemyHp in enemiesToRemove)
        {
            enemiesTouchingPlayer.Remove(enemyHp);
        }
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHp, maxHp);
    }

    public void TakeDamage(int amount)
    {
        if (currentHp <= 0 || amount <= 0)
        {
            return;
        }

        currentHp = Mathf.Max(0, currentHp - amount);
        OnHealthChanged?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            GameOver();
        }
    }

    public void ResetHp()
    {
        currentHp = maxHp;
        OnHealthChanged?.Invoke(currentHp, maxHp);
    }

    private void GameOver()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.FinishGame(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        RegisterEnemy(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        UnregisterEnemy(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        RegisterEnemy(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        UnregisterEnemy(other.gameObject);
    }

    private void RegisterEnemy(GameObject hitObject)
    {
        if (hitObject == null || !GameController.isGameStarted || GameController.isGameOver || GameController.isGameWon)
        {
            return;
        }

        EnemyHpController enemyHp = hitObject.GetComponentInParent<EnemyHpController>();

        if (enemyHp == null || enemyHp.IsDead)
        {
            return;
        }

        if (enemiesTouchingPlayer.Add(enemyHp))
        {
            TakeDamage(1);
            nextDamageTime = Time.time + damageInterval;
        }
    }

    private void UnregisterEnemy(GameObject hitObject)
    {
        if (hitObject == null)
        {
            return;
        }

        EnemyHpController enemyHp = hitObject.GetComponentInParent<EnemyHpController>();

        if (enemyHp != null)
        {
            enemiesTouchingPlayer.Remove(enemyHp);
        }
    }
}
