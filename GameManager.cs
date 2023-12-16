using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public ParticleSystem explosion;

    public TextMeshProUGUI scoreText;

    private Animator deathAnimation;

    public AudioClip clickSound;
    public AudioClip zombieDeathSound;
    public AudioClip powerUpSound;
    public AudioClip deactivatePowerUp;

    public AudioSource audioSource;

    public GameObject gameOverScene;
    public GameObject Crossbow;
    public GameObject playerUI;
    public GameObject powerUp;
    public GameObject healthPowerUp;
    public GameObject indicator;

    public GameObject[] spawnPoints;
    public GameObject[] enemies;
    public GameObject[] healthPoints;

    private PlayerController player;

    private Vector3 offset = new Vector3(0, 0.2f, 0);

    private float waitTime = 2;
    private float scoreToAdd = 10;
    private float score = 0;
    private float powerUpWaitTime = 45;
    private float healthPowerUpTime = 30;
    private float xPowerUpRange = 10;
    private float zPowerUpRange = 10;
    private float powerUpLength = 5;
    private float powerUpCount = 0;

    public int hearts = 3;

    public bool isGameActive = false;
    public bool powerUpAcitve = false;
    public bool healthPowerUpActive = false;


    // Start is called before the first frame update
    void Start()
    {
        deathAnimation = GameObject.Find("Player").GetComponent<Animator>();

        player = GameObject.Find("Player").GetComponent<PlayerController>();

        PlayerController.OnPowerUp += ActivatePowerUp;
        PlayerController.Damage += HeartCheck;
        PlayerController.ZombieDamage += PlayZombieDeathSound;
        PlayerController.ZombieDamage += UpdateScore;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnemySpawn()
    {
        while(isGameActive)
        {
            yield return new WaitForSeconds(waitTime);

            Instantiate(enemies[0], spawnPoints[RandomSpawnPoint()].transform.position + offset, enemies[0].transform.rotation);
        }
    }

    void GameOver()
    {
        isGameActive = false;

        player.DeathExplosion();

        deathAnimation.SetBool("Death_b", true);
        Crossbow.SetActive(false);

        gameOverScene.SetActive(true);
      
    }

    int RandomSpawnPoint()
    {
        return Random.Range(0, 5);
    }

    public void HeartCheck()
    {
        if (hearts == 0)
        {
            GameOver();
            healthPoints[2].SetActive(false);
        }
        else
        {
            if(hearts>0)
            healthPoints[2 - hearts].SetActive(false);
        }

    }

    public void StartGame(float difficulty)
    {
        waitTime /= difficulty;

        healthPowerUpTime /= difficulty;
        powerUpWaitTime /= difficulty;

        isGameActive = true;

        scoreToAdd *= difficulty;

        audioSource.PlayOneShot(clickSound, 5f);

        StartCoroutine(EnemySpawn());
        StartCoroutine(PowerUp());
        StartCoroutine(HealthPowerUp());

        scoreText.text = "Score " + score;

        playerUI.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        audioSource.PlayOneShot(clickSound, 5f);
    }

    public void EndGame()
    {
        Debug.Log("Quit");

        audioSource.PlayOneShot(clickSound, 5f);

        Application.Quit();

    }

    public void PlayZombieDeathSound()
    {
        audioSource.PlayOneShot(zombieDeathSound, 1f);
    }

    public void ZombieDeathParticle(Vector3 position, Quaternion rotation)
    {
        Instantiate(explosion, position, rotation);
    }

    public void UpdateScore()
    {
        score += scoreToAdd;

        scoreText.text = "Score " + score;
    }

    IEnumerator PowerUp()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(powerUpWaitTime);

            Instantiate(powerUp, GeneratePowerUpRandomPosition(), powerUp.transform.rotation);

        }

    }

    IEnumerator HealthPowerUp()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(healthPowerUpTime);

            Instantiate(healthPowerUp, GeneratePowerUpRandomPosition(), healthPowerUp.transform.rotation);

        }
    }

    Vector3 GeneratePowerUpRandomPosition()
    {
        return new Vector3(Random.Range(-xPowerUpRange, xPowerUpRange), 1, Random.Range(-zPowerUpRange, zPowerUpRange));
    }

    public void AddHealth()
    {
        if(hearts<3)
        {
            healthPoints[2-hearts].SetActive(true);
            hearts++;
        }
    }

    public void ActivatePowerUp()
    {
        if (powerUpAcitve == true)
        {
            powerUpCount++;
            audioSource.PlayOneShot(powerUpSound, 0.6f);
            StartCoroutine(DeactivatePowerUp());
        }
        else
        {
            powerUpAcitve = true;
            audioSource.PlayOneShot(powerUpSound, 0.6f);
            indicator.SetActive(true);
            StartCoroutine(DeactivatePowerUp());
        }
    }

    IEnumerator DeactivatePowerUp()
    {
        yield return new WaitForSeconds(powerUpLength);
        if (powerUpCount > 0)
                powerUpCount--;
        else
        {
            powerUpAcitve = false;
            audioSource.PlayOneShot(deactivatePowerUp, 0.6f);
            indicator.SetActive(false);
        }

    }

    public void ActivateHealtPowerUp()
    {
        audioSource.PlayOneShot(powerUpSound, 0.6f);
    }

}
