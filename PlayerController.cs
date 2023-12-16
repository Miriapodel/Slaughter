using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void PowerUpActivation();

    public delegate void onPowerUpCollison();

    public delegate void offPowerUpCollision();

    public static event PowerUpActivation OnPowerUp;

    public static event offPowerUpCollision Damage;

    public static event onPowerUpCollison ZombieDamage;

    public ParticleSystem explosion;

    public AudioClip hurtSound;

    public AudioSource audioSource;

    private GameManager gameManager;

    private Rigidbody playerRb;

    public Animator runAnimation;
  
    Vector3 setBackZ = new Vector3(0, 0, 0.4f);
    Vector3 setBackX = new Vector3(0.4f, 0, 0);

    public float speed = 10;
    public float rotationSpeed = 15;
    public float playerBoundary = 15;
    public float pushBackForce = 15;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        PlayerBoundary();
    }

    void Movement()
    {

        if (gameManager.isGameActive)
        {

            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");

            if (verticalInput > 0)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime);

                runAnimation.SetFloat("Speed_f", 0.6f);
                runAnimation.SetBool("Static_b", true);
            }
            else
                if (verticalInput<0)
                {
                    transform.Translate(Vector3.back * speed * Time.deltaTime);
                    runAnimation.SetFloat("Speed_f", 0.6f);
                    runAnimation.SetBool("Static_b", true);
            }
                    else
                        {
                            runAnimation.SetFloat("Speed_f", 0);
                            runAnimation.SetBool("Static_b", false);
                        }

            playerRb.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime * horizontalInput);

        }
        else
        {
            runAnimation.SetFloat("Speed_f", 0);
            runAnimation.SetBool("Static_b", false);
        }
    }

    void PlayerBoundary()
    {
        if (transform.position.z > playerBoundary)
        {
            transform.position -= setBackZ;
        }
        else
            if (transform.position.z < -playerBoundary)
        {
            transform.position += setBackZ;
        }

        if (transform.position.x > playerBoundary)
        {
            transform.position -= setBackX;
        }
        else
                if (transform.position.x < -playerBoundary)
        {
            transform.position += setBackX;
        }


    }


    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {

            if (gameManager.powerUpAcitve == false)
            {
                other.gameObject.GetComponent<Rigidbody>().AddForce(other.transform.forward * -pushBackForce);

                gameManager.hearts--;

                if (Damage != null)
                    Damage();
                

                audioSource.PlayOneShot(hurtSound, 0.5f);
            }
            else
            {

                if (ZombieDamage != null)
                    ZombieDamage();
                
                gameManager.ZombieDeathParticle(other.transform.position, other.transform.rotation);

                Destroy(other.gameObject);
            }
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PowerUp"))
        {
            Destroy(other.gameObject);
            if (OnPowerUp != null)
                OnPowerUp();
        }
        else
            if(other.gameObject.CompareTag("HealthPowerUp"))
        {
            Destroy(other.gameObject);
            gameManager.AddHealth();
            gameManager.ActivateHealtPowerUp();
        }
    }

    public void DeathExplosion()
    {
        explosion.Play();
    }

}
