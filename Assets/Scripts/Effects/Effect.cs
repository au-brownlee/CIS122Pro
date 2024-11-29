using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect
{
    public string Name;
    public int Amount;
    public int Duration;

    public Effect(string aName, int anAmount, int aDuration)
    {
        Name = aName;
        Amount = anAmount;
        Duration = aDuration;
    }
}
