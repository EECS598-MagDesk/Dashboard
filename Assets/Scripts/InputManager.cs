using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class InputManager : MonoBehaviour
{

    public string inputDir = "";

    public GameObject controller_prefab;

    GameObject controllerOne;

    Vector3 OneTarget;

    public List<Vector3> testOnePos = new List<Vector3>();

    private float controllerSpeed = 1000f;

    public float originY = 0f;
    public float lengthMulti = 73f;
    public float widthMulti = 36f;
    private float heightMulti = 70f;

    private float prevTime = 0f;
    private float timeOffset = 0.01f;

    public CommunicationManager commManager;

    public bool useTest = false;
    public bool useCommManager = false;

    public bool useMouse = false;
    private float mouseUpHeight = 0.06f;
    private float mouseDownHeight = 0.01f;
    //private float mousePlaneSpeed = 1000f;
    private float mouseVerticalSpeed = 35f;
    
    // Start is called before the first frame update
    void Start()
    {
        
        controllerOne = GameObject.Instantiate(controller_prefab, gameObject.transform);
        controllerOne.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (useMouse)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = - 2f * (mousePosition.x / Screen.width - 0.5f);
            mousePosition.x = 2f * (mousePosition.y / Screen.height - 0.5f);
            if (Input.GetMouseButton(0))
            {
                mousePosition.y = mouseDownHeight;
            }
            else
            {
                mousePosition.y = mouseUpHeight;
            }
            mouseNormalize(ref mousePosition);
            parseInput(mousePosition);
        }
        else
        {
            if (useTest)
            {
                parseInput(testOnePos);
            }
            else
            {
                if (Time.time > prevTime + timeOffset)
                {
                    if (useCommManager)
                    {
                        readCommManager();
                    }
                    else
                    {
                        readInput(inputDir);
                    }
                    prevTime = Time.time;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //controllerOne.transform.position = Vector3.MoveTowards(controllerOne.transform.position, OneTarget, speed * Time.deltaTime);
        Vector3 currentPosition = controllerOne.transform.position;
        Vector3 targetPosition = OneTarget;

        // Move towards target on each axis with different speeds
        float stepX = controllerSpeed * Time.deltaTime;
        float stepY = controllerSpeed * Time.deltaTime;
        float stepZ = controllerSpeed * Time.deltaTime;

        if (useMouse)
        {
            stepY = mouseVerticalSpeed * Time.deltaTime;
        }

        float newX = Mathf.MoveTowards(currentPosition.x, targetPosition.x, stepX);
        float newY = Mathf.MoveTowards(currentPosition.y, targetPosition.y, stepY);
        float newZ = Mathf.MoveTowards(currentPosition.z, targetPosition.z, stepZ);

        controllerOne.transform.position = new Vector3(newX, newY, newZ);
    }

    Vector3 SetYOffset(Vector3 pos)
    {
        return new Vector3(pos[0], pos[1], pos[2]);
    }

    void parseInput (List<Vector3> onePos)
    {
        if (onePos.Count >= 1)
        {
            if (!controllerOne.activeSelf)
            {
                controllerOne.transform.position = SetYOffset(onePos[0]);
                controllerOne.SetActive(true);
            }
            OneTarget = SetYOffset(onePos[0]);
        }
    }

    void parseInput(Vector3 onePos)
    {
        if (!controllerOne.activeSelf)
        {
            controllerOne.transform.position = SetYOffset(onePos);
            controllerOne.SetActive(true);
        }
        OneTarget = SetYOffset(onePos);
    }

    void normalize(ref float x, ref float y, ref float z)
    {
        x = lengthMulti * x;
        y = originY + heightMulti * y;
        z = widthMulti * z;
    }

    void normalize(ref Vector3 v)
    {
        v[0] = lengthMulti * v[0];
        v[1] = originY + heightMulti * v[1];
        v[2] = widthMulti * v[2];
    }

    void mouseNormalize(ref Vector3 v)
    {
        v[0] = 15f * v[0];
        v[1] = originY + 70f * v[1];
        v[2] = 27f * v[2];
    }

    void readCommManager()
    {
        string data = commManager.Get();
        string pattern = @"\([^)]*\)";

        // Match the pattern in the input string from the end using RightToLeft option
        Match lastMatch = Regex.Match(data, pattern, RegexOptions.RightToLeft);

        // Check if a match was found
        if (lastMatch.Success)
        {
            Debug.Log(lastMatch.Value);
        }
        else
        {
            return;
        }

        string[] lines = lastMatch.Value.Split(
            new[] { "\r\n", "\r", "\n" },
            StringSplitOptions.None
        );

        List<Vector3> onePos = new List<Vector3>();

        for (int i = 1; i < lines.Length - 1; i++)
        {
            string line = lines[i];
            string[] parsedLine = line.Split(char.Parse(" "));
            float x = float.Parse(parsedLine[0]);
            float y = float.Parse(parsedLine[2]);
            float z = float.Parse(parsedLine[1]);
            normalize(ref x, ref y, ref z);
            onePos.Add(new Vector3(z, y, -x));
        }
        parseInput(onePos);
    }

    void readInput(string dir)
    {
        List<Vector3> onePos = new List<Vector3>();

        StreamReader reader = new StreamReader(this.inputDir);

        List<string> lines = new List<string>();
        while(!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            lines.Add(line);
            
        }
        reader.Close();
        foreach (string line in lines) {
            //Debug.Log(line);
            string[] parsedLine = line.Split(char.Parse(" "));
            float x = float.Parse(parsedLine[0]);
            float y = float.Parse(parsedLine[2]);
            float z = float.Parse(parsedLine[1]);
            normalize(ref x, ref y, ref z);
            onePos.Add(new Vector3(z, y, -x));
        }

        parseInput(onePos);
        
    }
}
