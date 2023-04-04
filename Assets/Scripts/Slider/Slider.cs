using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slider : MonoBehaviour
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
        text.text = ((int)(value * 100f)).ToString();
    }

    public float Get()
    {
        return value;
    }

    public void SetValue(float v)
    {
        value = v;
    }
}
