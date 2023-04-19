using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Switch : MonoBehaviour
{
    private float value;
    public TextMeshPro text;
    private bool prevVal = false;

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
        if (this.Get())
        {
            if (!prevVal)
            {
                PlaySoundEffect();
            }
            text.text = "ON";
            prevVal = true;
        }
        else
        {
            if (prevVal)
            {
                PlaySoundEffect();
            }
            text.text = "OFF";
            prevVal = false;
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
