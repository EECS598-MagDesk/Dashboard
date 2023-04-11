using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderTest : TestObject<string>
{
    public Slider sliderObject;
    public GameObject highlightObject;

    public override string Get()
    {
        return sliderObject.Get().ToString();
    }

    public override void highlight(bool state)
    {
        highlightObject.SetActive(state);
    }
}
