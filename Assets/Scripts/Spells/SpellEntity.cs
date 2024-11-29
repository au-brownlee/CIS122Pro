using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEntity : MonoBehaviour
{
    private float nextUpdate = 0;

    public int energy = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Time.time + 1;
            UpdateState();
        }
    }

    void UpdateState()
    {
        energy -= 1;
        if (energy <= 0)
        {
            Destroy(gameObject);
        }
    }
}
