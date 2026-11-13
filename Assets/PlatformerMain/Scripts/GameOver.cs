using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject defeatPanel;

    private bool gameEnded;

    private void Start()
    {
        defeatPanel.SetActive(false);
    }

    public void Lose()
    {
        if (gameEnded)
            return;

        gameEnded = true;
        defeatPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(gameObject.scene.buildIndex);
    }
}