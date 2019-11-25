using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{

    float deletionTime = 1.8f;
    float counter;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > deletionTime)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().registerHit();
            Destroy(this.gameObject);
        }

    }
}
