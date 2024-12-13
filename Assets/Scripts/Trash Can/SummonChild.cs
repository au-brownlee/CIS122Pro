using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SummonChild : MonoBehaviour
{
    public KeyCode SummonKey;
    public GameObject Child;
    public GameObject Focus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(SummonKey)) 
        {
            Debug.Log("Summon!");
            var newChild = Instantiate(Child, Focus.transform.position, transform.rotation);
            newChild.transform.parent = transform;
        }
    }
}
