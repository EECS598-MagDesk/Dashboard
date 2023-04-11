using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Button : MonoBehaviour
{
    private float prevHeight = 0f;
    public GameObject key;
    public float triggerVal = 0.5f;
    private float keyOriHeight = 0f;
    private bool triggered = false;
    public string keyText = "A";
    public TextMeshPro text;

    public bool isKeyboardButton = false;
    public Keyboard keyboard;

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

    private void OnTriggerEnter(Collider other)
    {
        prevHeight = other.transform.position.y;
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 keyPos = key.transform.position;
        if ((prevHeight - other.transform.position.y) > triggerVal)
        {
            triggered = true;
        }
        key.transform.position = new Vector3(keyPos.x, keyOriHeight - (prevHeight - other.transform.position.y), keyPos.z);
        //Debug.Log(other.transform.position.y);
    }

    private void OnTriggerExit(Collider other)
    {
        if (isKeyboardButton && triggered)
        {
            keyboard.type(keyText);
        }
        triggered = false;
        Vector3 keyPos = key.transform.position;
        key.transform.position = new Vector3(keyPos.x, keyOriHeight, keyPos.z);
    }

}
