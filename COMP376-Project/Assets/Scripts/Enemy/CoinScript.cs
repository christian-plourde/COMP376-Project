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
            Vector3 velocity = GetComponent<Rigidbody>().velocity;
            //Debug.Log("Hit at velocity: "+velocity);

            if(Mathf.Abs(velocity.x) > 5 || Mathf.Abs(velocity.y) > 5 || Mathf.Abs(velocity.z) > 5)
               GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().registerHit();
            Destroy(this.gameObject);
        }

    }
}
