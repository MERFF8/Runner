using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIContoller : MonoBehaviour
{
    public TMP_Text LoadingText;
    public GameObject LoadingScreen;
    public GameObject gameOverPopup;
    public GameObject winPopup;
    public Slider levelProgressSlider;

    private float nextTextUpdateTime;
    private int loadingState;
    private readonly string[] loadingStates = { "Loading", "Loading.", "Loading..", "Loading..." };

    private void Start()
    {
        HidePopups();
        UpdateLoadingText();
    }

    private void Update()
    {
        UpdateLevelProgressBar();

        if (GameController.isGameStarted)
        {
            if (LoadingScreen != null)
            {
                LoadingScreen.SetActive(false);
            }

            return;
        }

        if (GameController.isGameOver || GameController.isGameWon)
        {
            if (LoadingScreen != null)
            {
                LoadingScreen.SetActive(false);
            }

            if (WasTapOrClickPressed())
            {
                RestartGame();
            }

            return;
        }

        if (Time.time >= nextTextUpdateTime)
        {
            nextTextUpdateTime = Time.time + 0.5f;
            UpdateLoadingText();
        }
    }

    public void ShowGameOver()
    {
        HidePopups();
        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(true);
        }
    }

    public void ShowWin()
    {
        HidePopups();
        if (winPopup != null)
        {
            winPopup.SetActive(true);
        }
    }

    public void HidePopups()
    {
        if (gameOverPopup != null)
        {
            gameOverPopup.SetActive(false);
        }

        if (winPopup != null)
        {
            winPopup.SetActive(false);
        }
    }

    private void UpdateLevelProgressBar()
    {
        if (levelProgressSlider == null)
        {
            return;
        }

        GameController gameController = GameController.Instance;
        if (gameController == null)
        {
            return;
        }

        levelProgressSlider.maxValue = gameController.roadEndZ;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            return;
        }

        float playerProgress = Mathf.Clamp(player.transform.position.z, 0f, gameController.roadEndZ);
        levelProgressSlider.value = playerProgress;
    }

    private bool WasTapOrClickPressed()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            return true;
        }

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            return true;
        }

        return false;
    }

    public void RestartGame()
    {
        GameController.isGameStarted = false;
        GameController.isGameOver = false;
        GameController.isGameWon = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateLoadingText()
    {
        if (LoadingText == null)
        {
            return;
        }

        loadingState = (loadingState + 1) % loadingStates.Length;
        LoadingText.text = loadingStates[loadingState];
    }
}
