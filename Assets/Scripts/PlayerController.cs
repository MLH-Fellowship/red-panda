using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 8f;
    public float jumpSpeed = 5f;
    bool isGrounded;

    // A threshold of 5 seconds for fear tolerance before the panda gives up
    public float fearThreshold = 5f;

    /* The current meter for fear, we can have this an integer and take the updates of the
     * integer floor of another meter if we want an interactive enter/exit exposure to fear. */
    private float fearMeter = 0f;

    /* Dummy variable for panda losing the level due to fear? */
    public bool deadPanda = false;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        GroundChecker();
        WalkHandler();
        JumpHandler();
        FearHandler();
    }

    void FearHandler()
    { // Handle fear meter too high
        if (fearMeter >= fearThreshold)
        { // Currently a simple log handle that is repeated, prefer a one off broadcast system?
            deadPanda = true;
            Debug.Log("Panda has a lot of fear!");
        } else if (fearMeter >= 0.66f*fearThreshold)
        {
            Debug.Log("Panda is pretty scared!");
        } else if (fearMeter >= 0.33f * fearThreshold)
        {
            Debug.Log("Panda is getting anxious!");
        }
    }

    public void ModifyFear(float additiveModifier)
    {
        fearMeter += additiveModifier;
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
}
