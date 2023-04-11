using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardTest : TestObject<string>
{
    public Keyboard keyboardObject;

    public override string Get()
    {
        return keyboardObject.Get();
    }

    public override void highlight(bool state)
    {
        
    }
}