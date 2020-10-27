using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FearMechanic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().ModifyFear(Time.deltaTime);
        }
    }
}
