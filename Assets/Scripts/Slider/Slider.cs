using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Slider : MonoBehaviour
{
    private float value;
    public TextMeshPro text;
    private float prevVal = 0f;

    public AudioClip soundEffect;

    private void PlaySoundEffect()
    {
        // Play the sound effect using AudioSource
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(prevVal - value) > 0.01f)
        {
            PlaySoundEffect();
            prevVal = value;
        }
        text.text = string.Format("{0}", Mathf.RoundToInt(value * 100f));
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
