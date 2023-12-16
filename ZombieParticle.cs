using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieParticle : MonoBehaviour
{
    private float waitTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AutoDelete());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator AutoDelete()
    {
        yield return new WaitForSeconds(waitTime);

        Destroy(gameObject);
    }

}
