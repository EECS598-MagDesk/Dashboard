using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Knob : MonoBehaviour
{

    private float prevAngle = 0f;
    private float keyOriAngle = 0f;
    private float value = 0f;

    private float upperLimit = 330f;
    private float middleLimit = 350f;
    private float lowerLimit = 10f;

    public TextMeshPro text;

    public AudioClip soundEffect;
    private float prevVal = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void PlaySoundEffect()
    {
        // Play the sound effect using AudioSource
        AudioSource.PlayClipAtPoint(soundEffect, transform.position);
    }

    static float Clamp360(float eulerAngles)
    {
        float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
        if (result < 0)
        {
            result += 360f;
        }
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(prevVal - value) > 0.01f)
        {
            PlaySoundEffect();
            prevVal = value;
        }
        //Debug.Log(transform.eulerAngles.y);
        if (transform.eulerAngles.y > upperLimit && transform.eulerAngles.y < middleLimit)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, upperLimit, transform.eulerAngles.z);
        }
        else if (transform.eulerAngles.y < lowerLimit || transform.eulerAngles.y > middleLimit)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, lowerLimit, transform.eulerAngles.z);
        }
        text.text = string.Format("{0:F2}", this.Get());
    }

    public float Get()
    {
        return value * 100f;
    }

    private float CalAngle(Vector3 ob, Vector3 ot)
    {
        return Mathf.Atan2((ot.x - ob.x), (ot.z - ob.z)) / Mathf.PI * 180;
    }

    private void OnTriggerEnter(Collider other)
    {
        prevAngle = CalAngle(transform.position, other.transform.position);
        PlaySoundEffect();
    }

    private void OnTriggerStay(Collider other)
    {
        float localAngle = CalAngle(transform.position, other.transform.position);
        //Debug.Log(localAngle);
        transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, keyOriAngle + (localAngle - prevAngle), transform.rotation.eulerAngles.x);
        if (transform.eulerAngles.y > upperLimit)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, upperLimit, transform.eulerAngles.z);
        }
        else if (transform.eulerAngles.y < lowerLimit)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, lowerLimit, transform.eulerAngles.z);
        }
        value = (transform.eulerAngles.y - lowerLimit) / (upperLimit - lowerLimit);
    }

    private void OnTriggerExit(Collider other)
    {
        keyOriAngle = transform.eulerAngles.y;
    }
}
