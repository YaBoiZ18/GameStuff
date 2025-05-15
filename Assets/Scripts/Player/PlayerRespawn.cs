using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint; // Stroe last checkpoint here
    private Health playerHealth;
    private UIManager uiManager;

    [Header("Lives System")]
    [SerializeField] private int maxLives = 3; // Total lives
    private int currentLives;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
        currentLives = maxLives;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckRespawn()
    {
        // Decrease a life on each death
        currentLives--;

        // Update the UI
        uiManager.UpdateLives(currentLives);

        // If out of lives, show game over
        if (currentLives <= 0)
        {
            uiManager.GameOver();
            return;
        }

        // Respawn logic
        playerHealth.Respawn();

        if (currentCheckpoint != null)
        {
            // Respawn at the last checkpoint
            transform.position = currentCheckpoint.position;
        }
        else
        {
            // Respawn at the default starting position
            transform.position = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint") 
        {
            currentCheckpoint = collision.transform; // Store as current checkpoint
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; //Deactivate checkpoint collider
            // Add an extra life (up to the maxLives limit)
            currentLives = Mathf.Clamp(currentLives + 1, 0, maxLives);

            // Update the UI to reflect the new life count
            uiManager.UpdateLives(currentLives);
        }
    }
}
