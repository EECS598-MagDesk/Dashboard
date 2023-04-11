using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TestObject<T> : MonoBehaviour
{
    public abstract T Get();
    public abstract void highlight(bool state);
}
