using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : TestObject<string>
{
    public Line lineObject;
    public GameObject highlightObject;

    public override string Get()
    {
        return lineObject.Get().ToString();
    }

    public override void highlight(bool state)
    {
        lineObject.SetLineOn(state);
    }
}
