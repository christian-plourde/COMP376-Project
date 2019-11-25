using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coinshot : MonoBehaviour
{
    int health = 2;
    int faceDirection = 1;

    float cooldown=4;
    float cooldownTimer;

    bool onCooldown;

    public float shotForce=2f;
    public float aimOffsetY = 1.8f;

    bool isDead;
    public GameObject coinprefab;

    public GameObject Target;
    public Transform launchPoint;

    public GameObject RagdollPrefab;

    Animator animator;

    void Start()
    {
        animator=GetComponent<Animator>();
    }

    void Update()
    {

        if (Target != null && !onCooldown)
        {
            animator.SetBool("throwing",true);
            //ThrowCoin();
        }

        if (onCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer > cooldown)
            {
                cooldownTimer = 0f;
                onCooldown = false;
            }
        }


        // rotate:
        if (Target != null)
        {
            if (Target.transform.position.x > transform.position.x && faceDirection==1)
            {
                transform.Rotate(0, 180, 0);
                faceDirection = -1;
            }
            else if (Target.transform.position.x < transform.position.x && faceDirection == -1)
            {
                transform.Rotate(0, 180, 0);
                faceDirection = 1;
            }
        }

    }


    private void ThrowCoin()
    {
        if (Target != null)
        {
            GameObject coin = Instantiate(coinprefab, launchPoint.position, Quaternion.identity);

            //direction:
            Vector3 adjustedTargetPos = new Vector3(Target.transform.position.x, Target.transform.position.y + aimOffsetY, Target.transform.position.z);
            Vector3 shotDirection = (adjustedTargetPos - launchPoint.position).normalized;
            coin.GetComponent<Rigidbody>().AddForce(shotDirection * shotForce, ForceMode.Impulse);
            onCooldown = true;
            animator.SetBool("throwing", false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "JoraFist")
        {
            other.gameObject.SetActive(false);
            health -= 1;
            if (health <= 0)
                KillCoinshot();
        }


        if (other.tag == "JoraFistCombo")
        {
            other.gameObject.SetActive(false);
            health -= 2;
            if (health <= 0)
                KillCoinshot();
        }
    }

    private void KillCoinshot()
    {
        GameObject temp=Instantiate(RagdollPrefab, transform.position, Quaternion.Euler(RagdollPrefab.transform.eulerAngles));
        if (faceDirection == -1)
            temp.transform.Rotate(0, 180, 0);
        Destroy(this.gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Interactable")
        {
            Rigidbody collided = collision.collider.GetComponent<Rigidbody>();
            if (Mathf.Abs((collided.velocity.x)) > 3f || Mathf.Abs((collided.velocity.y)) > 3f || Mathf.Abs((collided.velocity.z)) > 3f)
            {
                health--;
                if (health <= 0)
                    KillCoinshot();
            }
        }

        if (collision.collider.tag == "Spikes")
        {
            KillCoinshot();
        }

       



    }
}
