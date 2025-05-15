using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header ("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    [Header("Lives")]
    [SerializeField] private UnityEngine.UI.Text livesText; // Reference to the lives text UI

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        UpdateLives(3); // Initialize UI with the maximum lives
    }

    public void UpdateLives(int lives)
    {
        livesText.text = "Lives: " + lives;
    }

    // Activate game over screen
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void Restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayGame() 
    {
        SceneManager.LoadScene(2);
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit(); // Quits game (build only)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Quits game in Unity
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If already paused then unpause
            if (pauseScreen.activeInHierarchy)
            {
                PauseGame(false);
            }
            else 
            {
                PauseGame(true);
            }
        }
    }

    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);

        // If status == true then pause | if status == false then unpause
        Time.timeScale = System.Convert.ToInt32(!status);      
    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }
    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
}
