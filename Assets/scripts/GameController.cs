using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    public GameObject groundPrefab;
    public GameObject enemyPrefab;
    public Transform groundParent;
    public Transform enemyParent;

    public int minEnemiesPerBatch = 3;
    public int maxEnemiesPerBatch = 6;
    public float enemyBatchInterval = 25f;
    public float enemySpawnSquareSize = 15f;
    public float enemyMinDistance = 2f;

    public float roadEndZ = 1500f;

    public static bool isGameStarted = false;
    public static bool isGameOver = false;
    public static bool isGameWon = false;

    private UIContoller uiController;

    private void Awake()
    {
        Instance = this;
        isGameStarted = false;
        isGameOver = false;
        isGameWon = false;
    }

    public void StartGame()
    {
        if (isGameStarted || isGameOver || isGameWon)
        {
            return;
        }

        isGameStarted = false;
        isGameOver = false;
        isGameWon = false;

        StartCoroutine(GenerateGround());
        StartCoroutine(GenerateEnemies());
    }

    private void Start()
    {
        uiController = FindAnyObjectByType<UIContoller>();
        StartGame();
    }

    public void FinishGame(bool playerWon)
    {
        if (isGameOver || isGameWon)
        {
            return;
        }

        isGameStarted = false;
        isGameWon = playerWon;
        isGameOver = !playerWon;

        if (uiController == null)
        {
            uiController = FindAnyObjectByType<UIContoller>();
        }

        if (uiController != null)
        {
            if (playerWon)
            {
                uiController.ShowWin();
            }
            else
            {
                uiController.ShowGameOver();
            }
        }
    }

    private IEnumerator GenerateGround()
    {
        for (int i = 0; i < 20; i++)
        {
            Instantiate(groundPrefab, new Vector3(0f, 0f, 75f + i * 75f), Quaternion.identity, groundParent);
            yield return new WaitForSeconds(0.01f);
        }

    }

    private IEnumerator GenerateEnemies()
    {
        for (int i = 0; i < 20; i++)
        {
            SpawnEnemyBatch(25f + i * enemyBatchInterval);
            yield return new WaitForSeconds(0.01f);
        }

        isGameStarted = true;
    }

    private void SpawnEnemyBatch(float centerZ)
    {
        if (enemyPrefab == null)
        {
            return;
        }

        int enemyCount = Random.Range(minEnemiesPerBatch, maxEnemiesPerBatch + 1);
        float halfSize = enemySpawnSquareSize * 0.5f;
        List<Vector3> spawnPositions = new List<Vector3>();

        while (spawnPositions.Count < enemyCount)
        {
            float randomX = Random.Range(-halfSize, halfSize);
            float randomZ = centerZ + Random.Range(-halfSize, halfSize);
            Vector3 candidate = new Vector3(randomX, 0f, randomZ);

            bool tooClose = false;
            for (int i = 0; i < spawnPositions.Count; i++)
            {
                if (Vector3.Distance(candidate, spawnPositions[i]) < enemyMinDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
            {
                spawnPositions.Add(candidate);
            }
        }

        for (int i = 0; i < spawnPositions.Count; i++)
        {
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            Instantiate(enemyPrefab, spawnPositions[i], randomRotation, enemyParent);
        }
    }
}
