using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 8f;
    public float jumpSpeed = 5f;
    bool isGrounded;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Time.timeScale = 1.0f;
    }

    void Start()
    {
        if (GameManager.instance.paused)
            GameManager.instance.TogglePauseGame();
    }

    void FixedUpdate()
    {
        if (GameManager.instance.paused)
        {
            return;
        }
        GroundChecker();
        WalkHandler();
        JumpHandler();
    }

    void WalkHandler() 
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(hAxis * walkSpeed, rb.velocity.y, vAxis * walkSpeed);
        rb.velocity = movement;
    }

    void JumpHandler() 
    {
        float jAxis = Input.GetAxis("Jump");

        if (jAxis > 0f && this.isGrounded)
        {
            Vector3 jumpVector = new Vector3(0f, jumpSpeed, 0f);
            rb.AddForce(jumpVector, ForceMode.Impulse);
        }
    }

    void GroundChecker()
    {
        RaycastHit hit;
        float rayDist = 0.65f;
        Vector3 rayDir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, rayDir, out hit, rayDist))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            GameManager.instance.LevelEnd();
        }
    }
}
