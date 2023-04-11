using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keyboard : MonoBehaviour
{
    public string typedText = "";
    public GameObject buttonPrefab;
    private float keyWidth = 1;
    private float keyHeight = 1;

    public TextMeshPro textField;

    //const List<string> keyMap = new List<>{ "" };

    List<List<string>> keyMap = new List<List<string>>{
        new List<string>{ "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "<-" },
        new List<string>{ "A", "S", "D", "F", "G", "Y", "H", "J", "K", "L", "<_/" },
        new List<string>{ "Z", "X", "C", "V", "B", "N", "M", ",", ".", " " }
        };

    private Dictionary<string, GameObject> keyMapDict = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        float x = transform.position.x;
        foreach (List<string> row in keyMap) {
            float z = transform.position.z;
            foreach (string key in row)
            {
                keyMapDict[key] = Instantiate(buttonPrefab, new Vector3(x, transform.position.y, z), new Quaternion(0f, 0f, 0f, 0f));
                Button button_component = keyMapDict[key].GetComponent<Button>();
                button_component.keyText = key;
                button_component.isKeyboardButton = true;
                button_component.keyboard = gameObject.GetComponent<Keyboard>();
                z -= keyWidth;
            }
            x -= keyHeight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        textField.text = typedText;
    }

    public void type(string value)
    {
        if (value == "<-")
        {
            typedText = typedText.Substring(0, typedText.Length - 1);
        }
        else if (value == "<_/")
        {
            typedText += "\n";
        }
        else
        {
            typedText += value;
        }
    } 

    public string Get()
    {
        return typedText;
    }
}
