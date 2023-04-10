using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBlock : MonoBehaviour
{
    private float limit = 1;
    public Switch switchObj;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.localPosition.x > 0)
        {
            transform.localPosition = new Vector3(limit, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            transform.localPosition = new Vector3(-limit, transform.localPosition.y, transform.localPosition.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.x > limit)
        {
            transform.localPosition = new Vector3(limit, transform.localPosition.y, transform.localPosition.z);
        }  
        else if (transform.localPosition.x < -limit)
        {
            transform.localPosition = new Vector3(-limit, transform.localPosition.y, transform.localPosition.z);
        }
        switchObj.SetValue((transform.localPosition.x + limit) / (2 * limit));
    }

    private void OnTriggerStay(Collider other)
    {
        float colliderX = other.transform.position.x;
        if (colliderX > transform.position.x + limit || colliderX < transform.position.x - limit)
        {
            
        }
        else
        {
            transform.position = new Vector3(colliderX, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (transform.localPosition.x > 0)
        {
            transform.localPosition = new Vector3(limit, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            transform.localPosition = new Vector3(-limit, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
