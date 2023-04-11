using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTest : TestObject<string>
{
    public Switch switchObject;
    public GameObject highlightObject;

    public override string Get()
    {
        return switchObject.Get().ToString();
    }

    public override void highlight(bool state)
    {
        highlightObject.SetActive(state);
    }
}
