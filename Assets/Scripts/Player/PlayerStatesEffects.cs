using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatesEffects : StatesEffects
{
    public GameObject deathScreen;

    // Start is called before the first frame update
    internal override void Start()
    {
        base.Start();
        effects.Add(new Effect("heat", -1, -1));
        effects.Add(new Effect("feed", -0.3f, -1));
    }

    public override void Die()
    {
        dead = true;
        deathScreen.GetComponent<GameOver>().OpenDeathScreen();
    }
}
