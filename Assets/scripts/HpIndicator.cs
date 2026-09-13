using UnityEngine;
using UnityEngine.UI;

public class HpIndicator : MonoBehaviour
{
    [Header("Target")]
    public PlayerHpController playerHp;
    public EnemyHpController enemyHp;

    [Header("References")]
    private Slider hpSlider;
    private Camera mainCamera;

    [Header("Settings")]
    public bool isPlayerIndicator = false;

    private void Start()
    {
        hpSlider = GetComponent<Slider>();
        mainCamera = Camera.main;

        if (isPlayerIndicator)
        {
            if (playerHp == null)
            {
                playerHp = FindAnyObjectByType<PlayerHpController>();
            }

            if (playerHp != null)
            {
                playerHp.OnHealthChanged.AddListener(UpdateHp);
                UpdateHp(playerHp.CurrentHp, playerHp.MaxHp);
            }
        }
        else
        {
            if (enemyHp == null)
            {
                enemyHp = GetComponentInParent<EnemyHpController>();
            }

            if (enemyHp != null)
            {
                enemyHp.OnHealthChanged.AddListener(UpdateHp);
                UpdateHp(enemyHp.CurrentHp, enemyHp.MaxHp);
            }
        }
    }

    private void LateUpdate()
    {
        if (isPlayerIndicator || mainCamera == null)
        {
            return;
        }

        Vector3 directionToCamera = transform.position - mainCamera.transform.position;

        if (directionToCamera.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(directionToCamera);
        }
    }

    private void UpdateHp(int currentHp, int maxHp)
    {
        bool isVisible = currentHp > 0 && currentHp < maxHp;

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = currentHp;
            hpSlider.gameObject.SetActive(isVisible);
        }

        if (gameObject != null)
        {
            gameObject.SetActive(isVisible);
        }
    }
}
