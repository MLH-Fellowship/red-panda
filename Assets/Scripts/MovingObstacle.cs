using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float speed = 20f;

    public int direction = 1;

    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        transform.localPosition += new Vector3(Time.deltaTime * speed * direction, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EntitySpawner"))
        {
            speed = UnityEngine.Random.Range(15f, 24f);
            transform.position = initialPosition;
        }
    }
}
