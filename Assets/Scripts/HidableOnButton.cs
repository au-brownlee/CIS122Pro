using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HidableOnButton : MonoBehaviour
{
    public KeyCode HideKey;
    internal bool visible = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(HideKey))
        {
            if (visible)
            { transform.localScale = new Vector3(0, 0, 0);
                visible = false;
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
                visible = true;
            }
        }
    }
}
