using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Zombie"))
        {
            gameManager.PlayZombieDeathSound();
            gameManager.UpdateScore();
            gameManager.ZombieDeathParticle(other.transform.position, other.transform.rotation);

            Destroy(gameObject);
            Destroy(other.gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

}
