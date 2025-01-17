using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button retryButton;
    public Image[] livesImg;

    public int lives = 3;
    public int score = 0;
    public float respawnTime = 3.0f;
    public float respawnIgnoreCollisionTime = 3.0f;
    public float flashInterval = 0.5f;

    public void NewGame()
    {
        this.lives = 3;
        this.score = 0;
        this.scoreText.text = "00000";

        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
        foreach(Asteroid ast in asteroids)
        {
            Destroy(ast.gameObject);
        }

        for (int i = 0; i < lives; i++)
        {
            this.livesImg[i].enabled = true;
        }

        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        Respawn();
    }

    public void AsteroidDestroyed(Asteroid asteroid)
    {
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        if(asteroid.size <= 0.75f)
        {
            this.score += 100;
        }else if(asteroid.size <= 1.5f)
        {
            this.score += 50;
        }
        else
        {
            this.score += 25;
        }

        scoreText.text = Mathf.FloorToInt(score).ToString("D5");

    }

    public void PlayerDied()
    {
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();

        this.lives--;
        this.livesImg[lives].enabled = false;

        if(this.lives <= 0)
        {
            //this.livesImg[lives].enabled = false;
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), respawnTime);
        }

    }

    private void Respawn()
    {
        this.player.gameObject.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollision");
        StartCoroutine(InvincibilityCoroutine());
        this.player.gameObject.SetActive(true);

        Invoke("TurnOnCollision", respawnIgnoreCollisionTime); //go back to default setting(layer)
    }

    private void TurnOnCollision()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private IEnumerator InvincibilityCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < respawnIgnoreCollisionTime)
        {
            // Toggle sprite visibility
            player.spriteRenderer.enabled = !player.spriteRenderer.enabled;

            yield return new WaitForSeconds(flashInterval);

            elapsedTime += flashInterval;
        }

        // Ensure sprite is visible at the end
        player.spriteRenderer.enabled = true;
    }

    private void GameOver()
    {
        this.player.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        FindObjectOfType<AudioManager>().Play("gameOver");
    }
}
