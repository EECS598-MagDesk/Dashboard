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
        text.text = string.Format("{0:F2}", value * 100f);
    }

    public float Get()
    {
        return value * 100f;
    }

    public void SetValue(float v)
    {
        value = v;
    }
}
