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
    public bool isSatchel = false;
    public bool isCheckpoint = false;
    bool istriggered = false;
    bool isInteracted = false;
    GameObject temp;
    Chest refChest;
    cannonball refCannon;
    Satchel refSatchel;

    

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

        if(isSatchel)
        {
            refSatchel = this.GetComponentInParent<Satchel>();
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
                if (isChest && GameObject.FindGameObjectsWithTag("Note") == null)
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
                if (isSatchel && GameObject.FindGameObjectsWithTag("Note") == null)
                {
                    Satchel();
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
            if (isSatchel)
            {
                Satchel();
            }
            if(isCheckpoint)
            {
                Checkpoint();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player" && !isCheckpoint)
        {
            istriggered = false;
            Destroy(temp);
        }
    }

    private void Satchel()
    {
        if (refSatchel.onCooldown)
        {
            temp = Instantiate(eButton, eButton.transform.position, Quaternion.identity);
            temp.transform.SetParent(GameObject.Find("UI-Canvas").transform, false);
            temp.GetComponentInChildren<Text>().text = "Satchel On Cooldown";
        }
        else
        {
            temp = Instantiate(eButton, eButton.transform.position, Quaternion.identity);
            temp.transform.SetParent(GameObject.Find("UI-Canvas").transform, false);
            temp.GetComponentInChildren<Text>().text = "Press [E] To Open Satchel";
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

    private void Checkpoint()
    {
        temp = Instantiate(eButton, eButton.transform.position, Quaternion.identity);
        temp.transform.SetParent(GameObject.Find("UI-Canvas").transform, false);
        temp.GetComponentInChildren<Text>().text = "CheckPoint Saved";
        Destroy(temp, 3);
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
