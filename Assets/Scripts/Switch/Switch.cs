using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Switch : MonoBehaviour
{
    private float value;
    public TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.Get())
        {
            text.text = "ON";
        }
        else
        {
            text.text = "OFF";
        }
        
    }

    public bool Get()
    {
        return value > 0.5;
    }

    public void SetValue(float v)
    {
        value = v;
    }
}
