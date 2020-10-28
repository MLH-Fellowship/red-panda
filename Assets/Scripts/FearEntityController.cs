using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearEntityController : MonoBehaviour
{
    public float speed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.localPosition += new Vector3(transform.localPosition.z * Time.deltaTime * speed, 0, -transform.localPosition.x * Time.deltaTime * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
