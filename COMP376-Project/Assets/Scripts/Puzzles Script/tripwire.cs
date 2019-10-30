using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tripwire : MonoBehaviour
{
    public int direction;
    public GameObject wire;
    public GameObject arrowPrefab;
    public Transform spawnPoint;
    // Start is called before the first frame update
    private void Start()
    {
        direction = 1;
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("tripwire hit");
        Destroy(wire);

        //instantiate arrow prefab
        GameObject arrow = Instantiate(arrowPrefab,spawnPoint.position,Quaternion.identity);
        arrow.transform.Rotate(new Vector3(0, 0, 90));
        arrow.GetComponent<Rigidbody>().AddForce(new Vector3(1,0,0)*direction*2.5f,ForceMode.Impulse);
    }
}
