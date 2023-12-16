using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : MonoBehaviour
{

    private GameManager gameManager;

    public Transform spawnPoint;

    public GameObject arrow;

    public float shootSpeed = 15;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.Space) && gameManager.isGameActive)
        {
            Instantiate(arrow, spawnPoint.transform.position, spawnPoint.transform.rotation);

            FindObjectOfType<ArrowController>().GetComponent<Rigidbody>().AddForce(spawnPoint.transform.forward * shootSpeed);

        }
    }

}
