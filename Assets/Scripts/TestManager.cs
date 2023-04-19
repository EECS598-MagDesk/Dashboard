using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class TestManager : MonoBehaviour
{
    //public Vector3<>

    public Button finishTestButton;
    public Button toggleDashboardButton;
    public GameObject dashboard;
    public GameObject drawingPad;
    public TextMeshPro timeText;
    public TextMeshPro instructionText;
    public TextMeshPro taskText;

    public GameObject linePrefab;

    [Serializable]
    public struct testGameObject
    {
        public string name;
        public GameObject gameObject;
    }
    public testGameObject[] testGameObjectArray;

    public Dictionary<string, GameObject> testGameObjects;

    private List<TestCase> testCases;
    private bool toggleClicked = false;

    public FileHandler fileHandler;

    IEnumerator BlinkTextCO()
    {
        while (true)
        {
            // Set the text color to transparent and wait for a short time
            timeText.color = new Color(1f, 1f, 0f, 1f);
            yield return new WaitForSeconds(0.5f);

            // Set the text color to opaque and wait for a short time
            timeText.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator TestCoroutine()
    {
        fileHandler.ClearFile();
        bool pressFlag = false;
        taskText.text = "Press Next Test to start.";
        while (!(!finishTestButton.Get() && pressFlag))
        {
            if (finishTestButton.Get())
            {
                pressFlag = true;
            }
            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0; i < testCases.Count; i ++)
        {
            taskText.text = "Task: " + (i + 1).ToString() + "/" + testCases.Count.ToString();
            instructionText.text = testCases[i].instruction;
            Coroutine blinkCo = StartCoroutine(BlinkTextCO());
            for (int j = 3; j >= 0; j--)
            {
                timeText.text = "Next test start in: " + j.ToString();
                yield return new WaitForSeconds(1f);
            }
            StopCoroutine(blinkCo);
            timeText.color = new Color(1f, 1f, 1f, 1f);
            GameObject localObj = new GameObject();
            TestCase test = localObj.AddComponent<TestCase>();
            testCases[i].AssignInitialValue(ref test);
            test.StartTest();
            pressFlag = false;
            while (!test.CheckTestStatus())
            {
                if (finishTestButton.Get())
                {
                    pressFlag = true;
                }
                if (!finishTestButton.Get() && pressFlag)
                {

                    test.endSignal = true;
                }
                timeText.text = "Used Time: " + ((int)test.usedTime).ToString();
                yield return new WaitForSeconds(0.02f);
            }
            testCases[i].result = test.result;
            testCases[i].usedTime = test.usedTime;
            string textLog = "Type: " + testCases[i].testType + ", Result: " + testCases[i].result + ", Target: " + testCases[i].targetValue + ", Time: " + testCases[i].usedTime + ", Limit: " + testCases[i].timeLimit;
            fileHandler.AppendLine(textLog);
            Destroy(localObj);
        }
        instructionText.text = "All test finished, thank you for participating.\n";
        instructionText.text += ResultCheck();
        yield return null;
    }

    string ResultCheck()
    {
        int totalWords = 0;
        float totalTime = 0;
        string ret = "";
        float totalQuantError = 0;
        float totalQuantTimeLimit = 0;
        float totalQuantUsedTime = 0;
        float quantNumber = 0;
        float lineError = 0;
        float lineNumber = 0;
        foreach (TestCase testCase in testCases)
        {
            if (testCase.testType == "Keyboard")
            {
                if (testCase.result.Trim().ToLower() != testCase.targetValue.Trim().ToLower())
                {
                    //fail penalty

                }
                totalWords += testCase.targetValue.Split(' ').Length;
                totalTime += testCase.usedTime;
            }
            else if (testCase.testType == "Slider" || testCase.testType == "Knob")
            {
                totalQuantUsedTime += testCase.usedTime;
                totalQuantTimeLimit += testCase.timeLimit;
                Debug.Log(testCase.result);
                float value = float.Parse(testCase.result);
                totalQuantError += Mathf.Abs(value - float.Parse(testCase.targetValue));
                quantNumber++;
            }
            else if (testCase.testType == "Switch")
            {
                totalQuantUsedTime += testCase.usedTime;
                totalQuantTimeLimit += testCase.timeLimit;
                if (testCase.result.Trim().ToLower() != testCase.targetValue.Trim().ToLower())
                {
                    totalQuantError += 1;
                }
                quantNumber++;
            }
            else if (testCase.testType == "Line")
            {
                Debug.Log(testCase.result);
                lineError += float.Parse(testCase.result);
                lineNumber++;
            }
        }
        ret += string.Format("Keyboard results:\nTotal word input: {0}. Total time: {1:F2}. WPM: {2:F2}\n", totalWords, totalTime, totalTime / (float)totalWords * 60f);
        ret += string.Format("Other results:\nTotal used time: {0:F2}/{1:F2}. Avg Error: {2:F2}. Total Test: {3}\n", totalQuantUsedTime, totalQuantTimeLimit, totalQuantError / quantNumber, quantNumber);
        ret += string.Format("Line results:\nAvg error: {0:F2}. Total Test: {1}\n", lineError / lineNumber, lineNumber);
        fileHandler.AppendLine(ret);
        return ret;
    }

    // Start is called before the first frame update
    void Start()
    {
        testGameObjects = new Dictionary<string, GameObject>();
        foreach (testGameObject localTestGameobject in testGameObjectArray)
        {
            testGameObjects[localTestGameobject.name] = localTestGameobject.gameObject;
        }

        testCases = new List<TestCase>();

        // keyboardTest
        /*
        TestCase keyboardTest3 = new TestCase();
        keyboardTest3.testTarget = testGameObjects["Keyboard"].GetComponent<KeyboardTest>();
        keyboardTest3.targetValue = "mauris at fermentum ligula";
        keyboardTest3.testType = "Keyboard";
        keyboardTest3.timeLimit = 120f;
        keyboardTest3.instruction = "type the following sentence using the keyboard, make sure all the type are correct\n" + keyboardTest3.targetValue;
        testCases.Add(keyboardTest3);
        */

        TestCase sliderTest1 = new TestCase();
        sliderTest1.testTarget = testGameObjects["S0"].GetComponent<SliderTest>();
        sliderTest1.targetValue = "56";
        sliderTest1.testType = "Slider";
        sliderTest1.timeLimit = 30f;
        sliderTest1.instruction = "set the highlighted slider to " + sliderTest1.targetValue;
        testCases.Add(sliderTest1);

        TestCase keyboardTest1 = new TestCase();
        keyboardTest1.testTarget = testGameObjects["Keyboard"].GetComponent<KeyboardTest>();
        keyboardTest1.targetValue = "Alanson Sample";
        keyboardTest1.testType = "Keyboard";
        keyboardTest1.timeLimit = 120f;
        keyboardTest1.instruction = "Type the following name using the keyboard, make sure all the letters are correct\n" + keyboardTest1.targetValue;
        testCases.Add(keyboardTest1);

        TestCase knobTest1 = new TestCase();
        knobTest1.testTarget = testGameObjects["K0"].GetComponent<KnobTest>();
        knobTest1.targetValue = "56";
        knobTest1.testType = "Knob";
        knobTest1.timeLimit = 30f;
        knobTest1.instruction = "set the highlighted knob to " + knobTest1.targetValue;
        testCases.Add(knobTest1);

        TestCase switchTest1 = new TestCase();
        switchTest1.testTarget = testGameObjects["Sw0"].GetComponent<SwitchTest>();
        switchTest1.targetValue = "TRUE";
        switchTest1.testType = "Switch";
        switchTest1.timeLimit = 30f;
        switchTest1.instruction = "set the highlighted switch to " + switchTest1.targetValue;
        testCases.Add(switchTest1);



        TestCase sliderTest2 = new TestCase();
        sliderTest2.testTarget = testGameObjects["S4"].GetComponent<SliderTest>();
        sliderTest2.targetValue = "32";
        sliderTest2.testType = "Slider";
        sliderTest2.timeLimit = 30f;
        sliderTest2.instruction = "set the highlighted slider to " + sliderTest2.targetValue;
        testCases.Add(sliderTest2);

        TestCase keyboardTest2 = new TestCase();
        keyboardTest2.testTarget = testGameObjects["Keyboard"].GetComponent<KeyboardTest>();
        keyboardTest2.targetValue = "Human Centered Computing";
        keyboardTest2.testType = "Keyboard";
        keyboardTest2.timeLimit = 120f;
        keyboardTest2.instruction = "Type the following phrase using the keyboard, make sure all the letters and spaces are correct\n" + keyboardTest2.targetValue;
        testCases.Add(keyboardTest2);

        TestCase knobTest2 = new TestCase();
        knobTest2.testTarget = testGameObjects["K5"].GetComponent<KnobTest>();
        knobTest2.targetValue = "28";
        knobTest2.testType = "Knob";
        knobTest2.timeLimit = 30f;
        knobTest2.instruction = "set the highlighted knob to " + knobTest2.targetValue;
        testCases.Add(knobTest2);

        TestCase switchTest2 = new TestCase();
        switchTest2.testTarget = testGameObjects["Sw11"].GetComponent<SwitchTest>();
        switchTest2.targetValue = "TRUE";
        switchTest2.testType = "Switch";
        switchTest2.timeLimit = 30f;
        switchTest2.instruction = "set the highlighted switch to " + switchTest2.targetValue;
        testCases.Add(switchTest2);


        TestCase sliderTest3 = new TestCase();
        sliderTest3.testTarget = testGameObjects["S7"].GetComponent<SliderTest>();
        sliderTest3.targetValue = "95";
        sliderTest3.testType = "Slider";
        sliderTest3.timeLimit = 30f;
        sliderTest3.instruction = "set the highlighted slider to " + sliderTest3.targetValue;
        testCases.Add(sliderTest3);

        TestCase keyboardTest3 = new TestCase();
        keyboardTest3.testTarget = testGameObjects["Keyboard"].GetComponent<KeyboardTest>();
        keyboardTest3.targetValue = "University of Michigan";
        keyboardTest3.testType = "Keyboard";
        keyboardTest3.timeLimit = 120f;
        keyboardTest3.instruction = "Type the following name using the keyboard, make sure all the letters and spaces are correct\n" + keyboardTest3.targetValue;
        testCases.Add(keyboardTest3);

        TestCase knobTest3 = new TestCase();
        knobTest3.testTarget = testGameObjects["K2"].GetComponent<KnobTest>();
        knobTest3.targetValue = "1";
        knobTest3.testType = "Knob";
        knobTest3.timeLimit = 30f;
        knobTest3.instruction = "set the highlighted knob to " + knobTest3.targetValue;
        testCases.Add(knobTest3);

        TestCase switchTest3 = new TestCase();
        switchTest3.testTarget = testGameObjects["Sw8"].GetComponent<SwitchTest>();
        switchTest3.targetValue = "TRUE";
        switchTest3.testType = "Switch";
        switchTest3.timeLimit = 30f;
        switchTest3.instruction = "set the highlighted switch to " + switchTest3.targetValue;
        testCases.Add(switchTest3);


        TestCase sliderTest4 = new TestCase();
        sliderTest4.testTarget = testGameObjects["S2"].GetComponent<SliderTest>();
        sliderTest4.targetValue = "68";
        sliderTest4.testType = "Slider";
        sliderTest4.timeLimit = 30f;
        sliderTest4.instruction = "set the highlighted slider to " + sliderTest4.targetValue;
        testCases.Add(sliderTest4);

        TestCase keyboardTest4 = new TestCase();
        keyboardTest4.testTarget = testGameObjects["Keyboard"].GetComponent<KeyboardTest>();
        keyboardTest4.targetValue = "Magnetic Sensing";
        keyboardTest4.testType = "Keyboard";
        keyboardTest4.timeLimit = 120f;
        keyboardTest4.instruction = "Type the following phrase using the keyboard, make sure all the letters and spaces are correct\n" + keyboardTest4.targetValue;
        testCases.Add(keyboardTest4);

        TestCase knobTest4 = new TestCase();
        knobTest4.testTarget = testGameObjects["K6"].GetComponent<KnobTest>();
        knobTest4.targetValue = "88";
        knobTest4.testType = "Knob";
        knobTest4.timeLimit = 30f;
        knobTest4.instruction = "set the highlighted knob to " + knobTest4.targetValue;
        testCases.Add(knobTest4);

        TestCase switchTest4 = new TestCase();
        switchTest4.testTarget = testGameObjects["Sw12"].GetComponent<SwitchTest>();
        switchTest4.targetValue = "TRUE";
        switchTest4.testType = "Switch";
        switchTest4.timeLimit = 30f;
        switchTest4.instruction = "set the highlighted switch to " + switchTest4.targetValue;
        testCases.Add(switchTest4);


        TestCase sliderTest5 = new TestCase();
        sliderTest5.testTarget = testGameObjects["S6"].GetComponent<SliderTest>();
        sliderTest5.targetValue = "1";
        sliderTest5.testType = "Slider";
        sliderTest5.timeLimit = 30f;
        sliderTest5.instruction = "set the highlighted slider to " + sliderTest5.targetValue;
        testCases.Add(sliderTest5);

        TestCase keyboardTest5 = new TestCase();
        keyboardTest5.testTarget = testGameObjects["Keyboard"].GetComponent<KeyboardTest>();
        keyboardTest5.targetValue = "Gradient Descent";
        keyboardTest5.testType = "Keyboard";
        keyboardTest5.timeLimit = 120f;
        keyboardTest5.instruction = "Type the following phrase using the keyboard, make sure all the letters and spaces are correct\n" + keyboardTest5.targetValue;
        testCases.Add(keyboardTest5);

        TestCase knobTest5 = new TestCase();
        knobTest5.testTarget = testGameObjects["K3"].GetComponent<KnobTest>();
        knobTest5.targetValue = "15";
        knobTest5.testType = "Knob";
        knobTest5.timeLimit = 30f;
        knobTest5.instruction = "set the highlighted knob to " + knobTest5.targetValue;
        testCases.Add(knobTest5);

        TestCase switchTest5 = new TestCase();
        switchTest5.testTarget = testGameObjects["Sw3"].GetComponent<SwitchTest>();
        switchTest5.targetValue = "TRUE";
        switchTest5.testType = "Switch";
        switchTest5.timeLimit = 30f;
        switchTest5.instruction = "set the highlighted switch to " + switchTest5.targetValue;
        testCases.Add(switchTest5);


        TestCase lineTest1 = new TestCase();
        GameObject lineTestObj = Instantiate(linePrefab);
        lineTest1.testTarget = lineTestObj.GetComponent<LineTest>();
        lineTestObj.GetComponent<Line>().startPoint = new Vector3( 3f, 0f, 4f );
        lineTestObj.GetComponent<Line>().endPoint = new Vector3( -8f, 0f, -8f );
        lineTest1.testType = "Line";
        lineTest1.timeLimit = 30f;
        lineTest1.instruction = "Switch to drawing pad. Then trace the line";
        testCases.Add(lineTest1);

        TestCase lineTest2 = new TestCase();
        GameObject lineTestObj2 = Instantiate(linePrefab);
        lineTest2.testTarget = lineTestObj2.GetComponent<LineTest>();
        lineTestObj2.GetComponent<Line>().startPoint = new Vector3(2.5f, 0f, -14f);
        lineTestObj2.GetComponent<Line>().endPoint = new Vector3(-10f, 0f, -15f);
        lineTest2.testType = "Line";
        lineTest2.timeLimit = 30f;
        lineTest2.instruction = "Switch to drawing pad. Then trace the line";
        testCases.Add(lineTest2);

        TestCase lineTest3 = new TestCase();
        GameObject lineTestObj3 = Instantiate(linePrefab);
        lineTest3.testTarget = lineTestObj3.GetComponent<LineTest>();
        lineTestObj3.GetComponent<Line>().startPoint = new Vector3(-3f, 0f, -1f);
        lineTestObj3.GetComponent<Line>().endPoint = new Vector3(-2f, 0f, -9f);
        lineTest3.testType = "Line";
        lineTest3.timeLimit = 30f;
        lineTest3.instruction = "Switch to drawing pad. Then trace the line";
        testCases.Add(lineTest3);

        TestCase lineTest4 = new TestCase();
        GameObject lineTestObj4 = Instantiate(linePrefab);
        lineTest4.testTarget = lineTestObj4.GetComponent<LineTest>();
        lineTestObj4.GetComponent<Line>().startPoint = new Vector3(-1.5f, 0f, 7f);
        lineTestObj4.GetComponent<Line>().endPoint = new Vector3(3f, 0f, -12f);
        lineTest4.testType = "Line";
        lineTest4.timeLimit = 30f;
        lineTest4.instruction = "Switch to drawing pad. Then trace the line";
        testCases.Add(lineTest4);

        TestCase lineTest5 = new TestCase();
        GameObject lineTestObj5 = Instantiate(linePrefab);
        lineTest5.testTarget = lineTestObj5.GetComponent<LineTest>();
        lineTestObj5.GetComponent<Line>().startPoint = new Vector3(-8f, 0f, -3f);
        lineTestObj5.GetComponent<Line>().endPoint = new Vector3(5f, 0f, -3.5f);
        lineTest5.testType = "Line";
        lineTest5.timeLimit = 30f;
        lineTest5.instruction = "Switch to drawing pad. Then trace the line";
        testCases.Add(lineTest5);

        /*
        TestCase lineTest1 = new TestCase();
        GameObject lineTestObj = Instantiate(linePrefab);
        lineTest1.testTarget = lineTestObj.GetComponent<LineTest>();
        lineTestObj.GetComponent<Line>().startPoint = new Vector3(-3f, 0f, -4f);
        lineTestObj.GetComponent<Line>().endPoint = new Vector3(8f, 0f, 8f);
        lineTest1.testType = "Line";
        lineTest1.timeLimit = 30f;
        lineTest1.instruction = "Trace the line";
        testCases.Add(lineTest1);
        */

        StartCoroutine(TestCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (toggleDashboardButton.Get() && !toggleClicked)
        {
            toggleClicked = true;
            drawingPad.SetActive(!drawingPad.activeSelf);
            dashboard.SetActive(!dashboard.activeSelf);
        }
        if (!toggleDashboardButton.Get())
        {
            toggleClicked = false;
        }
    }
}
