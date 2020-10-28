using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 8f;
    public float jumpSpeed = 5f;
    bool isGrounded;

    // A threshold of 5 seconds for fear tolerance before the panda gives up
    public float fearThreshold = 8f;

    /* The current meter for fear, we can have this an integer and take the updates of the
     * integer floor of another meter if we want an interactive enter/exit exposure to fear. */
    private float fearMeter = 0f;

    /* Implementation of idea in previous comment for fearMeter */
    private int fearState = 0;

    /* Dummy variable for panda losing the level due to fear? */
    public bool deadPanda = false;

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

        if (Physics.Raycast(transform.position, rayDir, out hit, rayDist) && hit.collider.tag != "Safety" && hit.collider.tag != "FearEntity")
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    // Method to add/remove fear
    private void ModifyFear(float additiveModifier)
    {
        fearMeter += additiveModifier;

        if (additiveModifier > 0)
        {
            if (fearMeter >= fearThreshold && fearState <= 2)
            {
                FearStateTransition("Panda has a lot of fear!");
                deadPanda = true;
                walkSpeed = 0.2f * walkSpeed;
            }
            else if (fearMeter >= 0.66f * fearThreshold && fearState <= 1)
            {
                FearStateTransition("Panda is pretty scared!");
                walkSpeed = (1 - 0.66f) * walkSpeed;
            }
            else if (fearMeter >= 0.33f * fearThreshold && fearState == 0)
            {
                FearStateTransition("Panda is getting anxious!");
                walkSpeed = (1 - 0.33f) * walkSpeed;
            }
        } else if (additiveModifier < 0)
        {
            if (fearMeter < fearThreshold && fearState > 2)
            {
                FearStateTransition("Panda is shivering in safety!", -1);
                walkSpeed = 0.33f*8f;
            }
            else if (fearMeter < 0.5f * fearThreshold && fearState > 1)
            {
                FearStateTransition("Panda is calming down!", -1);
                walkSpeed = 0.66f * 8f;
            }
            else if (fearMeter < 0f && fearState >= 1)
            {
                FearStateTransition("Panda is calm!", -1);
                fearMeter = 0f;
                walkSpeed = 8f;
            }
        }
    }

    private void FearStateTransition(System.String str, int sign = 1)
    {
        UnityEngine.Debug.Log(str);

        if (sign > 0)
        {
            fearState++;
        } else if (sign < 0)
        {
            fearState--;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            GameManager.instance.LevelEnd();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("FearEntity"))
        {
            ModifyFear(Time.deltaTime);
        } else if (other.gameObject.CompareTag("Safety"))
        {
            ModifyFear(-Time.deltaTime);
        }
    }
}
