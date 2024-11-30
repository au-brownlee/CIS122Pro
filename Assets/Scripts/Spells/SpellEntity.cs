using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEntity : MonoBehaviour
{
    private float nextUpdate = 0;
    float deltaTime = 0.05f;

    public float energy = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Time.time + deltaTime;
            UpdateState();
        }
    }

    void UpdateState()
    {
        energy -= deltaTime;
        if (energy <= 0)
        {
            Destroy(gameObject);
        }
    }
}
