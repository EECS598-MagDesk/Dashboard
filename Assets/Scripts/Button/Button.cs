using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Button : MonoBehaviour
{
    private float prevHeight = 0f;
    public GameObject key;
    private float triggerVal = 0.3f;
    private float keyOriHeight = 0f;
    private bool triggered = false;
    public string keyText = "A";
    public TextMeshPro text;

    public bool isKeyboardButton = false;
    public Keyboard keyboard;

    public AudioClip soundEffect;

    // Start is called before the first frame update
    void Start()
    {
        keyOriHeight = key.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = keyText;
    }

    public bool Get()
    {
        return triggered;
    }

    private void PlaySoundEffect()
    {
        // Play the sound effect using AudioSource
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        prevHeight = other.transform.position.y;
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 keyPos = key.transform.position;
        if ((prevHeight - other.transform.position.y) > triggerVal && !triggered)
        {
            triggered = true;
            if (isKeyboardButton)
            {
                keyboard.type(keyText);
            }
            PlaySoundEffect();
        }
        key.transform.position = new Vector3(keyPos.x, keyOriHeight - (prevHeight - other.transform.position.y), keyPos.z);
        //Debug.Log(other.transform.position.y);
    }

    private void OnTriggerExit(Collider other)
    {
        /*
        if (isKeyboardButton && triggered)
        {
            keyboard.type(keyText);
        }
        */
        triggered = false;
        Vector3 keyPos = key.transform.position;
        key.transform.position = new Vector3(keyPos.x, keyOriHeight, keyPos.z);
    }

}
