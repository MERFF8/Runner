using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody rb;

    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public Vector3 bulletSpawnPosition = new Vector3(0f, 2f, 1f);
    public float bulletLifeTime = 2f;
    public Transform turret;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!GameController.isGameStarted || GameController.isGameOver || GameController.isGameWon)
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
            }

            return;
        }

        rb.linearVelocity = new Vector3(0f, 0f, moveSpeed);
    }

    private void Update()
    {
        if (!GameController.isGameStarted || GameController.isGameOver || GameController.isGameWon)
        {
            return;
        }

        if (GameController.Instance != null && transform.position.z >= GameController.Instance.roadEndZ)
        {
            GameController.Instance.FinishGame(true);
            return;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            ShootAtPointer(Mouse.current.position.ReadValue());
            return;
        }

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            ShootAtPointer(Touchscreen.current.primaryTouch.position.ReadValue());
        }
    }

    private void ShootAtPointer(Vector2 screenPosition)
    {
        if (bulletPrefab == null || Camera.main == null)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        Vector3 targetPoint = ray.origin + ray.direction * 50f;

        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }

        Vector3 spawnPosition = transform.position + bulletSpawnPosition;
        Vector3 direction = targetPoint - spawnPosition;

        if (direction.sqrMagnitude < 0.001f)
        {
            direction = Vector3.forward;
        }

        RotateTurretTowards(direction);

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();

        if (bulletController != null)
        {
            bulletController.Initialize(direction, bulletLifeTime);
        }
    }

    private void RotateTurretTowards(Vector3 direction)
    {
        if (turret == null)
        {
            return;
        }

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Vector3 horizontalDirection = direction;
        horizontalDirection.y = 0f;

        if (horizontalDirection.sqrMagnitude < 0.001f)
        {
            return;
        }

        float yaw = Mathf.Atan2(horizontalDirection.x, horizontalDirection.z) * Mathf.Rad2Deg;

        turret.localRotation = Quaternion.Euler(0f, yaw, 0f);
    }
}
