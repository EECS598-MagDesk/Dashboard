using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnobTest : TestObject<string>
{
    public Knob knobObject;
    public GameObject highlightObject;

    public override string Get()
    {
        return knobObject.Get().ToString();
    }

    public override void highlight(bool state)
    {
        highlightObject.SetActive(state);
    }
}
