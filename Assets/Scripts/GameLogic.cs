using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject enemySpawner;
    public Image ammo;
    public Image cleanFurniture;
    public Image furnitureClean;
    public Image furnitureClean2;
    public Image kick;
    public Image reload;
    public Image shoot;
    public static bool alreadyTutorialed = false;
    private bool isGameOver = false;
    private bool isTutorial = true;

    private void Start()
    {
        // Clear the game over flag.
        isGameOver = false;
        // Initially, the game over screen should be disabled.
        gameOverScreen.SetActive(false);
        if (alreadyTutorialed)
        {
            enemySpawner.SetActive(true);
            isTutorial = false;
            alreadyTutorialed = true;
        } else
        {
            StartCoroutine(tutorial());
        }
    }

    private void Update()
    {
        if (isTutorial)
        {
            return;
        }
        // Check if the game is over and the player presses 'R' to restart.
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            StartGame();
        }
    }

    IEnumerator tutorial()
    {
        kick.enabled = true;
        shoot.enabled = true;

        yield return new WaitForSeconds(5f);

        kick.enabled = false;
        shoot.enabled = false;

        ammo.enabled = true;
        reload.enabled = true;

        yield return new WaitForSeconds(10f);

        ammo.enabled = false;
        reload.enabled = false;

        furnitureClean.enabled = true;
        furnitureClean2.enabled = true;

        yield return new WaitForSeconds(5f);

        furnitureClean.enabled = false;
        furnitureClean2.enabled = false;

        cleanFurniture.enabled = true;
        enemySpawner.SetActive(true);
        isTutorial = false;
        alreadyTutorialed = true;
        yield return new WaitForSeconds(2f);

        cleanFurniture.enabled = false;

        yield return null;
    }

    public void GameOver()
    {
        // Find the "Game Over Screen" GameObject and enable it.
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);

            // Pause the game by freezing time.
            Time.timeScale = 0;

            // Set the game over flag.
            isGameOver = true;
        }
    }

    public void StartGame()
    {
        // Disable the game over screen.
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);

            // Resume the game by setting time scale back to 1.
            Time.timeScale = 1;
        }

        // Load the gameplay scene to restart the game.
        SceneManager.LoadScene("Dungeon");


    }
}
