using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderBlock : MonoBehaviour
{
    private float limit = 3;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        
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
        slider.SetValue((transform.localPosition.x + limit) / (2 * limit));
    }

    private void OnTriggerStay(Collider other)
    {
        float colliderX = other.transform.position.x;
        if (colliderX > limit || colliderX < -limit)
        {
            
        }
        else
        {
            transform.position = new Vector3(colliderX, transform.position.y, transform.position.z);
        }
    }
}
