using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class TestManager : MonoBehaviour
{
    //public Vector3<>

    public Button finishTestButton;
    public TextMeshPro timeText;
    public TextMeshPro instructionText;
    public TextMeshPro taskText;

    [Serializable]
    public struct testGameObject
    {
        public string name;
        public GameObject gameObject;
    }
    public testGameObject[] testGameObjectArray;

    public Dictionary<string, GameObject> testGameObjects;

    private List<TestCase> testCases;


    IEnumerator TestCoroutine()
    {
        for (int i = 0; i < testCases.Count; i ++)
        {
            taskText.text = "Task: " + i.ToString() + "/" + testCases.Count.ToString();
            instructionText.text = testCases[i].instruction;
            for (int j = 5; j >= 0; j--)
            {
                timeText.text = "Next test start in: " + j.ToString();
                yield return new WaitForSeconds(1f);
            }
            GameObject localObj = new GameObject();
            TestCase test = localObj.AddComponent<TestCase>();
            testCases[i].AssignInitialValue(ref test);
            test.StartTest();
            while (!test.CheckTestStatus())
            {
                if (finishTestButton.Get())
                {
                    test.endSignal = true;
                }
                timeText.text = "Used Time: " + ((int)test.usedTime).ToString();
                yield return new WaitForSeconds(0.2f);
            }
            testCases[i].result = test.result;
            testCases[i].usedTime = test.usedTime;
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
        foreach (TestCase testCase in testCases)
        {
            if (testCase.testType == "Keyboard Speed")
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
        }
        ret += string.Format("Keyboard results:\nTotal word input: {0}. Total time: {1:F2}. WPM: {2:F2}", totalWords, totalTime, totalTime / totalTime * 60);
        ret += string.Format("Other results:\nTotal used time: {0:F2}/{1:F2}. Avg Error: {2:F2}. Total Test: {3}", totalQuantUsedTime, totalQuantTimeLimit, totalQuantError / quantNumber, quantNumber);

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

        TestCase keyboardTest1 = new TestCase();
        keyboardTest1.testTarget = testGameObjects["Keyboard"].GetComponent<KeyboardTest>();
        keyboardTest1.targetValue = "test type";
        keyboardTest1.testType = "Keyboard Speed";
        keyboardTest1.timeLimit = 120f;
        keyboardTest1.instruction = "type: \"test type\" using the keyboard, make sure all the type are correct";
        testCases.Add(keyboardTest1);

        /*
        TestCase keyboardTest2 = new TestCase();
        keyboardTest2.testTarget = testGameObjects["Keyboard"].GetComponent<KeyboardTest>();
        keyboardTest2.targetValue = "test type";
        keyboardTest2.testType = "Keyboard";
        keyboardTest2.timeLimit = 120f;
        keyboardTest2.instruction = "type: \"test type\" using the keyboard, make sure all the type are correct";
        testCases.Add(keyboardTest2);

        TestCase keyboardTest3 = new TestCase();
        keyboardTest3.testTarget = testGameObjects["Keyboard"].GetComponent<KeyboardTest>();
        keyboardTest3.targetValue = "test type";
        keyboardTest3.testType = "Keyboard";
        keyboardTest3.timeLimit = 120f;
        keyboardTest3.instruction = "type: \"test type\" using the keyboard, make sure all the type are correct";
        testCases.Add(keyboardTest3);
        */

        
        TestCase sliderTest1 = new TestCase();
        sliderTest1.testTarget = testGameObjects["S0"].GetComponent<SliderTest>();
        sliderTest1.targetValue = "56";
        sliderTest1.testType = "Slider";
        sliderTest1.timeLimit = 30f;
        sliderTest1.instruction = "set the highlighted slider to " + sliderTest1.targetValue;
        testCases.Add(sliderTest1);
        
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
        switchTest1.testType = "Knob";
        switchTest1.timeLimit = 30f;
        switchTest1.instruction = "set the highlighted switch to " + switchTest1.targetValue;
        testCases.Add(switchTest1);
        

        StartCoroutine(TestCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
