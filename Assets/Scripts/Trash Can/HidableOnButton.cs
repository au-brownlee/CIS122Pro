using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HidableOnButton : MonoBehaviour
{
    public KeyCode HideKey;
    public bool visible = true;
    internal Vector3 size;

    // Start is called before the first frame update
    void Start()
    {
        size = transform.localScale;
        TheThing();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(HideKey)) TheThing();
    }

    void TheThing()
    {
        if (visible)
        {
            transform.localScale = new Vector3(0, 0, 0);
            visible = false;
        }
        else
        {
            transform.localScale = size;
            visible = true;
        }
    }
}
