using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    public string Name;
    public float Amount;
    public float Duration;

    public Effect(string aName, float anAmount, float aDuration)
    {
        Name = aName;
        Amount = anAmount;
        Duration = aDuration;
    }
}
