using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCase : MonoBehaviour
{

    public TestObject<string> testTarget;
    public string testType = "Empty";
    public string instruction = "No instruction";
    public float timeLimit = 999;
    public string targetValue;
    public bool endSignal = false;
    public string result;
    //public Coroutine co;
    public bool testFinished = false;
    public float usedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator testCaseCoroutine()
    {
        testTarget.highlight(true);
        usedTime = 0;
        float startTime = Time.time;
        while (Time.time < (startTime + timeLimit))
        {
            if (!endSignal)
            {
                usedTime = Time.time - startTime;
                yield return new WaitForSeconds(0.02f);
            }
            else
            {
                break;
            }
        }
        result = testTarget.Get();
        testTarget.highlight(false);
        testFinished = true;
        yield return null;
    }

    public void StartTest()
    {
        StartCoroutine(testCaseCoroutine());
        //StartCoroutine(testCaseCoroutine());
    }

    public bool CheckTestStatus()
    {
        return testFinished;
    }

    public float GetUsedTime()
    {
        return usedTime;
    }

    public string GetResult()
    {
        return result;
    }

    public void AssignInitialValue(ref TestCase other)
    {
        other.testTarget = testTarget;
        other.testType = testType;
        other.instruction = instruction;
        other.timeLimit = timeLimit;
    }

    public void AssignResultValue(ref TestCase other)
    {
        other.result = result;
        other.usedTime = usedTime;
    }

}
