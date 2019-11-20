using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject eButton;
    public bool isChest = false;
    public bool isInteractableOnce = false;
    public bool isCannon = false;
    bool istriggered = false;
    bool isInteracted = false;
    GameObject temp;
    Chest refChest;
    cannonball refCannon;

    

    // Start is called before the first frame update
    void Start()
    {
        if (isChest)
        {
            refChest = this.GetComponentInParent<Chest>();
        }
        if (isCannon)
        {
            refCannon = this.GetComponentInParent<cannonball>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (istriggered)
            {
                Destroy(temp);
                if (isChest)
                {
                    Chest();
                }
                if (isInteractableOnce)
                {
                    isInteracted = true;
                }
                if (isCannon)
                {
                    Cannon();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            istriggered = true;
            if (isChest)
            {
                Chest();
            }
            if (isInteractableOnce && !isInteracted)
            {
                Interact();
            }
            if (isCannon && other.GetComponent<Player>().usingPewter)
            {
                Cannon();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            istriggered = false;
            Destroy(temp);
        }
    }

    private void Chest()
    {
        if (refChest.onCooldown)
        {
            temp = Instantiate(eButton, eButton.transform.position, Quaternion.identity);
            temp.transform.SetParent(GameObject.Find("UI-Canvas").transform, false);
            temp.GetComponentInChildren<Text>().text = "Chest On Cooldown";
        }
        else
        {
            temp = Instantiate(eButton, eButton.transform.position, Quaternion.identity);
            temp.transform.SetParent(GameObject.Find("UI-Canvas").transform, false);
            temp.GetComponentInChildren<Text>().text = "Press [E] To Open Chest";
        }
    }

    private void Interact()
    {
        temp = Instantiate(eButton, eButton.transform.position, Quaternion.identity);
        temp.transform.SetParent(GameObject.Find("UI-Canvas").transform, false);
    }

    private void Cannon()
    {
        if (!refCannon.InHand)
        {
            temp = Instantiate(eButton, eButton.transform.position, Quaternion.identity);
            temp.transform.SetParent(GameObject.Find("UI-Canvas").transform, false);
        }
    }
}
