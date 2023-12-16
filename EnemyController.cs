using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Animator walkAnimation;

    private GameManager gameManager;

    private GameObject player;

    private Rigidbody enemyRb;

    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        enemyRb = GetComponent<Rigidbody>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {

        if (gameManager.isGameActive)
        {

            Vector3 direction = (player.transform.position - transform.position).normalized;

            enemyRb.AddForce(direction * speed);

            transform.LookAt(player.transform.position);

        }
        else
        {
            walkAnimation.speed = 0;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        }

    }


}
