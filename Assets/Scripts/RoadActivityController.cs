using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadActivityController : MonoBehaviour
{
    public float activeTime = 8f;
    public float breakTime = 4f;

    public float counter = 0f;
    public bool currentlyActive = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += Time.deltaTime;

        if (currentlyActive)
        {
            if (counter > activeTime)
            {
                counter = 0f;
                currentlyActive = false;
                LowActivity();
            }
        }
        else
        {
            if (counter > breakTime)
            {
                counter = 0f;
                currentlyActive = true;
                HighActivity();
            }
        }
    }

    void LowActivity()
    {
        transform.Find("CarToLeft").gameObject.SetActive(false);
        transform.Find("CarToRight").gameObject.SetActive(false);
        transform.Find("TooBusyLeft").gameObject.SetActive(false);
        transform.Find("TooBusyRight").gameObject.SetActive(false);
    }

    void HighActivity()
    {
        transform.Find("CarToLeft").gameObject.SetActive(true);
        transform.Find("CarToRight").gameObject.SetActive(true);
        transform.Find("TooBusyLeft").gameObject.SetActive(true);
        transform.Find("TooBusyRight").gameObject.SetActive(true);
    }
}
