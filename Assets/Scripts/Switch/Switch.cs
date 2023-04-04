using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Switch : MonoBehaviour
{
    private float value;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
